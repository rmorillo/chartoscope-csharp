using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class RenkoBars: LookBehindPool<IRenkoBar>, INotifyUpdate<IRenkoBar, RenkoPriceOption>, IByteArrayEncoder
    {
        public event Delegates.BarUpdateNotificationEventHandler<IRenkoBar> BarUpdated;
        public event Delegates.PriceUpdateNotificationEventHandler<RenkoPriceOption, double> PriceUpdated;

        private IPersistenceWriter _persistenceWriter;

        private int _brickValue;
        private double _lastClosingPrice= 0;
        private double _lastClosingPricePoint = 0;
        private double _pricePoint;
        public RenkoBars(int capacity, int brickValue, double pricePoint): base(capacity)
        {
            _brickValue = brickValue;
            _pricePoint = pricePoint;
        }

        public RenkoBars(int capacity, int brickValue, double pricePoint, IPersistenceWriter persistenceService) : base(capacity)
        {
            _brickValue = brickValue;
            _pricePoint = pricePoint;

            _persistenceWriter = persistenceService;
            _persistenceWriter.Initialize(RenkoBarEntity.Header.ToByteArray());
        }

        protected override IRenkoBar CreatePoolItem()
        {
            return new RenkoBar(DateTime.MinValue.Ticks, 0, 0);
        }

        public void Write(long timeStamp, double closingPrice)
        {
            PriceAction(timeStamp, closingPrice);
        }

        private void PriceAction(long timeStamp, double closingPrice)
        {
            double closingPricePoint = closingPrice / _pricePoint;

            if (_lastClosingPrice == 0)
            {
                _lastClosingPrice = closingPrice;
                _lastClosingPricePoint = closingPricePoint;
            }
            else
            {
                double pricePointDelta = closingPricePoint - _lastClosingPricePoint;                

                if (Math.Abs(pricePointDelta) + 1 >= _brickValue)
                {
                    UpdatePool(timeStamp, _lastClosingPrice, closingPrice);                        
                   
                    _lastClosingPrice = closingPrice;
                    _lastClosingPricePoint = closingPricePoint;
                }                
            }

            BarUpdated?.Invoke(Current);
            PriceUpdated?.Invoke(Current.Timestamp, RenkoPriceOption.Low, Current.Close);   
        }

        private void UpdatePool(long timeStamp, double open, double close)
        {
            ((RenkoBar)NextPoolItem).Write(timeStamp, open, close);            
            MoveNext();
        }
    }
}
