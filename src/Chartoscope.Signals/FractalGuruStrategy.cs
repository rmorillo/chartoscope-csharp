using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    //TODO:  Convert the following code into fractal indicator

    //while(i>=2)
    // {
    //  //----Fractals up
    //  bFound=false;
    //  dCurrent=High[i];
    //  if(dCurrent>High[i+1] && dCurrent>High[i+2] && dCurrent>High[i-1] && dCurrent>High[i-2])
    //    {
    //     bFound=true;
    //     ExtUpFractalsBuffer[i]=dCurrent;
    //    }
    //  //----6 bars Fractal
    //  if(!bFound && (Bars-i-1)>=3)
    //    {
    //     if(dCurrent==High[i+1] && dCurrent>High[i+2] && dCurrent>High[i+3] &&
    //        dCurrent>High[i-1] && dCurrent>High[i-2])
    //       {
    //        bFound=true;
    //        ExtUpFractalsBuffer[i]=dCurrent;
    //       }
    //    }         
    //  //----7 bars Fractal
    //  if(!bFound && (Bars-i-1)>=4)
    //    {   
    //     if(dCurrent>=High[i+1] && dCurrent==High[i+2] && dCurrent>High[i+3] && dCurrent>High[i+4] &&
    //        dCurrent>High[i-1] && dCurrent>High[i-2])
    //       {
    //        bFound=true;
    //        ExtUpFractalsBuffer[i]=dCurrent;
    //       }
    //    }  
    //  //----8 bars Fractal                          
    //  if(!bFound && (Bars-i-1)>=5)
    //    {   
    //     if(dCurrent>=High[i+1] && dCurrent==High[i+2] && dCurrent==High[i+3] && dCurrent>High[i+4] && dCurrent>High[i+5] && 
    //        dCurrent>High[i-1] && dCurrent>High[i-2])
    //       {
    //        bFound=true;
    //        ExtUpFractalsBuffer[i]=dCurrent;
    //       }
    //    } 
    //  //----9 bars Fractal                                        
    //  if(!bFound && (Bars-i-1)>=6)
    //    {   
    //     if(dCurrent>=High[i+1] && dCurrent==High[i+2] && dCurrent>=High[i+3] && dCurrent==High[i+4] && dCurrent>High[i+5] && 
    //        dCurrent>High[i+6] && dCurrent>High[i-1] && dCurrent>High[i-2])
    //       {
    //        bFound=true;
    //        ExtUpFractalsBuffer[i]=dCurrent;
    //       }
    //    }                                    
    //  //----Fractals down
    //  bFound=false;
    //  dCurrent=Low[i];
    //  if(dCurrent<Low[i+1] && dCurrent<Low[i+2] && dCurrent<Low[i-1] && dCurrent<Low[i-2])
    //    {
    //     bFound=true;
    //     ExtDownFractalsBuffer[i]=dCurrent;
    //    }
    //  //----6 bars Fractal
    //  if(!bFound && (Bars-i-1)>=3)
    //    {
    //     if(dCurrent==Low[i+1] && dCurrent<Low[i+2] && dCurrent<Low[i+3] &&
    //        dCurrent<Low[i-1] && dCurrent<Low[i-2])
    //       {
    //        bFound=true;
    //        ExtDownFractalsBuffer[i]=dCurrent;
    //       }                      
    //    }         
    //  //----7 bars Fractal
    //  if(!bFound && (Bars-i-1)>=4)
    //    {   
    //     if(dCurrent<=Low[i+1] && dCurrent==Low[i+2] && dCurrent<Low[i+3] && dCurrent<Low[i+4] &&
    //        dCurrent<Low[i-1] && dCurrent<Low[i-2])
    //       {
    //        bFound=true;
    //        ExtDownFractalsBuffer[i]=dCurrent;
    //       }                      
    //    }  
    //  //----8 bars Fractal                          
    //  if(!bFound && (Bars-i-1)>=5)
    //    {   
    //     if(dCurrent<=Low[i+1] && dCurrent==Low[i+2] && dCurrent==Low[i+3] && dCurrent<Low[i+4] && dCurrent<Low[i+5] && 
    //        dCurrent<Low[i-1] && dCurrent<Low[i-2])
    //       {
    //        bFound=true;
    //        ExtDownFractalsBuffer[i]=dCurrent;
    //       }                      
    //    } 
    //  //----9 bars Fractal                                        
    //  if(!bFound && (Bars-i-1)>=6)
    //    {   
    //     if(dCurrent<=Low[i+1] && dCurrent==Low[i+2] && dCurrent<=Low[i+3] && dCurrent==Low[i+4] && dCurrent<Low[i+5] && 
    //        dCurrent<Low[i+6] && dCurrent<Low[i-1] && dCurrent<Low[i-2])
    //       {
    //        bFound=true;
    //        ExtDownFractalsBuffer[i]=dCurrent;
    //       }                      
    //    }                                    
    //  i--;
    // }

    //http://forex-strategies-revealed.com/basic/fractal-guru-strategy
    //Forex trading strategy #13 (The Fractal Guru Strategy)
    public class FractalGuruStrategy: SignalBase
    {
        public const string IDENTITY_CODE = "fractal_guru_strategy";
        private SMA sma = null;

        public FractalGuruStrategy(BarItemType barType)
        {
            this.barType = barType;

            this.identityCode = string.Format("{0}({1})", IDENTITY_CODE, barType.Code);

            sma = new SMA(barType, 30);

            Register(sma);
        }
        
        private bool IsLongSetup(PriceBars priceBar)
        {           
            return sma.Value() < priceBar.LastItem.High && sma.Value() > priceBar.LastItem.Low && priceBar.LastItem.Close > priceBar.LastItem.Open;
        }

        private bool IsShortSetup(PriceBars priceBar)
        {
            return sma.Value() < priceBar.LastItem.High && sma.Value() > priceBar.LastItem.Low && priceBar.LastItem.Close < priceBar.LastItem.Open;
        }

        protected override void OutPosition(PriceBars priceBar, BarItemType barType)
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

        protected override void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
            if ((position == PositionMode.Long && IsShortSetup(priceBar))
                || (position == PositionMode.Short && IsLongSetup(priceBar)))
            {
                Exit();
            }
        }
    }
}
