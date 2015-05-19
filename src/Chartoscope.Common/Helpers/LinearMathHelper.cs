using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chartoscope.Common
{
    public class LinearMathHelper
    {
        public static double GetSlope(params ChartPoint[] chartPoints)
        {
            double xAvg = 0;
            double yAvg = 0;

            foreach(ChartPoint chartPoint in chartPoints)
            {
                xAvg += chartPoint.X;

                yAvg += chartPoint.Y;
            }            

            xAvg = xAvg / chartPoints.Length;

            yAvg = yAvg / chartPoints.Length;

            double v1 = 0;
            double v2 = 0;

            foreach (ChartPoint chartPoint in chartPoints)
            {
                v1 += (chartPoint.X - xAvg) * (chartPoint.Y- yAvg);
                v2 += Math.Pow(chartPoint.X - xAvg, 2);
            } 

            double a = v1 / v2;

            double b = yAvg - a * xAvg;

            return a; 
        }
    }
}
