using Chartoscope.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Chartoscope.Beacon.Shell
{
    public abstract class RequestResponseServiceBase<THeader, TPayload>
    {
        protected RequestResponseService<THeader, TPayload> _serviceRequest;

        private CancellationTokenSource _cts;
        private int _portNumber;

        public RequestResponseServiceBase(int portNumber)
        {
            _portNumber = portNumber;
            _cts = new CancellationTokenSource();
        }
        public virtual void Start()
        {
            _serviceRequest = new RequestResponseService<THeader, TPayload>(_portNumber);
            _serviceRequest.ReceiveMessage += OnReceiveMessage;
            _serviceRequest.Start(_cts.Token);
        }

        protected abstract TPayload GetResponse(THeader header, TPayload payload);

        private MessageFrame<THeader, TPayload> OnReceiveMessage(THeader header, TPayload payload)
        {
            TPayload response = GetResponse(header, payload);            

            return new MessageFrame<THeader, TPayload>()
            {
                MessageType = header,
                Payload = response
            };
        }
        public void Stop()
        {

        }        
    }
}
