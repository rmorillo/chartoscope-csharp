using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Chartoscope.Common;
using System.Diagnostics;

namespace Chartoscope.Toolbox
{
    public partial class MultiChart : UserControl, IPriceActionChart, IOscillatorChart, IPercentageChart, IPipChart
    {
        private string priceActionChartId = PriceActionChart.Identifier;
        private PriceActionChart priceActionChart = null;

        private string oscillatorChartId = OscillatorChart.Identifier;
        private OscillatorChart oscillatorChart = null;

        private string percentageChartId = PercentageChart.Identifier;
        private PercentageChart percentageChart = null;

        private string pipChartId = PipChart.Identifier;
        private PipChart pipChart = null;

        private Dictionary<int, Dictionary<string, ElementPosition>> chartPositions= null;

        private int maxBars= 0;
        private int barsViewable = 0;
        private double lastPosition = 0;

        public BarItemType SelectedTimeFrame { get; set; }

        public event ChartingDelegates.RequestUpdateHandler RequestUpdate;

        public MultiChart()
        {
            InitializeComponent();

            //this.SelectedTimeFrame = BarItemType.M1;

            priceActionChart = new PriceActionChart();
            oscillatorChart = new OscillatorChart();
            percentageChart = new PercentageChart();
            pipChart = new PipChart();
            
            //panel1.Width = 50;
        }

