using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chartoscope.Core.Library.UnitTest.Mocks
{
    public class MockProbe : IProbe<IOHLCBar>
    {
        public MockProbe(int id)
        {
            _id = id;
            _codeName = id.ToString();
            _fullName = id.ToString();
        }

        private string _fullName;
        public string FullName
        {
            get
            {
                return _fullName;
            }
        }

        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
        }

        private string _codeName;
        public string CodeName
        {
            get
            {
                return _codeName;
            }
        }

        public void PriceAction(IOHLCBar priceBar)
        {
            
        }
    }
}
