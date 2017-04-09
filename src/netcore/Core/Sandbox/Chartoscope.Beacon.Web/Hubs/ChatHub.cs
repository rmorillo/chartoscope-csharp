using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;

namespace Chartoscope.Beacon.Web
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        public ChatHub()
        {

        }
        public void SendMessage(string message)
        {
            string name = Context.User.Identity.Name;
            Clients.All.onMessageReceived(DateTime.Now.ToString(), name, message);
        }

        public override Task OnConnected()
        {
            //string name = Context.User.Identity.Name;
            //Groups.Add(Context.ConnectionId, name);
            return base.OnConnected();
        }
    }
}