        public void Initialize(int maxBars, int barsViewable)
        {
            this.maxBars = maxBars;
            this.barsViewable = barsViewable;

            //Candlesticks
            //1.  Series
            priceActionChart.Series = new Series(priceActionChartId);
            priceActionChart.SeriesCollection = chtChart.Series;
            priceActionChart.Series.ChartType = SeriesChartType.Candlestick;
            //candlestickChart.Series.IsVisibleInLegend = false;            
            priceActionChart.Series.IsValueShownAsLabel = false;
            priceActionChart.Series["PriceUpColor"] = "Green";
            priceActionChart.Series["PriceDownColor"] = "Red";
            //candlestickChart.Series["PixelPointWidth"] = "20";

            //2.  Chart area
            priceActionChart.ChartArea = new ChartArea(priceActionChartId);

            priceActionChart.ChartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            priceActionChart.ChartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            priceActionChart.ChartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            priceActionChart.ChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            priceActionChart.ChartArea.AxisX.IsReversed = true;
            priceActionChart.ChartArea.IsSameFontSizeForAllAxes = true;
            priceActionChart.ChartArea.AxisX.ScrollBar.Enabled = false;
            priceActionChart.ChartArea.AxisX.IsStartedFromZero = false;
            priceActionChart.ChartArea.AxisX.LabelStyle.TruncatedLabels = false;
            priceActionChart.ChartArea.AxisX.ScaleView.Zoomable = true;
            priceActionChart.ChartArea.AxisX.ScaleView.Zoom(0, barsViewable);
            priceActionChart.ChartArea.AxisX.ScaleView.Scroll((double)0);

            priceActionChart.Series.ChartArea = priceActionChartId;

            chtChart.ChartAreas.Add(priceActionChart.ChartArea);

            //Oscillator
            //1.  Series
            oscillatorChart.Series = new Series(oscillatorChartId);
            oscillatorChart.SeriesCollection = chtChart.Series;
            oscillatorChart.Series.ChartType = SeriesChartType.Column;
            oscillatorChart.Series.IsVisibleInLegend = false;
            
            //2. Chart area
            oscillatorChart.ChartArea = new ChartArea(oscillatorChartId);
            
            chtChart.Series.Add(oscillatorChart.Series);

            oscillatorChart.ChartArea.AlignWithChartArea = priceActionChartId;
            oscillatorChart.ChartArea.AxisX.IsReversed = true;
            oscillatorChart.ChartArea.AxisX.ScrollBar.Enabled = false;
            oscillatorChart.ChartArea.AxisX.IsStartedFromZero = false;
            oscillatorChart.ChartArea.AxisX.LabelStyle.TruncatedLabels = false;
            oscillatorChart.ChartArea.AxisX.ScaleView.Zoomable = true;
            oscillatorChart.ChartArea.AxisX.ScaleView.SizeType = DateTimeIntervalType.Auto;
            oscillatorChart.ChartArea.Position = new ElementPosition(0, 75, 100, 25);
            oscillatorChart.ChartArea.AxisX.ScaleView.Zoom(0, barsViewable);
            oscillatorChart.ChartArea.AxisX.ScaleView.Scroll((double) 0);

            //Percentage
            //1.  Series
            percentageChart.Series = new Series(percentageChartId);
            percentageChart.SeriesCollection = chtChart.Series;
            percentageChart.Series.ChartType = SeriesChartType.Column;
            percentageChart.Series.IsVisibleInLegend = false;

            //2. Chart area
            percentageChart.ChartArea = new ChartArea(percentageChartId);

            chtChart.Series.Add(percentageChart.Series);

            percentageChart.ChartArea.AlignWithChartArea = priceActionChartId;
            percentageChart.ChartArea.AxisX.IsReversed = true;
            percentageChart.ChartArea.AxisX.ScrollBar.Enabled = false;
            percentageChart.ChartArea.AxisX.IsStartedFromZero = false;
            percentageChart.ChartArea.AxisX.LabelStyle.TruncatedLabels = false;
            percentageChart.ChartArea.AxisX.ScaleView.Zoomable = true;
            percentageChart.ChartArea.AxisX.ScaleView.SizeType = DateTimeIntervalType.Auto;
            percentageChart.ChartArea.Position = new ElementPosition(0, 75, 100, 25);
            percentageChart.ChartArea.AxisX.ScaleView.Zoom(0, barsViewable);
            percentageChart.ChartArea.AxisX.ScaleView.Scroll((double)0);

            //Percentage
            //1.  Series
            pipChart.Series = new Series(pipChartId);
            pipChart.SeriesCollection = chtChart.Series;
            pipChart.Series.ChartType = SeriesChartType.Column;
            pipChart.Series.IsVisibleInLegend = false;

            //2. Chart area
            pipChart.ChartArea = new ChartArea(pipChartId);

            chtChart.Series.Add(pipChart.Series);

            pipChart.ChartArea.AlignWithChartArea = priceActionChartId;
            pipChart.ChartArea.AxisX.IsReversed = true;
            pipChart.ChartArea.AxisX.ScrollBar.Enabled = false;
            pipChart.ChartArea.AxisX.IsStartedFromZero = false;
            pipChart.ChartArea.AxisX.LabelStyle.TruncatedLabels = false;
            pipChart.ChartArea.AxisX.ScaleView.Zoomable = true;
            pipChart.ChartArea.AxisX.ScaleView.SizeType = DateTimeIntervalType.Auto;
            pipChart.ChartArea.Position = new ElementPosition(0, 75, 100, 25);
            pipChart.ChartArea.AxisX.ScaleView.Zoom(0, barsViewable);
            pipChart.ChartArea.AxisX.ScaleView.Scroll((double)0);

            //Chart positions
            chartPositions = new Dictionary<int, Dictionary<string, ElementPosition>>();
            //1. Single Charts
            Dictionary<string, ElementPosition> singleCharts = new Dictionary<string, ElementPosition>();
            singleCharts.Add(priceActionChartId, new ElementPosition(0,0,90,95));
            singleCharts.Add(oscillatorChartId, new ElementPosition(0, 0, 90, 95));
            singleCharts.Add(percentageChartId, new ElementPosition(0, 0, 90, 95));
            singleCharts.Add(pipChartId, new ElementPosition(0, 0, 90, 95));
            chartPositions.Add(1, singleCharts);
            //2. Double Charts
            Dictionary<string, ElementPosition> doubleCharts = new Dictionary<string, ElementPosition>();
            doubleCharts.Add(priceActionChartId, new ElementPosition(0, 0, 100, 75));
            doubleCharts.Add(oscillatorChartId, new ElementPosition(0, 75, 100, 25));
            doubleCharts.Add(percentageChartId, new ElementPosition(0, 75, 100, 25));
            doubleCharts.Add(pipChartId, new ElementPosition(0, 75, 100, 25));
            chartPositions.Add(2, doubleCharts);
            //3. Triple Charts
            Dictionary<string, ElementPosition> tripleCharts = new Dictionary<string, ElementPosition>();
            tripleCharts.Add(priceActionChartId, new ElementPosition(0, 0, 100, 40));
            tripleCharts.Add(oscillatorChartId, new ElementPosition(0, 40, 100, 20));
            tripleCharts.Add(percentageChartId, new ElementPosition(0, 60, 100, 20));
            tripleCharts.Add(pipChartId, new ElementPosition(0, 80, 100, 20));
            chartPositions.Add(3, tripleCharts);
            //4. Quad Charts
            Dictionary<string, ElementPosition> quadCharts = new Dictionary<string, ElementPosition>();
            quadCharts.Add(priceActionChartId, new ElementPosition(0, 0, 100, 40));
            quadCharts.Add(oscillatorChartId, new ElementPosition(0, 40, 100, 20));
            quadCharts.Add(percentageChartId, new ElementPosition(0, 60, 100, 20));
            quadCharts.Add(pipChartId, new ElementPosition(0, 80, 100, 20));
            chartPositions.Add(4, tripleCharts);
        }

