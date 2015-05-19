using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Caching
{
    public class IndicatorDataFrame
    {        
        public IndicatorDataFrame(string identityCode, Dictionary<string, IndicatorChartingInfo> charting)
        {
            this.identityCode = identityCode;
            this.charting = charting;
        }

        private string identityCode;

        public string IdentityCode
        {
            get { return identityCode; }
        }


        private Dictionary<string, IndicatorChartingInfo> charting;

        public Dictionary<string, IndicatorChartingInfo> Charting
        {
            get { return charting; }
        }

        private Dictionary<DateTime, double[]> values= new Dictionary<DateTime,double[]>();

        public Dictionary<DateTime, double[]> Values
        {
            get { return values; }
        }

        protected void AddValue(DateTime time, double value)
        {
            this.values.Add(time, new double[] { value });
        }
        
        protected void AddValue(DateTime time, double[] values)
        {
            this.values.Add(time, values);
        }

    }
}
