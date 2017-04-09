using Chartoscope.Core.Messaging;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Chartoscope.Beacon.Shell
{
    public class ServiceHost
    {
        private const string Binding = "tcp";
        private const string Localhost = "localhost";

        private FeedService _feedService;
        private BeaconAdminService _adminService;
        private int _adminPortNumber;
        private int _feedPortNumber;
        public ServiceHost(int feedPortNumber, int adminPortNumber)
        {
            _feedPortNumber = feedPortNumber;
            _adminPortNumber = adminPortNumber;
            _feedService = new FeedService();
            _adminService = new BeaconAdminService(_adminPortNumber)
                            .SetupTakeOnlineHandler(BringOnline);            
        }

        public void Start()
        {
            _adminService.Start();                                   
        }
        
        private void BringOnline()
        {
            var endPoint = Binding + "://" + Localhost + ":" + _feedPortNumber.ToString();

            using (var pubSocket = new PublisherSocket())
            {
                Console.WriteLine("Publisher socket binding...");
                pubSocket.Options.SendHighWatermark = 1000;
                pubSocket.Bind(endPoint);

                while (true)
                {
                    foreach (CurrencyPairOption currencyPair in Enum.GetValues(typeof(CurrencyPairOption)))
                    {
                        string forexPair = Enum.GetName(typeof(CurrencyPairOption), currencyPair);
                        QuoteData quote = _feedService.GetQuote(forexPair);

                        pubSocket.SendMoreFrame(forexPair).SendFrame(string.Format("bid:{0}, ask:{1}", quote.Bid, quote.Ask));
                    }

                    Thread.Sleep(1000);
                }
            }
        }

        public void Stop()
        {

        }
    }
}
