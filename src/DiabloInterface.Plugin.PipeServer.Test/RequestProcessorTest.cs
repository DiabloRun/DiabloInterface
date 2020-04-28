using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace DiabloInterface.Plugin.PipeServer.Test
{
    [TestClass]
    public class RequestProcessorTest
    {
        readonly RequestProcessor requestProcessor = new RequestProcessor();

        class RequestHandlerMock : IRequestHandler
        {
            public static IList<string> PreviousArguments { get; private set; }

            public Response HandleRequest(Request request, IList<string> arguments)
            {
                PreviousArguments = arguments;
                return new Response()
                {
                    Status = ResponseStatus.Success,
                };
            }
        }

        [TestMethod]
        public void RequestProcessorResolvesNormalResources()
        {
            var handlers = new Dictionary<string, Func<IRequestHandler>>();
            handlers.Add("test", () => new RequestHandlerMock());

            var request = new Request() { Resource = "test" };
            var response = requestProcessor.HandleRequest(handlers, request);

            Assert.AreEqual(ResponseStatus.Success, response.Status);
        }

        [TestMethod]
        public void RequestProcessorMatchesEntireString()
        {
            var handlers = new Dictionary<string, Func<IRequestHandler>>();
            handlers.Add("test", () => new RequestHandlerMock());

            var request = new Request() { Resource = "test/test" };
            var response = requestProcessor.HandleRequest(handlers, request);

            Assert.AreEqual(ResponseStatus.NotFound, response.Status);
        }

        [TestMethod]
        public void RequestProcessorResolvesRegexResources()
        {
            var handlers = new Dictionary<string, Func<IRequestHandler>>();
            handlers.Add(@"test/(\w+)$", () => new RequestHandlerMock());

            var request = new Request() { Resource = "test/item" };
            var response = requestProcessor.HandleRequest(handlers, request);

            Assert.AreEqual(ResponseStatus.Success, response.Status);
            Assert.AreEqual(1, RequestHandlerMock.PreviousArguments.Count);
            Assert.AreEqual("item", RequestHandlerMock.PreviousArguments[0]);
        }
    }
}
