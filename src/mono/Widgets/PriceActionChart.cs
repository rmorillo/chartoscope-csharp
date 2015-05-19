using System;
using System.Drawing;
using NPlot;
using Gtk.DotNet;
using Chartoscope.Common;
using Chartoscope.UI.Common;

namespace Chartoscope.Widgets
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PriceActionChart : Gtk.Bin, IPriceActionChart
	{
		protected NPlot.Bitmap.PlotSurface2D plotSurface;
        protected string infoText = "";
		
		public PriceActionChart ()
		{
			this.Build ();
			
			plotSurface= new NPlot.Bitmap.PlotSurface2D(700, 500);
			
			infoText = "";
            infoText += "Simple CandlePlot example. Demonstrates - \n";
            infoText += " * Setting candle plot datapoints using arrays \n";
            infoText += " * Plot Zoom interaction using MouseWheel ";

            plotSurface.Clear();

            FilledRegion fr = new FilledRegion(
                    new VerticalLine(1.2),
                    new VerticalLine(2.4));
            fr.Brush = Brushes.BlanchedAlmond;
            //plotSurface.Add(fr);

            // note that arrays can be of any type you like.
//            int[] opens = { 1, 2, 1, 2, 1, 3 };
//            double[] closes = { 2, 2, 2, 1, 2, 1 };
//            float[] lows =         { 0, 1, 1, 1, 0, 0 };
//            System.Int64[] highs =        { 3, 2, 3, 3, 3, 4 };
//            int[] times = { 0, 1, 2, 3, 4, 5 };
//
//            CandlePlot cp = new CandlePlot();
//            cp.CloseData = closes;
//            cp.OpenData = opens;
//            cp.LowData = lows;
//            cp.HighData = highs;
//            cp.AbscissaData = times;
//            plotSurface.Add(cp);

            HorizontalLine line = new HorizontalLine( 1.2 );
            line.LengthScale = 0.89f;
            //plotSurface.Add( line, -10 );

            VerticalLine line2 = new VerticalLine( 1.2 );
            line2.LengthScale = 0.89f;
            //plotSurface.Add( line2 );
            
            //plotSurface.AddInteraction (new PlotZoom());

            plotSurface.Title = "Line in the Title Number 1\nFollowed by another title line\n and another";
            plotSurface.Refresh();
		}

		protected void OnExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			System.Drawing.Graphics g = Gtk.DotNet.Graphics.FromDrawable (this.GdkWindow); 
	        g.DrawImage(System.Drawing.Image.FromHbitmap(plotSurface.Bitmap.GetHbitmap()), 0,0);
			//Pen p1 = new Pen (Color.Red);
	        //g.DrawRectangle (p1, 10, 10, 100, 100);
		}

		#region IPriceActionChart implementation
		public void NewPriceBar (DateTime timeStamp, double open, double high, double low, double close, double volume)
		{
			throw new NotImplementedException ();
		}

		public void LoadPriceBars (Bookmark<IPriceBar> priceBars)
		{
			int maxIndex= priceBars.Count;
			double[] opens= new double[maxIndex];
			double[] highs= new double[maxIndex];
			double[] lows= new double[maxIndex];
			double[] closes= new double[maxIndex];
			double[] times= new double[maxIndex];
			
			for(int i= 0; i<maxIndex; i++)
			{
				IPriceBar priceBar= priceBars[i];
				opens[i]= priceBar.Open;
				highs[i]= priceBar.High;
				lows[i]= priceBar.Low;
				closes[i]= priceBar.Close;
				times[i]= i;
			}
			CandlePlot cp = new CandlePlot();
			cp.OpenData= opens;
			cp.HighData= highs;
			cp.LowData= lows;
			cp.CloseData= closes;
			cp.AbscissaData= times;
			
			plotSurface.Clear();
			plotSurface.Add(cp);
			
			plotSurface.Refresh();
		}
		#endregion
	}
}

