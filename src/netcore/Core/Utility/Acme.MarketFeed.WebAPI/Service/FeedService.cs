using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.MarketFeed.WebAPI
{
    public class FeedService
    {
        private Dictionary<CurrencyPairOption, PriceGenerator> _priceGenerator;
        public FeedService()
        {
            _priceGenerator = new Dictionary<CurrencyPairOption, PriceGenerator>();

            string[] currencyPairs = Enum.GetNames(typeof(CurrencyPairOption));

            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            foreach (string currencyPair in currencyPairs)
            {
                int initialNumber = random.Next(800, 1300);
                _priceGenerator.Add((CurrencyPairOption)Enum.Parse(typeof(CurrencyPairOption), currencyPair), new PriceGenerator(_priceGenerator.Count, 4, 10, initialNumber, 0.004, 0.002, 3));
            }
        }

        public QuoteData GetQuote(string currencyPair)
        {
            var currencyPairEnum = (CurrencyPairOption)Enum.Parse(typeof(CurrencyPairOption), currencyPair);

            var nextQuote = _priceGenerator[currencyPairEnum].NextQuote;

            return nextQuote;
        }
    }
}
