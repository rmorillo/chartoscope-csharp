using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{

    //http://forex-strategies-revealed.com/4h-bollinger-band-strategy
    //Forex trading strategy #10 (H4 Bollinger Band Strategy)
    public class H4BollingerBandStrategy: SignalBase
    {
        public const string IDENTITY_CODE = "h4_bollinger";
        private BollingerBands bb = null;

        public H4BollingerBandStrategy(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            bb = new BollingerBands(barType, 20);

            Register(bb);
        }

        private bool upperBandPierced = false;
        private bool lowerBandPierced = false;

        private int piercingBar = int.MinValue;
        private double piercingBarBandWidth= double.NaN;

        private bool IsLongSetup(PriceBars priceBar)
        {
            if (!lowerBandPierced)
            {
                lowerBandPierced = bb.LowerBand() > priceBar.LastItem.Low && bb.LowerBand() < priceBar.LastItem.Close;
                if (lowerBandPierced)
                {
                    upperBandPierced = false;
                    piercingBar = priceBar.LastItem.GetHashCode();
                    piercingBarBandWidth = bb.UpperBand() - bb.LowerBand();
                }
            }
            return lowerBandPierced && priceBar.LastItem.GetHashCode() != piercingBar && priceBar.LastItem.Close < bb.UpperBand() && (bb.UpperBand() - bb.LowerBand()) > piercingBarBandWidth;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            if (!upperBandPierced)
            {
                upperBandPierced = bb.UpperBand() < priceBar.LastItem.High && bb.UpperBand() > priceBar.LastItem.Close;               
                if (upperBandPierced)
                {
                    lowerBandPierced = false;
                    piercingBar = priceBar.LastItem.GetHashCode();
                    piercingBarBandWidth = bb.UpperBand() - bb.LowerBand();
                }
            }
            return upperBandPierced && priceBar.LastItem.GetHashCode() != piercingBar && priceBar.LastItem.Close < bb.UpperBand() && (bb.UpperBand() - bb.LowerBand())>piercingBarBandWidth;
        }

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
        {
            if (IsLongSetup(priceBar))
            {
                Enter(PositionMode.Long);
                lowerBandPierced = false;
                upperBandPierced = false;
            }
            else if (IsShortSetup(priceBar))
            {
                Enter(PositionMode.Short);
                lowerBandPierced = false;
                upperBandPierced = false;
            }

        }

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if (priceBar.LastItem.Low<bb.LowerBand() || priceBar.LastItem.High>bb.UpperBand())
            {
                Exit();
            }
        }
    }
}
