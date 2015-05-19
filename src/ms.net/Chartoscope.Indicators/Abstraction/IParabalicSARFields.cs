using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Indicators
{
    public interface IParabalicSARFields
    {        
        double CurrentSAR(int index = 0);
        double NextSAR(int index = 0);
        double EP(int index = 0);
        double DeltaEP_SAR(int index = 0);
        double AF(int index = 0);
        double Direction(int index = 0);
        double DeltaSAR(int index = 0);
    }
}
