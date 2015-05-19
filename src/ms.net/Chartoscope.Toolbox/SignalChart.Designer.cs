namespace Chartoscope.Toolbox
{
    partial class SignalChart
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.chtEquityCurve = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lvwOrders = new System.Windows.Forms.ListView();
            this.hdrProfitLoss = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrPips = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrTimeOpened = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hdrTimeClosed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.imlIcons = new System.Windows.Forms.ImageList(this.components);
            this.multiChart1 = new Chartoscope.Toolbox.MultiChart();
            this.timeNavigator1 = new Chartoscope.Toolbox.TimeNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtEquityCurve)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.multiChart1);
            this.splitContainer1.Panel2.Controls.Add(this.timeNavigator1);
            this.splitContainer1.Size = new System.Drawing.Size(1336, 515);
            this.splitContainer1.SplitterDistance = 445;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chtEquityCurve);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lvwOrders);
            this.splitContainer2.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer2.Size = new System.Drawing.Size(445, 515);
            this.splitContainer2.SplitterDistance = 98;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // chtEquityCurve
            // 
            chartArea2.Name = "ChartArea1";
            this.chtEquityCurve.ChartAreas.Add(chartArea2);
            this.chtEquityCurve.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chtEquityCurve.Legends.Add(legend2);
            this.chtEquityCurve.Location = new System.Drawing.Point(0, 0);
            this.chtEquityCurve.Name = "chtEquityCurve";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chtEquityCurve.Series.Add(series2);
            this.chtEquityCurve.Size = new System.Drawing.Size(445, 98);
            this.chtEquityCurve.TabIndex = 0;
            this.chtEquityCurve.Text = "chart1";
            // 
            // lvwOrders
            // 
            this.lvwOrders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrProfitLoss,
            this.hdrPips,
            this.hdrPosition,
            this.hdrTimeOpened,
            this.hdrTimeClosed});
            this.lvwOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwOrders.FullRowSelect = true;
            this.lvwOrders.GridLines = true;
            this.lvwOrders.Location = new System.Drawing.Point(0, 0);
            this.lvwOrders.Name = "lvwOrders";
            this.lvwOrders.Size = new System.Drawing.Size(445, 390);
            this.lvwOrders.TabIndex = 1;
            this.lvwOrders.UseCompatibleStateImageBehavior = false;
            this.lvwOrders.View = System.Windows.Forms.View.Details;
            // 
            // hdrProfitLoss
            // 
            this.hdrProfitLoss.Text = "P/L";
            this.hdrProfitLoss.Width = 30;
            // 
            // hdrPips
            // 
            this.hdrPips.Text = "Pips";
            this.hdrPips.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.hdrPips.Width = 41;
            // 
            // hdrPosition
            // 
            this.hdrPosition.Text = "Position";
            this.hdrPosition.Width = 58;
            // 
            // hdrTimeOpened
            // 
            this.hdrTimeOpened.Text = "Time Opened";
            this.hdrTimeOpened.Width = 142;
            // 
            // hdrTimeClosed
            // 
            this.hdrTimeClosed.Text = "Time Closed";
            this.hdrTimeClosed.Width = 166;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 390);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(445, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // imlIcons
            // 
            this.imlIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imlIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // multiChart1
            // 
            this.multiChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multiChart1.Location = new System.Drawing.Point(0, 0);
            this.multiChart1.Name = "multiChart1";
            this.multiChart1.SelectedTimeFrame = null;
            this.multiChart1.Size = new System.Drawing.Size(886, 405);
            this.multiChart1.TabIndex = 1;
            // 
            // timeNavigator1
            // 
            this.timeNavigator1.CurrentDate = new System.DateTime(((long)(0)));
            this.timeNavigator1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.timeNavigator1.Location = new System.Drawing.Point(0, 405);
            this.timeNavigator1.MinimumSize = new System.Drawing.Size(0, 110);
            this.timeNavigator1.Name = "timeNavigator1";
            this.timeNavigator1.Size = new System.Drawing.Size(886, 110);
            this.timeNavigator1.TabIndex = 2;
            // 
            // SignalChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SignalChart";
            this.Size = new System.Drawing.Size(1336, 515);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtEquityCurve)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtEquityCurve;
        private System.Windows.Forms.ListView lvwOrders;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private MultiChart multiChart1;
        private System.Windows.Forms.ColumnHeader hdrProfitLoss;
        private System.Windows.Forms.ColumnHeader hdrPosition;
        private System.Windows.Forms.ColumnHeader hdrTimeOpened;
        private System.Windows.Forms.ColumnHeader hdrTimeClosed;
        private System.Windows.Forms.ImageList imlIcons;
        private System.Windows.Forms.ColumnHeader hdrPips;
        private TimeNavigator timeNavigator1;

    }
}
