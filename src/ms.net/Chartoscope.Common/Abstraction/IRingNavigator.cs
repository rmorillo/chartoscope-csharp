using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public interface IRingNavigator<T>
    {
        T Last(int count = 0);       

        T LastItem { get; }
        bool HasValue(int count);
    }
}
