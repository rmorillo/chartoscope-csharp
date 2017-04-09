using Chartoscope.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Shell
{
    public class RequestResponseClientBase<THeader, TPayload>
    {
        protected RequestResponseClient<THeader, TPayload> _client;
        private string _ipAddress;
        private int _portNumber;

        protected byte[] EmptyPayload = new byte[0];

        public RequestResponseClientBase(string ipAddress, int portNumber)
        {
            _ipAddress = ipAddress;
            _portNumber = portNumber;

            _client = new RequestResponseClient<THeader, TPayload>(_ipAddress, _portNumber);
        }

        public bool IsListening()
        {
            return _client.IsListening();
        }

        protected MessageFrame<THeader, TPayload> SendRequest(THeader header, TPayload payload)
        {
            return _client.SendRequest(new MessageFrame<THeader, TPayload>() { MessageType = header, Payload = payload });
        }
        protected T ValidateResponse<T>(Func<bool> isValidMessage, Func<T> response, T defaultValue)
        {
            if (isValidMessage())
            {
                return response();
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
