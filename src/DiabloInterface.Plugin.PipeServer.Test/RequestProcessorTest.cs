using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace DiabloInterface.Plugin.PipeServer.Test
{
    [TestClass]
    public class RequestProcessorTest
    {
        private static IRequestHandler CreateHandler(string id)
        {
            var handler = new Mock<IRequestHandler>();
            handler
                .Setup(h => h.HandleRequest(It.IsAny<Request>(), It.IsAny<IList<string>>()))
                .Returns((Request r, IList<string> args) => new Response()
                {
                    Status = ResponseStatus.Success,
                    Payload = new { args, id },
                });
            return handler.Object;
        }
        
        [TestMethod]
        public void RequestProcessorResolvesNormalResources()
        {
            var handlers = new Dictionary<string, Func<IRequestHandler>>();
            handlers.Add("test", () => CreateHandler("first"));

            var requestProcessor = new RequestProcessor();
            var request = new Request() { Resource = "test" };
            var response = requestProcessor.HandleRequest(handlers, request);

            Assert.AreEqual(ResponseStatus.Success, response.Status);

            Assert.AreEqual(0, ((List<string>)((dynamic)response.Payload).args).Count);
            Assert.AreEqual("first", (string)((dynamic)response.Payload).id);

            Assert.AreEqual(null, response.Errors);

            Assert.AreEqual("test", response.Resource);
        }

        [TestMethod]
        public void RequestProcessorMatchesEntireString()
        {
            var handlers = new Dictionary<string, Func<IRequestHandler>>();
            handlers.Add("test", () => CreateHandler("first"));

            var requestProcessor = new RequestProcessor();
            var request = new Request() { Resource = "test/test" };
            var response = requestProcessor.HandleRequest(handlers, request);

            Assert.AreEqual(ResponseStatus.NotFound, response.Status);

            Assert.AreEqual(null, response.Payload);

            Assert.AreEqual(1, ((string[])response.Errors).Length);
            Assert.AreEqual("Resource 'test/test' not found.", ((string[])response.Errors)[0]);

            Assert.AreEqual("test/test", response.Resource);
        }

        [TestMethod]
        public void RequestProcessorMatchesNoResource()
        {
            var handlers = new Dictionary<string, Func<IRequestHandler>>();
            handlers.Add("test", () => CreateHandler("first"));

            var requestProcessor = new RequestProcessor();
            var request = new Request() { };
            var response = requestProcessor.HandleRequest(handlers, request);

            Assert.AreEqual(ResponseStatus.NotFound, response.Status);

            Assert.AreEqual(null, response.Payload);

            Assert.AreEqual(1, ((string[])response.Errors).Length);
            Assert.AreEqual("No resource specified.", ((string[])response.Errors)[0]);

            Assert.AreEqual(null, response.Resource);
        }

        [TestMethod]
        public void RequestProcessorResolvesRegexResources()
        {
            var handlers = new Dictionary<string, Func<IRequestHandler>>();
            handlers.Add(@"test/(\w+)$", () => CreateHandler("first"));
            handlers.Add("test/item", () => CreateHandler("second"));

            var requestProcessor = new RequestProcessor();
            var request = new Request() { Resource = "test/item" };
            var response = requestProcessor.HandleRequest(handlers, request);

            Assert.AreEqual(ResponseStatus.Success, response.Status);

            Assert.AreEqual(1, ((List<string>)((dynamic)response.Payload).args).Count);
            Assert.AreEqual("item", ((List<string>)((dynamic)response.Payload).args)[0]);
            Assert.AreEqual("first", (string)((dynamic)response.Payload).id);

            Assert.AreEqual(null, response.Errors);

            Assert.AreEqual("test/item", response.Resource);
        }
    }
}
