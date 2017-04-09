using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core
{
    public class TickerSymbol
    {
        public TickerSymbol(string codeName)
        {
            _codeName = codeName;
            Id = Guid.NewGuid();
        }

        private string _codeName;
        public string CodeName { get { return _codeName; } }

        public Guid Id { get; private set; }
    }
}
