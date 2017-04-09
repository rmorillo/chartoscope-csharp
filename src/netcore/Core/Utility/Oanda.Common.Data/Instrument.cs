using System;

namespace Oanda.Common.Data
{
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
}
