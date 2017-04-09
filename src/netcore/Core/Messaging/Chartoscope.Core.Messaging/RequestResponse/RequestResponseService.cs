using NetMQ;
using NetMQ.Sockets;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chartoscope.Core.Messaging
{
    public class RequestResponseService<THeader, TPayload>
    {
        private int _tcpPort;

        private bool _running;

        private ResponseSocket _activeResponder;
        private string _activeEndPoint;

        public delegate MessageFrame<THeader, TPayload> ReceiveMessageHandler(THeader header, TPayload payload);

        public event ReceiveMessageHandler ReceiveMessage;

        public RequestResponseService(int tcpPort)
        {
            _tcpPort = tcpPort;
            _activeEndPoint = "tcp://*:" + _tcpPort.ToString();
        }

        public void Start(CancellationToken cancellationToken)
        {           
            using (_activeResponder = new ResponseSocket())
            {
                // Bind
                _activeResponder.Bind(_activeEndPoint);

                _running = true;

                while (_running && !cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        // Receive
                        byte[] request = _activeResponder.ReceiveFrameBytes();
                        
                        MessageFrame<THeader, TPayload> messageFrame = MessageSerializer.Deserialize<MessageFrame<THeader, TPayload>>(request);

                        if (ReceiveMessage != null)
                        {
                            MessageFrame<THeader, TPayload> reply = ReceiveMessage(messageFrame.MessageType, messageFrame.Payload);

                            // Do some work
                            Thread.Sleep(1);

                            // Send
                            _activeResponder.SendFrame(MessageSerializer.ObjectToByteArray(reply),false);
                        }
                        
                    }
                    catch (Exception)
                    {
                        if (_running)
                        {
                            throw;
                        }
                    }

                }
            }

            _activeResponder = null;
        }

        public void Stop()
        {
            _running = false;
                        
            if (_activeEndPoint != null)
            {                
                _activeResponder.Unbind(_activeEndPoint);
                _activeResponder.Close();
            }            
        }        
    }
}
