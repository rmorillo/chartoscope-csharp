using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

namespace Chartoscope.Core.Messaging.UnitTest
{
    [TestClass]
    public class RequestResponseServiceTest
    {
        [TestMethod]
        public void TestBasicFeature()
        {
            var requestResponseService = new RequestResponseService<string, byte[]>(5555);

            requestResponseService.ReceiveMessage+= ((header, payload) => new MessageFrame<string, byte[]>() { MessageType= "abc", Payload= new byte[] { 123 } });

            CancellationTokenSource cts = new CancellationTokenSource();

            var task= Task.Run(() =>
            {
                requestResponseService.Start(cts.Token);
            });

            var requestResponseClient = new RequestResponseClient<string, byte[]>("127.0.0.1", 5555);

            MessageFrame<string, byte[]> reply = requestResponseClient.SendRequest(new MessageFrame<string, byte[]>() { MessageType="hellomessage", Payload= Encoding.ASCII.GetBytes("Hello World")});            

            requestResponseService.Stop();

            if (!task.IsCompleted)
            {
                cts.Cancel();                
            }

            Assert.AreEqual("abc", reply.MessageType);
            Assert.AreEqual(reply.Payload.Length, 1);
            Assert.AreEqual(reply.Payload[0], 123);
        }
    }
}
