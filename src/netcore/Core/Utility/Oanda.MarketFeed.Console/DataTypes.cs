using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Oanda.MarketFeed.Console
{
    public class IsOptionalAttribute : Attribute
    {
        public override string ToString()
        {
            return "Is Optional";
        }
    }

    public class MaxValueAttribute : Attribute
    {
        public object Value { get; set; }
        public MaxValueAttribute(int i)
        {
            Value = i;
        }
    }

    public class MinValueAttribute : Attribute
    {
        public object Value { get; set; }
        public MinValueAttribute(int i)
        {
            Value = i;
        }
    }

    public class Instrument
    {
        public bool HasInstrument;
        private string _instrument;
        public string instrument
        {
            get { return _instrument; }
            set
            {
                _instrument = value;
                HasInstrument = true;
            }
        }

        public bool HasdisplayName;
        private string _displayName;
        public string displayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                HasdisplayName = true;
            }
        }

        public bool Haspip;
        private string _pip;
        public string pip
        {
            get { return _pip; }
            set
            {
                _pip = value;
                Haspip = true;
            }
        }

        [IsOptional]
        public bool HaspipLocation;
        private int _pipLocation;
        public int pipLocation
        {
            get { return _pipLocation; }
            set
            {
                _pipLocation = value;
                HaspipLocation = true;
            }
        }

        [IsOptional]
        public bool HasextraPrecision;
        private int _extraPrecision;
        public int extraPrecision
        {
            get { return _extraPrecision; }
            set
            {
                _extraPrecision = value;
                HasextraPrecision = true;
            }
        }

        public bool HasmaxTradeUnits;
        private int _maxTradeUnits;
        public int maxTradeUnits
        {
            get { return _maxTradeUnits; }
            set
            {
                _maxTradeUnits = value;
                HasmaxTradeUnits = true;
            }
        }
    }

    public class InstrumentsResponse
    {
        public List<Instrument> instruments;
    }

    public class PricesResponse
    {
        public long time;
        public List<Price> prices;
    }

    public class Price
    {
        public enum State
        {
            Default,
            Increasing,
            Decreasing
        };

        public string instrument { get; set; }
        public string time;
        public double bid { get; set; }
        public double ask { get; set; }
        public string status;
        public State state = State.Default;

        public void update(Price update)
        {
            if (this.bid > update.bid)
            {
                state = State.Decreasing;
            }
            else if (this.bid < update.bid)
            {
                state = State.Increasing;
            }
            else
            {
                state = State.Default;
            }

            this.bid = update.bid;
            this.ask = update.ask;
            this.time = update.time;
        }
    }

    public class Position
    {
        public string side { get; set; }
        public string instrument { get; set; }
        public int units { get; set; }
        public double avgPrice { get; set; }
    }

    public class PositionsResponse
    {
        public List<Position> positions;
    }

    public class Common
    {
        public static object GetDefault(Type t)
        {
            return typeof(Common).GetTypeInfo().GetDeclaredMethod("GetDefaultGeneric").MakeGenericMethod(t).Invoke(null, null);
        }

        public static T GetDefaultGeneric<T>()
        {
            return default(T);
        }
    }
    public class Response
    {
        public override string ToString()
        {
            // use reflection to display all the properties that have non default values
            StringBuilder result = new StringBuilder();
            var props = this.GetType().GetTypeInfo().DeclaredProperties;
            result.AppendLine("{");
            foreach (var prop in props)
            {
                if (prop.Name != "Content" && prop.Name != "Subtitle" && prop.Name != "Title" && prop.Name != "UniqueId")
                {
                    object value = prop.GetValue(this);
                    bool valueIsNull = value == null;
                    object defaultValue = Common.GetDefault(prop.PropertyType);
                    bool defaultValueIsNull = defaultValue == null;
                    if ((valueIsNull != defaultValueIsNull) // one is null when the other isn't
                        || (!valueIsNull && (value.ToString() != defaultValue.ToString()))) // both aren't null, so compare as strings
                    {
                        result.AppendLine(prop.Name + " : " + prop.GetValue(this));
                    }
                }
            }
            result.AppendLine("}");
            return result.ToString();
        }
    }

    public class PostOrderResponse : Response
    {
        public string instrument { get; set; }
        public string time { get; set; }
        public double? price { get; set; }

        public Order orderOpened { get; set; }
        public TradeData tradeOpened { get; set; }
        public List<Transaction> tradesClosed { get; set; } // TODO: verify this
        public Transaction tradeReduced { get; set; } // TODO: verify this
    }

    public class Order : Response
    {
        public long id { get; set; }
        public string instrument { get; set; }
        public int units { get; set; }
        public string side { get; set; }
        public string type { get; set; }
        public string time { get; set; }
        public double price { get; set; }
        public double takeProfit { get; set; }
        public double stopLoss { get; set; }
        public string expiry { get; set; }
        public double upperBound { get; set; }
        public double lowerBound { get; set; }
        public int trailingStop { get; set; }
    }

    public class TradeData : Response
    {
        public long id { get; set; }
        public int units { get; set; }
        public string side { get; set; }
        public string instrument { get; set; }
        public string time { get; set; }
        public double price { get; set; }
        public double takeProfit { get; set; }
        public double stopLoss { get; set; }
        public int trailingStop { get; set; }
        public double trailingAmount { get; set; }
    }

    public class Transaction : Response
    {
        public long id { get; set; }
        public int accountId { get; set; }
        public string time { get; set; }
        public string type { get; set; }
        public string instrument { get; set; }
        public string side { get; set; }
        public int units { get; set; }
        public double price { get; set; }
        public double lowerBound { get; set; }
        public double upperBound { get; set; }
        public double takeProfitPrice { get; set; }
        public double stopLossPrice { get; set; }
        public double trailingStopLossDistance { get; set; }
        public double pl { get; set; }
        public double interest { get; set; }
        public double accountBalance { get; set; }
        public long tradeId { get; set; }
        public long orderId { get; set; }
        public TradeData tradeOpened { get; set; }
        public TradeData tradeReduced { get; set; }
        public string reason { get; set; }
        public string expiry { get; set; }

        /// <summary>
        /// Gets a basic title for the type of transaction
        /// </summary>
        /// <returns></returns>
        public string GetTitle()
        {
            switch (type)
            {
                case "CloseOrder":
                    return "Order Closed";
                case "SellLimit":
                    return "Sell Limit Order Created";
                case "BuyLimit":
                    return "Buy Limit Order Created";
            }
            return type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetReadableString()
        {
            string readable = units + " " + instrument + " at " + price;
            if (pl != 0)
            {
                readable += "\nP/L: " + pl;
            }
            return readable;
        }
    }

    public class DeletePositionResponse : Response
    {
        public List<long> ids { get; set; }
        public string instrument { get; set; }
        public int totalUnits { get; set; }
        public double price { get; set; }
    }

    public class RatesSession : StreamSession<RateStreamResponse>
    {
        private readonly List<Instrument> _instruments;

        public RatesSession(int accountId, List<Instrument> instruments) : base(accountId)
        {
            _instruments = instruments;
        }

        protected override async Task<WebResponse> GetSession()
        {
            return await Rest.StartRatesSession(_instruments, _accountId);
        }
    }

    public abstract class StreamSession<T> where T : IHeartbeat
    {
        protected readonly int _accountId;
        private WebResponse _response;
        private bool _shutdown;

        public delegate void DataHandler(T data);

        public event DataHandler DataReceived;

        public void OnDataReceived(T data)
        {
            DataHandler handler = DataReceived;
            if (handler != null) handler(data);
        }

        protected StreamSession(int accountId)
        {
            _accountId = accountId;
        }

        protected abstract Task<WebResponse> GetSession();

        public async void StartSession()
        {
            _shutdown = false;
            _response = await GetSession();


            await Task.Run(() =>
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    StreamReader reader = new StreamReader(_response.GetResponseStream());
                    while (!_shutdown)
                    {
                        MemoryStream memStream = new MemoryStream();

                        string line = reader.ReadLine();
                        memStream.Write(Encoding.UTF8.GetBytes(line), 0, Encoding.UTF8.GetByteCount(line));
                        memStream.Position = 0;


                        var data = (T)serializer.ReadObject(memStream);

                        // Don't send heartbeats
                        if (!data.IsHeartbeat())
                        {
                            OnDataReceived(data);
                        }

                    }
                }
            );

        }

        public void StopSession()
        {
            _shutdown = true;
        }
    }

    public interface IHeartbeat
    {
        bool IsHeartbeat();
    }

    public class RateStreamResponse : IHeartbeat
    {
        public Heartbeat heartbeat;
        public Price tick;
        public bool IsHeartbeat()
        {
            return (heartbeat != null);
        }
    }

    public class Heartbeat
    {
        public string time { get; set; }
    }

    public class Event : IHeartbeat
    {
        public Heartbeat heartbeat { get; set; }
        public Transaction transaction { get; set; }
        public bool IsHeartbeat()
        {
            return (heartbeat != null);
        }
    }

    public class EventsSession : StreamSession<Event>
    {
        public EventsSession(int accountId) : base(accountId)
        {
        }

        protected override async Task<WebResponse> GetSession()
        {
            return await Rest.StartEventsSession(new List<int> { _accountId });
        }
    }
}
