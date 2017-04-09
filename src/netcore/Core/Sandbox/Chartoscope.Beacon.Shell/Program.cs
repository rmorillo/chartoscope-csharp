using Chartoscope.Beacon.Shell;
using System;

class Program
{
    static void Main(string[] args)
    {
        if (args[0] == "PUB")
        {
            var feedPortNumber = Convert.ToInt32(args[1]);
            var adminPortNumber= Convert.ToInt32(args[2]);
            var serviceHost = new ServiceHost(feedPortNumber, adminPortNumber);

            serviceHost.Start();
        }
        else if (args[0] == "SUB")
        {
            var client = new FeedClient("localhost", Convert.ToInt32(args[1]), args[2]);

            client.Start();
        }
    }
}