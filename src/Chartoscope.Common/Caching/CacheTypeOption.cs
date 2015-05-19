using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public enum CacheTypeOption
    {
        [CacheFileExtension("bar")]
        Pricebar,
        [CacheFileExtension("ind")]
        Indicator,
        [CacheFileExtension("alt")]
        Alert,
        [CacheFileExtension("sig")]
        Signal,
        [CacheFileExtension("str")]
        Strategy,
        [CacheFileExtension("ord")]
        Orders,
        [CacheFileExtension("cfg")]
        Configuration
    }
}
