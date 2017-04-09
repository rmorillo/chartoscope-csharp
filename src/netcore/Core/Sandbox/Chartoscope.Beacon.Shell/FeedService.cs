using System;
using System.Collections.Generic;
using System.Text;

namespace Chartoscope.Beacon.Shell
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

        //public void AppendPriceBars(Guid currencyPair, PriceBarData[] priceBars)
        //{

        //    var textWriter = new StreamWriter(@"C:\Temp\" + Enum.GetName(typeof(CurrencyPair), currencyPair) + ".csv", true);
        //    var csv = new CsvWriter(textWriter);
        //    foreach (var item in priceBars)
        //    {
        //        csv.WriteRecord(item);
        //    }

        //    textWriter.Close();
        //}

        //public PriceBarData[] GetLatestPriceHistory(CurrencyPair currencyPair, int limit)
        //{
        //    var result = new List<PriceBarData>();

        //    var textReader = new StreamReader(@"C:\Temp\" + Enum.GetName(typeof(CurrencyPair), currencyPair) + ".csv");

        //    int lineCount = 0;
        //    while (!textReader.EndOfStream)
        //    {
        //        textReader.ReadLine();
        //        lineCount++;
        //    }

        //    textReader.BaseStream.Position = 0;

        //    if (lineCount > limit)
        //    {
        //        for (int i = 0; i < lineCount - limit; i++)
        //        {
        //            textReader.ReadLine();
        //        }
        //    }

        //    var csv = new CsvReader(textReader);
        //    while (csv.Read())
        //    {
        //        var timeStamp = csv.GetField<DateTime>(0);
        //        var open = csv.GetField<double>(1);
        //        var high = csv.GetField<double>(2);
        //        var low = csv.GetField<double>(3);
        //        var close = csv.GetField<double>(4);

        //        result.Add(new PriceBarData() { Timestamp = timeStamp, Open = open, High = high, Low = low, Close = close });
        //    }

        //    textReader.Close();

        //    result.Reverse();

        //    return result.ToArray();
        //}
    }
}
