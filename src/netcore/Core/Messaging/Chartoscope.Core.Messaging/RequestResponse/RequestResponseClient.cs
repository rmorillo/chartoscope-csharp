using NetMQ;
using NetMQ.Sockets;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chartoscope.Core.Messaging
{
    public class RequestResponseClient<THeader, TPayload>
    {
        private const string Binding = "tcp";

        private string _ipAddress;
        private int _tcpPort;
        private readonly byte[] _outgoingByte = new byte[0];
        private byte[] _incomingByte = null;

        public RequestResponseClient(string ipAddress, int tcpPort)
        {
            _ipAddress = ipAddress;
            _tcpPort = tcpPort;
        }

        public bool IsListening()
        {
            var isListening = false;
            var endPoint = Binding + "://" + _ipAddress + ":" + _tcpPort.ToString();

            using (var requester = new RequestSocket(endPoint))
            {
                if (requester.TrySendFrame(TimeSpan.FromSeconds(2), _outgoingByte) &&
                        requester.TryReceiveFrameBytes(TimeSpan.FromSeconds(2), out _incomingByte))
                {
                    isListening= true;
                }

                requester.Disconnect(endPoint);
            }

            return isListening;
        }

        public MessageFrame<THeader, TPayload> SendRequest(MessageFrame<THeader, TPayload> requestMessage)
        {
            var endPoint = Binding + "://" + _ipAddress + ":" + _tcpPort.ToString();

            using (var requester = new RequestSocket(endPoint))
            {
                // Connect
                requester.Connect(endPoint);                              

                // Send
                requester.SendFrame(ObjectToByteArray(requestMessage), false);

                // Receive
                //TODO: Implement time-out
                byte[] response = requester.ReceiveFrameBytes();
                
                requestMessage = Deserialize<MessageFrame<THeader, TPayload>>(response);                

                requester.Disconnect(endPoint);
            }

            return requestMessage;
        }

        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);
                return ms.ToArray();
            };            
        }

        private T Deserialize<T>(byte[] input)
        {
            using (MemoryStream ms = new MemoryStream(input))
            {                
                ms.Seek(0, SeekOrigin.Begin);
                return Serializer.Deserialize<T>(ms);
            }

        }
    }
}
