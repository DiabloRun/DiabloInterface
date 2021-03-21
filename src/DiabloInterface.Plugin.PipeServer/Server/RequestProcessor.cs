using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Zutatensuppe.DiabloInterface.Lib;

namespace Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server
{
    public class RequestProcessor
    {
        static readonly ILogger Logger = Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private List<string> ToParameters(Match match)
        {
            var parameters = new List<string>();
            for (var i = 1; i < match.Groups.Count; ++i)
                parameters.Add(match.Groups[i].Value);
            return parameters;
        }

        public Response HandleRequest(
            IReadOnlyDictionary<string, Func<IRequestHandler>> handlers,
            Request request
        ) {
            if (string.IsNullOrEmpty(request?.Resource))
            {
                return new Response()
                {
                    Status = ResponseStatus.NotFound,
                    Errors = new[] { "No resource specified." },
                };
            }

            string resource = Request.NormalizeResource(request.Resource);

            var resourceData = (
                from pair in handlers
                let match = Regex.Match(resource, pair.Key, RegexOptions.Singleline)
                where match.Success && match.Length == resource.Length
                select new
                {
                    Handler = pair.Value(),
                    Params = ToParameters(match),
                }
            ).FirstOrDefault();

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
                var response = resourceData.Handler.HandleRequest(request, resourceData.Params);
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
