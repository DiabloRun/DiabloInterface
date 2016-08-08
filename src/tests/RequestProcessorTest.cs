using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Zutatensuppe.DiabloInterface.Server;

namespace tests
{
    [TestClass]
    public class RequestProcessorTest
    {
        RequestProcessor requestProcessor = new RequestProcessor();

        class RequestHandlerMock : IRequestHandler
        {
            public static IList<string> PreviousArguments { get; private set; }

            public QueryResponse HandleRequest(QueryRequest request, IList<string> arguments)
            {
                PreviousArguments = arguments;
                return new QueryResponse()
                {
                    Status = QueryStatus.Success,
                };
            }
        }

        [TestMethod]
        public void RequestProcessorResolvesNormalResources()
        {
            Dictionary<string, Func<IRequestHandler>> handlers = new Dictionary<string, Func<IRequestHandler>>();
            handlers.Add("test", () => new RequestHandlerMock());

            var request = new QueryRequest() { Resource = "test" };
            var response = requestProcessor.HandleRequest(handlers, request);

            Assert.AreEqual(QueryStatus.Success, response.Status);
        }

        [TestMethod]
        public void RequestProcessorMatchesEntireString()
        {
            Dictionary<string, Func<IRequestHandler>> handlers = new Dictionary<string, Func<IRequestHandler>>();
            handlers.Add("test", () => new RequestHandlerMock());

            var request = new QueryRequest() { Resource = "test/test" };
            var response = requestProcessor.HandleRequest(handlers, request);

            Assert.AreEqual(QueryStatus.NotFound, response.Status);
        }

        [TestMethod]
        public void RequestProcessorResolvesRegexResources()
        {
            Dictionary<string, Func<IRequestHandler>> handlers = new Dictionary<string, Func<IRequestHandler>>();
            handlers.Add(@"test/(\w+)$", () => new RequestHandlerMock());

            var request = new QueryRequest() { Resource = "test/item" };
            var response = requestProcessor.HandleRequest(handlers, request);

            Assert.AreEqual(QueryStatus.Success, response.Status);
            Assert.AreEqual(1, RequestHandlerMock.PreviousArguments.Count);
            Assert.AreEqual("item", RequestHandlerMock.PreviousArguments[0]);
        }
    }
}
