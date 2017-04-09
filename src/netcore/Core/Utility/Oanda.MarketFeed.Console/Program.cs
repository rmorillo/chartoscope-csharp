using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Oanda.MarketFeed.Console;

class Program
{
    private const int AccountId = 6184634;
    private const string TestInstrument = "EUR_USD";

    private static string[] _majors = new string[] {"EUR_USD", "USD_CAD"};

    public static object RESTClient { get; private set; }
    private static Semaphore _tickReceived;
    private static Semaphore _eventReceived;
    static void Main(string[] args)
    {       
        List<Instrument> instruments= null;
        var task= Task.Run(async () => instruments = await Rest.GetInstrumentsAsync(AccountId, null, new List<string>(_majors)));
        task.Wait();

        var selectedMajors = instruments.Where(s=> Array.Exists<string>(_majors, y=> y==s.instrument));
        RatesSession session = new RatesSession(AccountId, new List<Instrument>(selectedMajors));
        _tickReceived = new Semaphore(0, 100);
        session.DataReceived += SessionOnDataReceived;
        session.StartSession();
        Console.WriteLine("Starting rate stream test");
        bool success = _tickReceived.WaitOne(10000);
        session.StopSession();


        EventsSession eventSession = new EventsSession(AccountId);
        _eventReceived = new Semaphore(0, 100);
        eventSession.DataReceived += OnEventReceived;
        eventSession.StartSession();
        Console.WriteLine("Starting event stream test");        
        Task.Run(() =>
                {
                    success = _eventReceived.WaitOne(10000);
                    eventSession.StopSession();            
                }
            );

        List<Price> prices;

        var task2 = Task.Run(async () => prices = await Rest.GetRatesAsync(instruments));
        task2.Wait();


        List<Position> positions= null;

        var task3 = Task.Run(async () =>  positions= await Rest.GetPositionsAsync(AccountId));
        task3.Wait();

        var request = new Dictionary<string, string>
                    {
                        {"instrument", TestInstrument},
                        {"units", "10000"},
                        {"side", "sell"},
                        {"type", "market"},
                        {"price", "1.0"}
                    };

        if (positions.Count == 0)
        {
            //Open a position
            PostOrderResponse response = null;
            var task4 = Task.Run(async () => response = await Rest.PostOrderAsync(AccountId, request));
            task4.Wait();

            if (response.tradeOpened != null && response.tradeOpened.id > 0)
            {
                Console.WriteLine("Post order success");
            }
        }        
        else
        {
            //Close all positions
            foreach (var position in positions)
            {
                DeletePositionResponse closePositionResponse = null;
                var task5 = Task.Run(async () => closePositionResponse = await Rest.DeletePositionAsync(AccountId, TestInstrument));
                task5.Wait();

                if (closePositionResponse.ids.Count > 0 && closePositionResponse.instrument == TestInstrument)
                {
                    Console.WriteLine("Position closed");
                }

                if (closePositionResponse.totalUnits > 0 && closePositionResponse.price > 0)
                {
                    Console.WriteLine("Position close response seems valid");
                }    
                
                foreach(var id in closePositionResponse.ids)
                {
                    Transaction transaction = null;
                    var task6= Task.Run(async () => transaction = await Rest.GetTransactionDetailsAsync(AccountId, id));
                    task6.Wait();
                }
            }
        }

        Console.ReadLine();
    }

    private static void SessionOnDataReceived(RateStreamResponse data)
    {
        // _results.Verify the tick data
       // _results.Verify(data.tick != null, "Streaming Tick received");
        if (data.tick != null)
        {
            Console.WriteLine("Instrument: {0}, Bid: {1}, Ask: {2}", data.tick.instrument, data.tick.bid, data.tick.ask);
            //_results.Verify(data.tick.ask > 0 && data.tick.bid > 0, "Streaming tick has bid/ask");
            //_results.Verify(!string.IsNullOrEmpty(data.tick.instrument), "Streaming tick has instrument");
        }
        //_tickReceived.Release();
    }

    private static void OnEventReceived(Event data)
    {
        // _results.Verify the event data
        //_results.Verify(data.transaction != null, "Event transaction received");
        if (data.transaction != null)
        {
            Console.WriteLine("Instrument: {0}, Side: {1}, Account Balance: {2}", data.transaction.instrument, data.transaction.side, data.transaction.accountBalance);
            //_results.Verify(data.transaction.id != 0, "Event data received");
            //_results.Verify(data.transaction.accountId != 0, "Account id received");
        }
        //_eventReceived.Release();
    }
}
