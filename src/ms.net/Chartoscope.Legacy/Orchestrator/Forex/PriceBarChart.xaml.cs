using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chartoscope.Orchestrator.Forex
{
    /// <summary>
    /// Interaction logic for PriceBarChart.xaml
    /// </summary>
    public partial class PriceBarChart : UserControl
    {
        public PriceBarChart()
        {
            InitializeComponent();

            var value = new Dictionary<int, double>();
            for (int i = 0; i < 10; i++)
                value.Add(i, 10 * i);
            
            Chart chart = this.FindName("MyWinformChart") as Chart;
            
            chart.DataSource = value;
            chart.Series["series"].XValueMember = "Key";
            chart.Series["series"].YValueMembers = "Value";            
            chart.ChartAreas[0].BackColor= System.Drawing.Color.Gray;
            chart.ChartAreas[0].BackGradientStyle = GradientStyle.LeftRight;
            chart.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Regular);
            chart.ChartAreas[0].AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            chart.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Regular);

        }
    }
}
