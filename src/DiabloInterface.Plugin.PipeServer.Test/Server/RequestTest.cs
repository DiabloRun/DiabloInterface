using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Zutatensuppe.DiabloInterface.Plugin.PipeServer.Server;

namespace DiabloInterface.Plugin.PipeServer.Server.Test
{
    [TestClass]
    public class RequestTest
    {
        [TestMethod]
        public void TestDeserialize()
        {
            var str = "{\"Resource\": \"characters/current\", \"Payload\": null}";
            var deserialized = JsonConvert.DeserializeObject<Request>(str);

            Assert.AreEqual("characters/current", deserialized.Resource);
            Assert.AreEqual(null, deserialized.Payload);

            str = "{\"Resource\": \"characters/current\"}";
            deserialized = JsonConvert.DeserializeObject<Request>(str);

            Assert.AreEqual("characters/current", deserialized.Resource);
            Assert.AreEqual(null, deserialized.Payload);
        }
    }
}
