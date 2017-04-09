using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Shell
{
    public class FeedClient
    {
        private const string Binding = "tcp";

        private string _ipAddress;        
        private int _portNumber;
        private string _topic;

        public FeedClient(string ipAddress, int portNumber, string topic)
        {
            _ipAddress = ipAddress;                       
            _portNumber = portNumber;
            _topic = topic;        
        }

        public void Start()
        {
            var endPoint = Binding + "://" + _ipAddress + ":" + _portNumber.ToString();
            using (var subSocket = new SubscriberSocket())
            {
                subSocket.Options.ReceiveHighWatermark = 1000;
                subSocket.Connect(endPoint);
                subSocket.Subscribe(_topic);
                Console.WriteLine("Subscriber socket connecting...");
                while (true)
                {
                    string messageTopicReceived = subSocket.ReceiveFrameString();
                    string messageReceived = subSocket.ReceiveFrameString();
                    Console.WriteLine(messageReceived);
                }
            }
        }

        //public QuoteData GetNextQuote(CurrencyPair currencyPair)
        //{
        //    QuoteData result = null;

        //    ChannelFactory<IFeedService> channelFactory = null;
        //    EndpointAddress ep = null;

        //    string strEPAdr = "http://" + _ipAddress + ":" + _portNumber.ToString() + "/FeedService";
        //    try
        //    {
        //        switch (_binding)
        //        {
        //            case "TCP":
        //                strEPAdr = "net.tcp://" + _ipAddress + ":" + _portNumber.ToString() + "/FeedService";
        //                ep = new EndpointAddress(strEPAdr);
        //                NetTcpBinding tcpb = new NetTcpBinding();
        //                channelFactory = new ChannelFactory<IFeedService>(tcpb);
        //                break;

        //            case "HTTP":
        //                strEPAdr = "http://" + _ipAddress + ":" + _portNumber.ToString() + "/FeedService";
        //                ep = new EndpointAddress(strEPAdr);
        //                BasicHttpBinding httpb = new BasicHttpBinding();
        //                channelFactory = new ChannelFactory<IFeedService>(httpb);
        //                break;
        //        }

        //        IFeedService feedClientProxy = channelFactory.CreateChannel(ep);
        //        result = feedClientProxy.GetQuote(Enum.GetName(typeof(CurrencyPair), currencyPair));

        //        channelFactory.Close();
        //    }
        //    catch (Exception eX)
        //    {
        //        Console.WriteLine("Error while performing operation [" + eX.Message + "] \n\n Inner Exception [" + eX.InnerException + "]");
        //    }

        //    return result;
        //}

        //public void AppendPriceBars(CurrencyPair currencyPair, PriceBarData[] priceBars)
        //{
        //    ChannelFactory<IFeedService> channelFactory = null;
        //    EndpointAddress ep = null;

        //    string strEPAdr = "http://" + _ipAddress + ":" + _portNumber.ToString() + "/FeedService";
        //    try
        //    {
        //        switch (_binding)
        //        {
        //            case "TCP":
        //                strEPAdr = "net.tcp://" + _ipAddress + ":" + _portNumber.ToString() + "/FeedService";
        //                ep = new EndpointAddress(strEPAdr);
        //                NetTcpBinding tcpb = new NetTcpBinding();
        //                channelFactory = new ChannelFactory<IFeedService>(tcpb);
        //                break;

        //            case "HTTP":
        //                strEPAdr = "http://" + _ipAddress + ":" + _portNumber.ToString() + "/FeedService";
        //                ep = new EndpointAddress(strEPAdr);
        //                BasicHttpBinding httpb = new BasicHttpBinding();
        //                channelFactory = new ChannelFactory<IFeedService>(httpb);
        //                break;
        //        }

        //        IFeedService feedClientProxy = channelFactory.CreateChannel(ep);

        //        feedClientProxy.AppendPriceBars(currencyPair, priceBars);

        //        channelFactory.Close();
        //    }
        //    catch (Exception eX)
        //    {
        //        Console.WriteLine("Error while performing operation [" + eX.Message + "] \n\n Inner Exception [" + eX.InnerException + "]");
        //    }
        //}

        //public PriceBarData[] GetLatestPriceHistory(CurrencyPair currencyPair, int limit)
        //{
        //    PriceBarData[] result = null;

        //    ChannelFactory<IFeedService> channelFactory = null;
        //    EndpointAddress ep = null;

        //    string strEPAdr = "http://" + _ipAddress + ":" + _portNumber.ToString() + "/FeedService";
        //    try
        //    {
        //        switch (_binding)
        //        {
        //            case "TCP":
        //                strEPAdr = "net.tcp://" + _ipAddress + ":" + _portNumber.ToString() + "/FeedService";
        //                ep = new EndpointAddress(strEPAdr);
        //                NetTcpBinding tcpb = new NetTcpBinding();
        //                channelFactory = new ChannelFactory<IFeedService>(tcpb);
        //                break;

        //            case "HTTP":
        //                strEPAdr = "http://" + _ipAddress + ":" + _portNumber.ToString() + "/FeedService";
        //                ep = new EndpointAddress(strEPAdr);
        //                BasicHttpBinding httpb = new BasicHttpBinding();
        //                channelFactory = new ChannelFactory<IFeedService>(httpb);
        //                break;
        //        }

        //        IFeedService feedClientProxy = channelFactory.CreateChannel(ep);
        //        result = feedClientProxy.GetLatestPriceHistory(currencyPair, limit);

        //        channelFactory.Close();
        //    }
        //    catch (Exception eX)
        //    {
        //        Console.WriteLine("Error while performing operation [" + eX.Message + "] \n\n Inner Exception [" + eX.InnerException + "]");
        //    }

        //    return result;
        //}
    }
}
