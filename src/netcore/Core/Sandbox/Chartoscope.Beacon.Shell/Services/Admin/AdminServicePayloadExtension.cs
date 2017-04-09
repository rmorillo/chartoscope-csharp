using Chartoscope.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Shell
{
    public static class AdminServicePayloadExtension
    {
        public static BeaconServiceStatusOption ToServiceStatus(this AdminServicePayload payload)
        {
            return MessageSerializer.Deserialize<BeaconServiceStatusOption>(payload.Data);
        }

        public static AdminServicePayload AsSetServiceStatus(this AdminServicePayload payload, BeaconServiceStatusOption status)
        {
            payload.Data= MessageSerializer.ObjectToByteArray(status);
            return payload;
        }

        public static AdminServicePayload AsTakeOnlineResponse(this AdminServicePayload payload, TakeOnlineResponseOption takeOnlineResponse)
        {
            payload.Data = MessageSerializer.ObjectToByteArray(takeOnlineResponse);
            return payload;
        }
        public static TakeOnlineResponseOption ToTakeOnlineResponse(this AdminServicePayload payload)
        {
            return MessageSerializer.Deserialize<TakeOnlineResponseOption>(payload.Data);
        }

        public static AdminServicePayload AsEmptyPayload(this AdminServicePayload payload)
        {
            payload.Data = new byte[0];
            return payload;     
        }
    }
}