        public void Clear()
        {
            foreach (Series series in chtChart.Series)
            {
                series.Points.Clear();
            }
            chtChart.Annotations.Clear();
            chtChart.Images.Clear();
            priceActionChart.IndexKeyForTime.Clear();
            priceActionChart.TimeKeyForIndex.Clear();
        }

        private void RefreshPositions()
        {
            Dictionary<string, ElementPosition> positions = chartPositions[chtChart.ChartAreas.Count];
            foreach (KeyValuePair<string, ElementPosition> position in positions)
            {
                if (chtChart.ChartAreas.FindByName(position.Key)!=null)
                {
                    chtChart.ChartAreas[position.Key].Position = position.Value;
                }
            }
        }

        bool IPriceActionChart.Visible { get { return chtChart.ChartAreas.FindByName(priceActionChartId) != null; } }

        void IPriceActionChart.Show()
        {
            if (chtChart.ChartAreas.FindByName(priceActionChartId)==null)
            {
                chtChart.ChartAreas.Add(priceActionChart.ChartArea);
            }

            if (chtChart.Series.FindByName(priceActionChartId) == null)
            {
                chtChart.Series.Add(priceActionChart.Series);
            }

            RefreshPositions();
        }

        void IPriceActionChart.Hide()
        {
            if (chtChart.ChartAreas.FindByName(priceActionChartId) == null)
            {
                chtChart.ChartAreas.Remove(priceActionChart.ChartArea);
            }

            RefreshPositions();
        }

        bool IOscillatorChart.Visible { get { return chtChart.ChartAreas.FindByName(oscillatorChartId) != null; } }

        void IOscillatorChart.Show()
        {
            if (chtChart.ChartAreas.FindByName(oscillatorChartId) == null)
            {
                chtChart.ChartAreas.Add(oscillatorChart.ChartArea);
            }

            if (chtChart.Series.FindByName(oscillatorChartId) == null)
            {
                chtChart.Series.Add(oscillatorChart.Series);
            }

            oscillatorChart.Series.ChartArea = oscillatorChartId;

            RefreshPositions();
        }

