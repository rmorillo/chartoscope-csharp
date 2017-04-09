using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public interface IProbe<T>
    {
        int Id { get; }

        string FullName { get; }

        string CodeName { get; }

        void PriceAction(T priceBar);
    }
}
