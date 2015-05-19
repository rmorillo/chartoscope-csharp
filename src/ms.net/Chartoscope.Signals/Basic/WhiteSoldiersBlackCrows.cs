using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //http://forex-strategies-revealed.com/3white-soldiers-3black-crows
    //Forex trading strategy #12 (3 white soldiers / 3 black crows)
    public class WhiteSoldiersBlackCrows: SignalBase
    {
        public const string IDENTITY_CODE = "3_white_soldiers_3_black_crows";
        private BollingerBands bb = null;

        public WhiteSoldiersBlackCrows(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            bb = new BollingerBands(barType, 20);

            Register(bb);
        }

        private bool IsThreeWhiteSoldiers(PriceBars priceBar)
        {
            return priceBar.LastItem.BullishCandle() && priceBar.Last(1).BullishCandle() && priceBar.Last(2).BullishCandle();
        }

        private bool IsThreeBlackCrows(PriceBars priceBar)
        {
            return priceBar.LastItem.BearishCandle() && priceBar.Last(1).BearishCandle() && priceBar.Last(2).BearishCandle();
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {
            return priceBar.LastItem.High > bb.UpperBand() && priceBar.LastItem.Close < bb.UpperBand() && IsThreeWhiteSoldiers(priceBar);
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return priceBar.LastItem.Low < bb.LowerBand() && priceBar.LastItem.Close > bb.LowerBand() && IsThreeBlackCrows(priceBar);
        }

        private bool bouncedOff = false;
        private double bouncedOffPreviousClose = double.NaN;

        private PositionMode lastPosition;

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
            if (bouncedOff)
            {
                if (lastPosition == PositionMode.Short)
                {                    
                    Enter(PositionMode.Long);                    
                }
                else
                {
                    Enter(PositionMode.Short);
                }
            }
            else
            {
                if (IsLongSetup(priceBar))
                {
                    Enter(PositionMode.Long);
                }
                else if (IsShortSetup(priceBar))
                {
                    Enter(PositionMode.Short);
                }
            }

        }

        private bool BouncesOff(PositionMode position, PriceBars priceBar)
        {
            return (position == PositionMode.Long && priceBar.LastItem.Open < bb.UpperBand() && priceBar.LastItem.BearishCandle())
                || (position == PositionMode.Short && priceBar.LastItem.Open > bb.LowerBand() && priceBar.LastItem.BullishCandle());
        }

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if (bouncedOff)
            {
                if ((position==PositionMode.Long && (priceBar.LastItem.High>bb.UpperBand()))
                    || (position==PositionMode.Short && (priceBar.LastItem.Low<bb.LowerBand())))
                {
                    bouncedOff= false;
                    bouncedOffPreviousClose = double.NaN;
                    Exit();
                }
            }
            else
            {
                if (BouncesOff(position, priceBar))
                {
                    lastPosition = position;
                    bouncedOff = true;
                    bouncedOffPreviousClose = priceBar.Last(1).Close;
                    Exit();
                }
            }
        }
    }
}
