using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class RenkoBarEntity
    {
        public static readonly EntityField Timestamp;
        public static readonly EntityField Open;
        public static readonly EntityField Close;        

        public static readonly EntityHeader Header;
        static RenkoBarEntity()
        {
            Header = new EntityHeader("renko", "Renko", "Renko");

            Timestamp = new EntityField("timestamp", "Timestamp", "Timestamp", typeof(long), sizeof(long)).Register(Header);
            Open = new EntityField("open", "Open", "Open", typeof(double), sizeof(double)).Register(Header);          
        }
    }
}