        void IOscillatorChart.Hide()
        {
            if (chtChart.ChartAreas.FindByName(oscillatorChartId) == null)
            {
                chtChart.ChartAreas.Remove(oscillatorChart.ChartArea);
            }

            RefreshPositions();
        }       

        void IPriceActionChart.SetDataPoints(BarItem[] barItems)
        {
            priceActionChart.ResetDataPointBounds();
            priceActionChart.IndexKeyForTime.Clear();
            priceActionChart.TimeKeyForIndex.Clear();
            priceActionChart.Series.Points.Clear();
            priceActionChart.ChartArea.AxisX.CustomLabels.Clear();

            for (int indicatorIndex = barItems.Length - 1; indicatorIndex >= 0; indicatorIndex--)
            {
                BarItem barItem = barItems[indicatorIndex];

                int chartIndex= priceActionChart.Series.Points.AddY(barItem.High, barItem.Low, barItem.Open, barItem.Close);

                if (barItem.High > priceActionChart.DataPointMax)
                    priceActionChart.DataPointMax = barItem.High;
                if (barItem.Low < priceActionChart.DataPointMin)
                    priceActionChart.DataPointMin = barItem.Low;

                priceActionChart.IndexKeyForTime.Add(chartIndex, barItem.Time);

                if (!priceActionChart.TimeKeyForIndex.ContainsKey(barItem.Time))
                {
                    priceActionChart.TimeKeyForIndex.Add(barItem.Time, chartIndex);
                }

                priceActionChart.ChartArea.AxisX.CustomLabels.Add(GetPrimaryLabel(this.SelectedTimeFrame, barItem, chartIndex));

                CustomLabel dateLabel = GetSecondaryLabel(this.SelectedTimeFrame, barItem, chartIndex);
                if (dateLabel != null)
                {
                    priceActionChart.ChartArea.AxisX.CustomLabels.Add(dateLabel);
                }
            }

            priceActionChart.ChartArea.AxisY.Minimum = priceActionChart.DataPointMin - ((priceActionChart.DataPointMax - priceActionChart.DataPointMin) * 0.2);
            priceActionChart.ChartArea.AxisY.Maximum = priceActionChart.DataPointMax + ((priceActionChart.DataPointMax - priceActionChart.DataPointMin) * 0.2);

            chtChart.ChartAreas[0].AxisX.ScaleView.Position = 1;
        }

        private void chtChart_MouseMove(object sender, MouseEventArgs e)
        {
            double currentScrollPosition= chtChart.ChartAreas[0].AxisX.ScaleView.Position;
            double newScrollPosition = 0;
            double currentPosition = 0;
            if (chtChart.Series.Count>0 && e.Button == System.Windows.Forms.MouseButtons.Left && e.X > 0 && e.X < chtChart.Width)
            {
                currentPosition = chtChart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);                
                double displacement = this.lastPosition - currentPosition;
                newScrollPosition= chtChart.ChartAreas[0].AxisX.ScaleView.Position + displacement;
                chtChart.ChartAreas[0].AxisX.ScaleView.Scroll(newScrollPosition);
                currentPosition = chtChart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
            }
            
