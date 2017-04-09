using Chartoscope.Core.Messaging;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chartoscope.Beacon.Shell
{
    public class BeaconAdminService: RequestResponseServiceBase<AdminServiceHeaderOption, AdminServicePayload>
    {                
        private BeaconServiceStatusOption _serviceStatus = BeaconServiceStatusOption.Offline;
        private Action _takeOnlineHandler= null;
        
        public BeaconAdminService(int portNumber):base(portNumber)
        {                        
        }

        public BeaconAdminService SetupTakeOnlineHandler(Action takeOnlineHandler)
        {
            _takeOnlineHandler = takeOnlineHandler;
            return this;
        }

        protected override AdminServicePayload GetResponse(AdminServiceHeaderOption header, AdminServicePayload payload)
        {
            switch (header)
            {
                case AdminServiceHeaderOption.GetStatus:
                    return  GetStatus();
                case AdminServiceHeaderOption.TakeOnline:
                    return TakeOnline();
                default:
                    return null;
            }
        }

        private AdminServicePayload GetStatus()
        {
            return new AdminServicePayload().AsSetServiceStatus(_serviceStatus);
        }

        private AdminServicePayload TakeOnline()
        {
            if (_takeOnlineHandler == null)
            {
                return new AdminServicePayload().AsTakeOnlineResponse(TakeOnlineResponseOption.Failure);
            }
            else
            {
                Task.Run(()=> _takeOnlineHandler());
                _serviceStatus = BeaconServiceStatusOption.Online;
                return new AdminServicePayload().AsTakeOnlineResponse(TakeOnlineResponseOption.Success);
            }
        }
    }
}
