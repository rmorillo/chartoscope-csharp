using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chartoscope.Beacon.Common;
using Chartoscope.Utility.RestClient;
using Oanda.Common.Data;
using System.Text;

namespace Oanda.MarketFeed.BeaconProvider
{
    public class ForexMarketFeed : IBeaconProvider
    {
        private const string InstrumentsRequestBasePath = "instruments?accountId={0}";
        private IRestClient _restClient;        
        private int _accountId;
        private Instruments _instruments;
        private CurrencyPairNameResolver _currencyPairNameResolver;
        private string _streamingRatesUri;
        private string _accessToken;
        private IEnumerable<string> _instrumentList;
        private string _resolvedInstrumentCodes;
        private JsonWebStream<RateStreamResponse> _webStream;
        private PriceTickPool _priceTickPool;

        public event DelegateDefinitions.TickDataHandler TickReceived;

        public ForexMarketFeed(IRestClient restClient, IDictionary<string, string> config)
        {
            _restClient = restClient;
            _accountId = int.Parse(config["AccountId"]);
            _streamingRatesUri = config["StreamingRatesUri"];
            _accessToken= config["AccessToken"];
            var selectedInstruments = config["SelectedInstruments"];
            _instrumentList = selectedInstruments.Split(',');            
            _instruments = null;

            _priceTickPool = new PriceTickPool(100);
        }
        public IInstrumentNameResolver InstrumentNameResolver
        {
            get
            {
                return _currencyPairNameResolver;
            }
        }

        public void Initialize()
        {            
            var instrumentsRequest = new JsonGetRequest<Instruments>(string.Format(InstrumentsRequestBasePath, _accountId));
            var response= _restClient.Get(instrumentsRequest);

            response.Wait();

            _instruments = response.Result;

            _currencyPairNameResolver = new CurrencyPairNameResolver(_instruments.instruments);

            var instrumentCodes = new StringBuilder();
            foreach (var instrumentCode in _instrumentList)
            {
                var nameResolution = _currencyPairNameResolver.Resolve(instrumentCode);
                if (nameResolution.Status == InstrumentNameResolutionResultOption.Success)
                {
                    if (instrumentCodes.Length == 0)
                    {
                        instrumentCodes.Append(nameResolution.Mapping.ProviderInstrumentCode);
                    }
                    else
                    {
                        instrumentCodes.Append(',');
                        instrumentCodes.Append(nameResolution.Mapping.ProviderInstrumentCode);
                    }
                }
            }

            _resolvedInstrumentCodes = instrumentCodes.ToString();            
        }

        public void Startup()
        {
            if (_instruments==null)
            {
                throw new Exception("Not initialized");
            }
            else
            {
                var session = new JsonWebSession(_streamingRatesUri, _accessToken);
                var path = "prices?accountId=" + _accountId + "&instruments=" + Uri.EscapeDataString(_resolvedInstrumentCodes);
                _webStream = new JsonWebStream<RateStreamResponse>(path, session);
                _webStream.DataReceived += OnDataReceived;
                Task.Run(async ()=> await _webStream.BeginReceiveStreamAsync());
            }
        }

        public void Shutdown()
        {
            _webStream.EndReceiveStream();
        }

        private void OnDataReceived(RateStreamResponse data)
        {
            _priceTickPool.Write(DateTime.Parse(data.tick.time).Ticks, data.tick.ask, data.tick.bid);            
            TickReceived(_priceTickPool.Current);
        }
    }
}
