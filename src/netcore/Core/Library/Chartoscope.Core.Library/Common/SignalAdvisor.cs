using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class SignalAdvisor
    {
        private const int _positionFullStrength= 100;

        public event Delegates.BuySignalEventHandler Buy;
        public event Delegates.SellSignalEventHandler Sell;
        public event Delegates.CloseSignalEventHandler Close;
        
        protected TickerReference _SignalTimeframe;
        

        private int _positionStrength;

        public int PositionStrength
        {
            get { return _positionStrength; }        
        }


        private PositionOption _position;
        public PositionOption Position
        {
            get
            {
                return _position;
            }
        }
        public SignalAdvisor(TickerReference tickerReference)
        {
            _SignalTimeframe = tickerReference;

            _position = PositionOption.None;
            _positionStrength = _positionFullStrength;
            
        }

        protected void OpenPosition(PositionOption position, int strength=100)
        {
            if (position==PositionOption.Buy)
            {
                if (Buy!=null)
                {
                    if (_position==PositionOption.Sell)
                    {

                        ClosePosition();
                    }

                    _position = PositionOption.Buy;
                    _positionStrength = strength;

                    Buy(strength);                    
                }
            }
            else if (position == PositionOption.Sell)
            {
                if (Sell!=null)
                {
                    if (_position == PositionOption.Buy)
                    {

                        ClosePosition();
                    }
                    _position = PositionOption.Sell;
                    _positionStrength = strength;

                    Sell(strength);                    
                }
            }
        }

        protected void ClosePosition()
        {            
            if (Close != null)
            {
                if (_position != PositionOption.None)
                {
                    _position = PositionOption.None;
                    _positionStrength = _positionFullStrength;

                    Close();
                }
            }
        }
    }
}
