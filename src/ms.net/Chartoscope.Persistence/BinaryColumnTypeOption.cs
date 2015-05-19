using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Persistence
{
    public enum BinaryColumnTypeOption
    {
        [BinaryColumnTypeIdentity("Double", 8, null)]
        Double,
        [BinaryColumnTypeIdentity("Integer", 4, null)]
        Integer,
        [BinaryColumnTypeIdentity("Long", 8, null)]
        Long,
        [BinaryColumnTypeIdentity("String", "")]
        String,
        [BinaryColumnTypeIdentity("Guid", 16, null)]
        Guid
    }
}
