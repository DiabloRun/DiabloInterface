using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Zutatensuppe.DiabloInterface.Core.Logging;

namespace Zutatensuppe.DiabloInterface.Server
{
    public class RequestProcessor
    {
        public QueryResponse HandleRequest(IReadOnlyDictionary<string, Func<IRequestHandler>> handlers, QueryRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Resource))
            {
                return new QueryResponse()
                {
                    Status = QueryStatus.NotFound,
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

            if (resourceData == null || resourceData.Handler == null)
            {
                return new QueryResponse()
                {
                    Resource = resource,
                    Status = QueryStatus.NotFound,
                    Errors = new[] { $"Resource '{resource}' not found." },
                };
            }

            try
            {
                List<string> parameters = new List<string>();
                for (int i = 1; i < resourceData.Match.Groups.Count; ++i)
                    parameters.Add(resourceData.Match.Groups[i].Value);

                var response = resourceData.Handler.HandleRequest(request, parameters);
                if (response == null)
                    throw new RequestHandlerNullResponseException(resource, resourceData.Handler);

                response.Resource = resource;
                return response;
            }
            catch (Exception e)
            {
                Logger.Instance.WriteLine($"Failed to process request [{resource}]{Environment.NewLine}{e}");
                return new QueryResponse()
                {
                    Resource = resource,
                    Status = QueryStatus.Error,
                    Errors = new[] { e.Message },
                };
            }
        }
    }
}
