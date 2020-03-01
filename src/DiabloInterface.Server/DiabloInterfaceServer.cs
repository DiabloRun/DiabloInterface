using Zutatensuppe.DiabloInterface.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Converters;

namespace Zutatensuppe.DiabloInterface.Server
{
    public class DiabloInterfaceServer
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        readonly string pipeName;
        Thread listenThread;

        readonly Dictionary<string, Func<IRequestHandler>> requestHandlers = new Dictionary<string, Func<IRequestHandler>>();
        public IReadOnlyDictionary<string, Func<IRequestHandler>> RequestHandlers => requestHandlers;

        public DiabloInterfaceServer(string pipeName)
        {
            Logger.Info("Initializing pipe server.");

            this.pipeName = pipeName;

            listenThread = new Thread(ServerListen) {IsBackground = true};
            listenThread.Start();
        }

        public void AddRequestHandler(string resource, Func<IRequestHandler> handler)
        {
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentNullException(nameof(resource));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            // Always use lower case resources.
            resource = resource.Trim().ToLowerInvariant();
            requestHandlers[resource] = handler;
        }

        public void Stop()
        {
            if (listenThread != null)
            {
                listenThread.Abort();
                listenThread = null;
            }
        }

        void ServerListen()
        {
            var ps = new PipeSecurity();
            System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.BuiltinUsersSid, null);
            ps.AddAccessRule(new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));

            while (true)
            {
                NamedPipeServerStream pipe = null;
                try
                {
                    pipe = new NamedPipeServerStream(pipeName,
                        PipeDirection.InOut, 1,
                        PipeTransmissionMode.Message,
                        PipeOptions.Asynchronous,
                        1024, 1024, ps);
                    pipe.WaitForConnection();
                    HandleClientConnection(pipe);
                    pipe.Close();
                }
                catch (UnauthorizedAccessException e )
                {
                    // note: should only come here if another pipe with same name is already open (= another instance of d2interface is running)
                    Console.WriteLine("Error: {0}", e.Message);
                    Thread.Sleep(1000); // try again in 1 sec to prevent tool from lagging
                }
                catch (IOException e)
                {
                    Logger.Error("Item Server Failure", e);
                    pipe?.Close();
                }
            }
        }

        void HandleClientConnection(NamedPipeServerStream pipe)
        {
            // Read query request.
            var reader = new JsonStreamReader(pipe, Encoding.UTF8);
            var legacyRequest = reader.ReadJson<LegacyRequest>();
            if (!string.IsNullOrEmpty(legacyRequest.EquipmentSlot))
            {
                writeResponse(pipe, new LegacyResponse(HandleRequest(legacyRequest.AsRequest())));
            } else
            {
                writeResponse(pipe, HandleRequest(reader.ReadJson<Request>()));
            }
        }

        void writeResponse(NamedPipeServerStream pipe, object response)
        {
            // Get response and write.
            var writer = new JsonStreamWriter(pipe, Encoding.UTF8,
                new IsoDateTimeConverter());
            writer.WriteJson(response);
            writer.Flush();
        }

        Response HandleRequest(Request request)
        {
            return new RequestProcessor().HandleRequest(requestHandlers, request);
        }
    }
}
