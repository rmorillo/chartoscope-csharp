using Chartoscope.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Shell
{
    public class BeaconAdminClient: RequestResponseClientBase<AdminServiceHeaderOption, AdminServicePayload>
    {                
        public BeaconAdminClient(string ipAddress, int portNumber):base(ipAddress, portNumber)
        {          
        }        

        public BeaconServiceStatusOption GetStatus()
        {
            
            MessageFrame<AdminServiceHeaderOption, AdminServicePayload> reply = SendRequest(AdminServiceHeaderOption.GetStatus, new AdminServicePayload().AsEmptyPayload());

            return ValidateResponse<BeaconServiceStatusOption>(() => reply.MessageType == AdminServiceHeaderOption.GetStatus, () => reply.Payload.ToServiceStatus(), BeaconServiceStatusOption.Unknown);            
        }

        public TakeOnlineResponseOption TakeOnline()
        {
            MessageFrame<AdminServiceHeaderOption, AdminServicePayload> reply = SendRequest(AdminServiceHeaderOption.TakeOnline, new AdminServicePayload().AsEmptyPayload());

            return ValidateResponse<TakeOnlineResponseOption>(() => reply.MessageType == AdminServiceHeaderOption.TakeOnline, () => reply.Payload.ToTakeOnlineResponse(), TakeOnlineResponseOption.Unknown);
        }
    }
}
