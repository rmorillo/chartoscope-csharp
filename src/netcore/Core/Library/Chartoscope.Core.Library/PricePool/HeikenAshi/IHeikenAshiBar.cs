using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IHeikenAshiBar: ITimestamp
    { 
        double Open { get; }
        double High { get; }
        double Low { get; }
        double Close { get; }
        double Body { get; }
        double UpperWick { get; }
        double LowerWick { get; }
        bool IsFilled { get; }
    }
}
