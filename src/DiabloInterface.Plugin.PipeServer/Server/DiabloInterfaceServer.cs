using Zutatensuppe.DiabloInterface.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server
{
    public class DiabloInterfaceServer
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string pipeName;
        private readonly Cache cache;
        private readonly PipeSecurity ps;
        NamedPipeServerStream stream;
        public bool Running { get; private set; }

        readonly Dictionary<string, Func<IRequestHandler>> requestHandlers = new Dictionary<string, Func<IRequestHandler>>();
        public IReadOnlyDictionary<string, Func<IRequestHandler>> RequestHandlers => requestHandlers;

        public DiabloInterfaceServer(string pipeName)
        {
            Logger.Info("Initializing pipe server.");

            this.pipeName = pipeName;

            this.cache = new Cache();

            ps = new PipeSecurity();
            System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.BuiltinUsersSid, null);
            ps.AddAccessRule(new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
        }

        public void AddRequestHandler(string resource, Func<IRequestHandler> handler)
        {
            // Always use lower case resources.
            resource = resource.Trim().ToLowerInvariant();
            requestHandlers[resource] = handler;
        }

        public void Start()
        {
            try
            {
                ServerListen();
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

        private void ServerListen()
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
                ServerListen();
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

        Response HandleRequest(Request request)
        {
            string cacheKey = request?.Resource + "|" + request?.Payload?.ToString();
            var resp = cache.Get(cacheKey);
            if (resp == null)
            {
                resp = new RequestProcessor().HandleRequest(
                    requestHandlers,
                    request
                );
                cache.Set(cacheKey, resp, 2000);
            }
            return (Response)resp;
        }
    }
}
