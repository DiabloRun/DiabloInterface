using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Zutatensuppe.DiabloInterface.Lib;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server
{
    public class Server
    {
        static readonly ILogger Logger = Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string pipeName;
        private readonly RequestProcessor requestProcessor;
        private readonly Dictionary<string, Func<IRequestHandler>> requestHandlers;
        private readonly Cache cache;
        private readonly uint cacheMs;
        private readonly PipeSecurity ps;
        private NamedPipeServerStream stream;

        public bool Running { get; private set; }

        public Server(string pipeName, uint cacheMs)
        {
            Logger.Info("Initializing pipe server.");

            this.pipeName = pipeName;

            requestProcessor = new RequestProcessor();
            requestHandlers = new Dictionary<string, Func<IRequestHandler>>();

            this.cache = new Cache();
            this.cacheMs = cacheMs;

            ps = new PipeSecurity();
            var sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.BuiltinUsersSid, null);
            ps.AddAccessRule(new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
        }

        public void AddRequestHandler(string resource, Func<IRequestHandler> handler)
        {
            requestHandlers[Request.NormalizeResource(resource)] = handler;
        }

        public void Start()
        {
            try
            {
                Listen();
                Running = true;
            }
            catch (Exception e)
            {
                Logger.Error("Pipe server error", e);
                Running = false;
            }
        }

        public void Stop()
        {
            stream?.Close();
            Running = false;
        }

        private void Listen()
        {
            stream = new NamedPipeServerStream(pipeName,
                PipeDirection.InOut, 1,
                PipeTransmissionMode.Message,
                PipeOptions.Asynchronous,
                1024, 1024, ps);
            stream.BeginWaitForConnection(WaitForConnectionCallBack, stream);
        }

        private void WaitForConnectionCallBack(IAsyncResult iar)
        {
            try
            {
                // Get the pipe
                NamedPipeServerStream stream = (NamedPipeServerStream)iar.AsyncState;
                // End waiting for the connection
                stream.EndWaitForConnection(iar);

                HandleClientConnection(stream);

                stream.WaitForPipeDrain();
                stream.Disconnect();
                stream.Close();
                stream = null;

                // Listen for next message
                Listen();
                Running = true;
            }
            catch (ObjectDisposedException e)
            {
                // this is expected to happen when the server is stopped
                // while we wait for connection
                Logger.Info("ObjectDisposedException", e);
                Running = false;
            }
            catch (Exception e)
            {
                Logger.Error("Pipe server error", e);
                Running = false;
            }
        }

        private void HandleClientConnection(NamedPipeServerStream pipe)
        {
            var reader = new JsonStreamReader(pipe, Encoding.UTF8);
            var requestJsonString = reader.ReadJsonString();
            var response = BuildResponse(requestJsonString);
            var writer = new JsonStreamWriter(pipe);
            writer.WriteJson(response);
            writer.Flush();
        }

        private object BuildResponse(string requestJsonString)
        {
            var legacyRequest = JsonConvert.DeserializeObject<LegacyRequest>(requestJsonString);

            if (!string.IsNullOrEmpty(legacyRequest.EquipmentSlot))
            {
                return new LegacyResponse(HandleRequest(legacyRequest.AsRequest()));
            }

            var request = JsonConvert.DeserializeObject<Request>(requestJsonString);
            return HandleRequest(request);
        }

        private Response HandleRequest(Request request)
        {
            if (cacheMs == 0)
                return HandleRequestUncached(request);

            string cacheKey = request?.Resource + "|" + request?.Payload?.ToString();
            var resp = cache.Get(cacheKey);
            if (resp == null)
            {
                resp = HandleRequestUncached(request);
                cache.Set(cacheKey, resp, cacheMs);
            }
            return (Response)resp;
        }

        private Response HandleRequestUncached(Request request)
        {
            return requestProcessor.HandleRequest(requestHandlers, request);
        }
    }
}