            if (chtChart.Series.Count > 0 && e.Button == System.Windows.Forms.MouseButtons.Left && (chtChart.ChartAreas[0].AxisX.ScaleView.Position >= maxBars || chtChart.ChartAreas[0].AxisX.ScaleView.Position <= 1))
            {
                ChartScrollDirectionMode scrollDirection = chtChart.ChartAreas[0].AxisX.ScaleView.Position >= maxBars ? ChartScrollDirectionMode.LeftToRight : ChartScrollDirectionMode.RightToLeft;

                if (RequestUpdate != null && RequestUpdate(lastDateTimePosition, priceActionChart.IndexKeyForTime[priceActionChart.IndexKeyForTime.Count - 1], priceActionChart.IndexKeyForTime[0], scrollDirection))
                {
                    int index = priceActionChart.TimeKeyForIndex[lastDateTimePosition];

                    newScrollPosition = index - chtChart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);                

                    chtChart.ChartAreas[0].AxisX.ScaleView.Scroll(newScrollPosition);

                    RefreshLastPosition(e.X);
                }
            } 
        }

        private DateTime lastDateTimePosition = DateTime.MinValue;
        private void chtChart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && chtChart.Series.Count>0 && priceActionChart.IndexKeyForTime.Count>0)
            {
                RefreshLastPosition(e.X);
            }
        }

        private void RefreshLastPosition(int xValue)
        {
            this.lastPosition = chtChart.ChartAreas[0].AxisX.PixelPositionToValue(xValue);
            if (priceActionChart.IndexKeyForTime.ContainsKey((int)this.lastPosition))
            {
                lastDateTimePosition = priceActionChart.IndexKeyForTime[(int)this.lastPosition];
            }
        }

        private CustomLabel GetPrimaryLabel(BarItemType barType, BarItem barItem, double chartIndex)
        {
            CustomLabel primaryLabel = new CustomLabel();

            primaryLabel.FromPosition = chartIndex;
            primaryLabel.ToPosition = chartIndex + 1;
            primaryLabel.RowIndex = 0;
            if (barType.Mode == BarItemMode.Time)
            {
                if (barType.Tag == "M" || (barType.Tag == "H" && barType.Value < 4))
                {
                    primaryLabel.Text = barItem.Time.ToString("HH:mm");
                }
                else if (barType.Tag == "H" && barType.Value >= 4)
                {
                    primaryLabel.Text = barItem.Time.ToString(@"ddd, HH\h");
                }
                else if (barType.Tag == "D")
                {
                    primaryLabel.Text = barItem.Time.ToString(@"ddd, dd");
                }
                else if (barType.Tag == "W")
                {
                    string weekPeriod = barItem.Time.ToString("dd") + "-" + barItem.Time.AddDays(6).ToString("dd, MMM");
                    primaryLabel.Text = weekPeriod;
                }
                else if (barType.Tag == "MN")
                {
                    primaryLabel.Text = barItem.Time.ToString(@"MMMM");
                }
            }
            else
            {
                primaryLabel.Text = barItem.Time.ToString("HH:mm");
            }

            return primaryLabel;
        }

        private CustomLabel GetSecondaryLabel(BarItemType barType, BarItem barItem, double chartIndex)
        {
            CustomLabel secondaryLabel = null;

            if (barType.Mode == BarItemMode.Time)
            {
                if (barType.Tag == "M" || (barType.Tag == "H" && barType.Value < 4))
                {
                    if (chartIndex % 10 == 0)
                    {
                        secondaryLabel = new CustomLabel();
                        secondaryLabel.FromPosition = chartIndex - 5;
                        secondaryLabel.ToPosition = chartIndex + 5;
                        secondaryLabel.RowIndex = 3;
                        secondaryLabel.ForeColor = Color.DarkBlue;

                        secondaryLabel.Text = barItem.Time.ToString("ddd, dd-MMM-yyyy");
                    }
                }
                else if (barType.Tag == "H" && barType.Value >= 4)
                {
                    if (chartIndex % 8 == 0)
                    {
                        secondaryLabel = new CustomLabel();
                        secondaryLabel.FromPosition = chartIndex - 5;
                        secondaryLabel.ToPosition = chartIndex + 5;
                        secondaryLabel.RowIndex = 3;
                        secondaryLabel.ForeColor = Color.DarkBlue;
                        secondaryLabel.Text = barItem.Time.ToString("dd-MMM-yyyy");
                    }
                }
                else if (barType.Tag == "D" || barType.Tag == "W")
                {
                    if (chartIndex % 12 == 0)
                    {
                        secondaryLabel = new CustomLabel();
                        secondaryLabel.FromPosition = chartIndex - 5;
                        secondaryLabel.ToPosition = chartIndex + 5;
                        secondaryLabel.RowIndex = 3;
                        secondaryLabel.ForeColor = Color.DarkBlue;
                        secondaryLabel.Text = barItem.Time.ToString("yyyy");
                    }
                }
                else if (barType.Tag == "MN")
                {
                    if (chartIndex % 15 == 0)
                    {
                        secondaryLabel = new CustomLabel();
                        secondaryLabel.FromPosition = chartIndex - 5;
                        secondaryLabel.ToPosition = chartIndex + 5;
                        secondaryLabel.RowIndex = 3;
                        secondaryLabel.ForeColor = Color.DarkBlue;
                        secondaryLabel.Text = barItem.Time.ToString("MMMM, yyyy");
                    }
                }
            }
            else
            {
                if (chartIndex % 10 == 0)
                {
                    secondaryLabel = new CustomLabel();
                    secondaryLabel.FromPosition = chartIndex - 5;
                    secondaryLabel.ToPosition = chartIndex + 5;
                    secondaryLabel.RowIndex = 3;
                    secondaryLabel.ForeColor = Color.DarkBlue;

                    secondaryLabel.Text = barItem.Time.ToString("ddd, dd-MMM-yyyy");
                }
            }

            return secondaryLabel;
        }       

        public void ScrollToCenter(DateTime time)
        {
            priceActionChart.ChartArea.AxisX.ScaleView.Position = priceActionChart.TimeKeyForIndex[time] - 14;
        }


        void IPriceActionChart.PlotIndicator(string indicatorName, ChartIndicatorItem[] indicatorItems, ChartTypeOption chartType)
        {
            priceActionChart.AddIndicatorSeries(indicatorName, indicatorItems, chartType, priceActionChartId);
        }


        void IOscillatorChart.PlotIndicator(string indicatorName, ChartIndicatorItem[] indicatorItems, ChartTypeOption chartType)
        {
            oscillatorChart.AddIndicatorSeries(indicatorName, indicatorItems, chartType, oscillatorChartId);
        }

        void IPriceActionChart.SelectIndicator(string indicatorName)
        {
            if (priceActionChart.SeriesCollection.FindByName(indicatorName) != null)
            {
                priceActionChart.SeriesCollection[indicatorName].BorderWidth = 3;
            }
        }

        void IPriceActionChart.UnselectIndicator(string indicatorName)
        {
            if (priceActionChart.SeriesCollection.FindByName(indicatorName) != null)
            {
                priceActionChart.SeriesCollection[indicatorName].BorderWidth = 1;
            }
        }


        void IPriceActionChart.UnplotIndicator(string indicatorName)
        {
            Series indicatorSeries= priceActionChart.SeriesCollection.FindByName(indicatorName);
            if (indicatorSeries != null)
            {
                priceActionChart.SeriesCollection.Remove(indicatorSeries);
            }
        }


        void IOscillatorChart.UnplotIndicator(string indicatorName)
        {
            Series indicatorSeries = oscillatorChart.SeriesCollection.FindByName(indicatorName);
            if (indicatorSeries != null)
            {
                oscillatorChart.SeriesCollection.Remove(indicatorSeries);
            }
        }


        void IPriceActionChart.ChangeChartType(ChartTypeOption chartType)
        {
            priceActionChart.ChangeChartType(chartType);
        }

        private void MultiChart_Resize(object sender, EventArgs e)
        {
            //tpnCurrentDateTime.Height = this.Height;
            chtChart.Invalidate();
            chtChart.Update();
        }

        private void chtChart_PostPaint(object sender, ChartPaintEventArgs e)
        {

        }

        private void MultiChart_Paint(object sender, PaintEventArgs e)
        {
            //tpnCurrentDateTime.Invalidate();
        }

        bool IPercentageChart.Visible { get { return chtChart.ChartAreas.FindByName(percentageChartId) != null; } }

        void IPercentageChart.Show()
        {
            if (chtChart.ChartAreas.FindByName(percentageChartId) == null)
            {
                chtChart.ChartAreas.Add(percentageChart.ChartArea);
            }

            if (chtChart.Series.FindByName(percentageChartId) == null)
            {
                chtChart.Series.Add(percentageChart.Series);
            }

            percentageChart.Series.ChartArea = percentageChartId;

            RefreshPositions();
        }

        void IPercentageChart.Hide()
        {
            if (chtChart.ChartAreas.FindByName(percentageChartId) == null)
            {
                chtChart.ChartAreas.Remove(percentageChart.ChartArea);
            }

            RefreshPositions();
        }

        void IPercentageChart.PlotIndicator(string indicatorName, ChartIndicatorItem[] indicatorItems, ChartTypeOption chartType)
        {
            percentageChart.AddIndicatorSeries(indicatorName, indicatorItems, chartType, percentageChartId);
        }

        void IPercentageChart.UnplotIndicator(string indicatorName)
        {
            Series indicatorSeries = percentageChart.SeriesCollection.FindByName(indicatorName);
            if (indicatorSeries != null)
            {
                percentageChart.SeriesCollection.Remove(indicatorSeries);
            }
        }

        bool IPipChart.Visible { get { return chtChart.ChartAreas.FindByName(pipChartId) != null; } }

        void IPipChart.Show()
        {
            if (chtChart.ChartAreas.FindByName(pipChartId) == null)
            {
                chtChart.ChartAreas.Add(pipChart.ChartArea);
            }

            if (chtChart.Series.FindByName(pipChartId) == null)
            {
                chtChart.Series.Add(pipChart.Series);
            }

            pipChart.Series.ChartArea = pipChartId;

            RefreshPositions();
        }

        void IPipChart.Hide()
        {
            if (chtChart.ChartAreas.FindByName(pipChartId) == null)
            {
                chtChart.ChartAreas.Remove(pipChart.ChartArea);
            }

            RefreshPositions();
        }

        void IPipChart.PlotIndicator(string indicatorName, ChartIndicatorItem[] indicatorItems, ChartTypeOption chartType)
        {
            pipChart.AddIndicatorSeries(indicatorName, indicatorItems, chartType, pipChartId);
        }

        void IPipChart.UnplotIndicator(string indicatorName)
        {
            Series indicatorSeries = pipChart.SeriesCollection.FindByName(indicatorName);
            if (indicatorSeries != null)
            {
                pipChart.SeriesCollection.Remove(indicatorSeries);
            }
        }

        private List<int> annotationOverlaps = new List<int>();

        public void PlotSignal(ChartSignalItem[] chartSignalItems)
        {
            chtChart.Annotations.Clear();
            chtChart.Images.Clear();
            chtChart.Images.Add(new NamedImage("buy", Properties.Resources.buysignal));
            chtChart.Images.Add(new NamedImage("sell", Properties.Resources.sellsignal));
            chtChart.Images.Add(new NamedImage("no_signal", Properties.Resources.indecisionsignal));

            Bitmap reversalToBuyBitmap = new Bitmap(Math.Max(Properties.Resources.buysignal.Size.Width, Properties.Resources.indecisionsignal.Size.Width), Properties.Resources.indecisionsignal.Height + Properties.Resources.buysignal.Height);
            Graphics gfxReversalToBuy = Graphics.FromImage(reversalToBuyBitmap);
            gfxReversalToBuy.DrawImageUnscaled(Properties.Resources.indecisionsignal, 0, 0);
            gfxReversalToBuy.DrawImageUnscaled(Properties.Resources.buysignal, 0, Properties.Resources.indecisionsignal.Height);
            gfxReversalToBuy.Dispose();        
            chtChart.Images.Add(new NamedImage("reversalToBuy", reversalToBuyBitmap));

            Bitmap reversalToSellBitmap = new Bitmap(Math.Max(Properties.Resources.sellsignal.Size.Width, Properties.Resources.indecisionsignal.Size.Width), Properties.Resources.indecisionsignal.Height + Properties.Resources.sellsignal.Height);
            Graphics gfxReversalToSell = Graphics.FromImage(reversalToSellBitmap);
            gfxReversalToSell.DrawImageUnscaled(Properties.Resources.indecisionsignal, 0, 0);
            gfxReversalToSell.DrawImageUnscaled(Properties.Resources.sellsignal, 0, Properties.Resources.indecisionsignal.Height);
            gfxReversalToSell.Dispose();
            chtChart.Images.Add(new NamedImage("reversalToSell", reversalToSellBitmap));
                   
            DateTime lastSignalTime = DateTime.MinValue ;
            SignalState lastSignal= SignalState.NoSignal;
            foreach (ChartSignalItem chartSignalItem in chartSignalItems)
            {
                if (priceActionChart.TimeKeyForIndex.ContainsKey(chartSignalItem.Time))
                {
                    ImageAnnotation logo = new ImageAnnotation();
                    switch (chartSignalItem.SignalState)
                    {
                        case SignalState.Long:
                            logo.Image = "buy";
                            break;
                        case SignalState.Short:
                            logo.Image = "sell";
                            break;
                        case SignalState.NoSignal:
                            logo.Image = "no_signal";
                            break;
                    }
                    logo.ShadowOffset = 5;
                    logo.ShadowColor = Color.Black;
                    logo.AnchorOffsetY = 2;
                    DataPoint dataPoint = priceActionChart.Series.Points[priceActionChart.TimeKeyForIndex[chartSignalItem.Time]];
                    //dataPoint.BorderColor = Color.Black;
                    //dataPoint.LabelForeColor = Color.Black;
                    //dataPoint.LabelBackColor = Color.Yellow;
                    //dataPoint.Label= eventListItem.EventName;;
                    //dataPoint.Font = new Font("Arial", 8);

                    if (chartSignalItem.Time == lastSignalTime)
                    {
                        chtChart.Annotations.RemoveAt(chtChart.Annotations.Count - 1);
                        logo.AnchorDataPoint = dataPoint;
                        logo.Alignment = ContentAlignment.TopCenter;
                        if (lastSignal == SignalState.NoSignal && chartSignalItem.SignalState == SignalState.Long)
                        {
                            logo.Image = "reversalToBuy";
                        }
                        else if (lastSignal == SignalState.NoSignal && chartSignalItem.SignalState == SignalState.Short)
                        {
                            logo.Image = "reversalToSell";
                        }

                        chtChart.Annotations.Add(logo);
                    }
                    else
                    {
                        lastSignalTime = chartSignalItem.Time;

                        logo.AnchorDataPoint = dataPoint;
                        logo.Alignment = ContentAlignment.TopCenter;
                        chtChart.Annotations.Add(logo);
                    }                 
                    
                }
            }

            if (this.annotationOverlaps.Count > 0)
            {
                chtChart.ChartAreas[0].RecalculateAxesScale();
                double imageHeightToYValue = chtChart.ChartAreas[0].AxisY.PixelPositionToValue(0) - chtChart.ChartAreas[0].AxisY.PixelPositionToValue(Properties.Resources.buysignal.Height);
                foreach (int index in this.annotationOverlaps)
                {
                    chtChart.Annotations[index].AnchorY = chtChart.Annotations[index].AnchorDataPoint.YValues[2] + imageHeightToYValue;
                }
                this.annotationOverlaps.Clear();
            }
        }

        private void chtChart_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MultiChart_Load(object sender, EventArgs e)
        {
            
        }

        private void chtChart_PrePaint(object sender, ChartPaintEventArgs e)
        {

        }


        public void PlotStrategy(ChartStrategyItem[] chartStrategyItems)
        {
            throw new NotImplementedException();
        }
    }
}
