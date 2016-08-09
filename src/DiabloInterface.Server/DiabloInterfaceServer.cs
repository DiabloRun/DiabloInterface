using Zutatensuppe.DiabloInterface.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Converters;

namespace Zutatensuppe.DiabloInterface.Server
{
    public class DiabloInterfaceServer
    {
        string pipeName;
        Thread listenThread;

        Dictionary<string, Func<IRequestHandler>> requestHandlers = new Dictionary<string, Func<IRequestHandler>>();
        public IReadOnlyDictionary<string, Func<IRequestHandler>> RequestHandlers => requestHandlers;

        public DiabloInterfaceServer(string pipeName)
        {
            this.pipeName = pipeName;

            listenThread = new Thread(new ThreadStart(ServerListen));
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        /// <summary>
        /// Adds or overrides a request handler resource.
        /// </summary>
        /// <param name="resource">The resource to handle.</param>
        /// <param name="handlerFactory">The factory that creates a new handler for each request.</param>
        public void AddRequestHandler(string resource, Func<IRequestHandler> handlerFactory)
        {
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentNullException(nameof(resource));
            if (handlerFactory == null)
                throw new ArgumentNullException(nameof(handlerFactory));

            // Always use lower case resources.
            resource = resource.Trim().ToLowerInvariant();
            requestHandlers[resource] = handlerFactory;
        }

        /// <summary>
        /// Removes a request handler if it exists. Does nothing if the resource does not exist.
        /// </summary>
        /// <param name="resource"></param>
        public void RemoveRequestHandler(string resource)
        {
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentNullException(nameof(resource));

            resource = resource.Trim().ToLowerInvariant();
            if (requestHandlers.ContainsKey(resource))
                requestHandlers.Remove(resource);
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
                    Logger.Instance.WriteLine("ItemServer Error: {0}", e.Message);

                    if (pipe != null) pipe.Close();
                }
            }
        }

        void HandleClientConnection(NamedPipeServerStream pipe)
        {
            // Read query request.
            var reader = new JsonStreamReader(pipe, Encoding.UTF8);
            var request = reader.ReadJson<QueryRequest>();

            // Get response and write.
            var response = HandleRequest(request);
            var writer = new JsonStreamWriter(pipe, Encoding.UTF8,
                new IsoDateTimeConverter());
            writer.WriteJson(response);
            writer.Flush();
        }

        QueryResponse HandleRequest(QueryRequest request)
        {
            var processor = new RequestProcessor();
            return processor.HandleRequest(requestHandlers, request);
        }
    }
}
