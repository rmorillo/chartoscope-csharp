using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Droidworks.Common;

namespace Metadroids.Analytics.Builtin
{
    public class BollingerBands: IBuiltinMultiIndicator
    {
        internal BollingerBandsCore _bollingerBandsCore { get; set; }

        public BollingerBandsCore Core { get { return _bollingerBandsCore; } }

        public int BarCount { get; private set; }

        public BollingerBands(int barCount)
        {
            this.BarCount = barCount;
        }

        public double GetUpperBand(int index= 0)
        {            
            return _bollingerBandsCore.Last(index).Values[0];
        }

        public double GetMiddleBand(int index = 0)
        {
            return _bollingerBandsCore.Last(index).Values[1];
        }

        public double GetLowerBand(int index = 0)
        {
            return _bollingerBandsCore.Last(index).Values[1];
        }

        public BuiltinIndicatorOption BuiltinIndicator
        {
            get { return BuiltinIndicatorOption.BollingerBands; }
        }

        Droidworks.Common.BuiltinIndicatorTypeOption IBuiltinMultiIndicator.BuiltinIndicator
        {
            get { throw new NotImplementedException(); }
        }

        MultiIndicator IBuiltinMultiIndicator.Core
        {
            get { return this.Core; }
        }
    }
}
