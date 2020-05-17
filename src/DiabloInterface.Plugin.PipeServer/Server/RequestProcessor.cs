using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Zutatensuppe.DiabloInterface.Core.Logging;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server
{
    public class RequestProcessor
    {
        static readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        public Response HandleRequest(IReadOnlyDictionary<string, Func<IRequestHandler>> handlers, Request request)
        {
            if (string.IsNullOrEmpty(request?.Resource))
            {
                return new Response()
                {
                    Status = ResponseStatus.NotFound,
                    Errors = new[] { "No resource specified." },
                };
            }

            // Get first matching resource.
            string resource = request.Resource.Trim().ToLowerInvariant();
            var resourceData = (from pair in handlers
                                let match = Regex.Match(resource, pair.Key, RegexOptions.Singleline)
                                where match.Success && match.Length == resource.Length
                                select new
                                {
                                    Handler = pair.Value(),
                                    Match = match
                                }).FirstOrDefault();

            if (resourceData?.Handler == null)
            {
                return new Response()
                {
                    Resource = resource,
                    Status = ResponseStatus.NotFound,
                    Errors = new[] { $"Resource '{resource}' not found." },
                };
            }

            try
            {
                var parameters = new List<string>();
                for (var i = 1; i < resourceData.Match.Groups.Count; ++i)
                    parameters.Add(resourceData.Match.Groups[i].Value);

                var response = resourceData.Handler.HandleRequest(request, parameters);
                if (response == null)
                    throw new RequestHandlerNullResponseException(resource, resourceData.Handler);

                response.Resource = resource;
                return response;
            }
            catch (Exception e)
            {
                Logger.Error($"Failed to process request [{resource}]", e);
                return new Response()
                {
                    Resource = resource,
                    Status = ResponseStatus.Error,
                    Errors = new[] { e.Message },
                };
            }
        }
    }
}
