namespace Chartoscope.Toolbox
{
    partial class TimeNavigator
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
            this.tbrHourly = new System.Windows.Forms.TrackBar();
            this.tbrDaily = new System.Windows.Forms.TrackBar();
            this.tbrMonthly = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbrHourly)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbrDaily)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbrMonthly)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbrHourly
            // 
            this.tbrHourly.AutoSize = false;
            this.tbrHourly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbrHourly.LargeChange = 4;
            this.tbrHourly.Location = new System.Drawing.Point(35, 3);
            this.tbrHourly.Maximum = 24;
            this.tbrHourly.Name = "tbrHourly";
            this.tbrHourly.Size = new System.Drawing.Size(1037, 28);
            this.tbrHourly.TabIndex = 0;
            this.tbrHourly.Scroll += new System.EventHandler(this.tbrHourly_Scroll);
            // 
            // tbrDaily
            // 
            this.tbrDaily.AutoSize = false;
            this.tbrDaily.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbrDaily.LargeChange = 2;
            this.tbrDaily.Location = new System.Drawing.Point(35, 37);
            this.tbrDaily.Maximum = 31;
            this.tbrDaily.Name = "tbrDaily";
            this.tbrDaily.Size = new System.Drawing.Size(1037, 32);
            this.tbrDaily.TabIndex = 1;
            this.tbrDaily.Scroll += new System.EventHandler(this.tbrDaily_Scroll);
            // 
            // tbrMonthly
            // 
            this.tbrMonthly.AutoSize = false;
            this.tbrMonthly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbrMonthly.LargeChange = 1;
            this.tbrMonthly.Location = new System.Drawing.Point(35, 75);
            this.tbrMonthly.Maximum = 5;
            this.tbrMonthly.Name = "tbrMonthly";
            this.tbrMonthly.Size = new System.Drawing.Size(1037, 32);
            this.tbrMonthly.TabIndex = 2;
            this.tbrMonthly.Scroll += new System.EventHandler(this.tbrMonthly_Scroll);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox6, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox4, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbrHourly, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbrMonthly, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbrDaily, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1112, 110);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::Chartoscope.Toolbox.Properties.Resources.months_trackbar;
            this.pictureBox6.Location = new System.Drawing.Point(1075, 72);
            this.pictureBox6.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(32, 32);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 8;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::Chartoscope.Toolbox.Properties.Resources.months_trackbar;
            this.pictureBox5.Location = new System.Drawing.Point(0, 72);
            this.pictureBox5.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(32, 32);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox5.TabIndex = 7;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::Chartoscope.Toolbox.Properties.Resources.days_trackbar;
            this.pictureBox4.Location = new System.Drawing.Point(1075, 34);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(32, 32);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 6;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::Chartoscope.Toolbox.Properties.Resources.days_trackbar;
            this.pictureBox3.Location = new System.Drawing.Point(0, 34);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 32);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Chartoscope.Toolbox.Properties.Resources.DefaultTheme_16;
            this.pictureBox2.Location = new System.Drawing.Point(1075, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Chartoscope.Toolbox.Properties.Resources.DefaultTheme_16;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // TimeNavigator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(0, 110);
            this.Name = "TimeNavigator";
            this.Size = new System.Drawing.Size(1112, 110);
            ((System.ComponentModel.ISupportInitialize)(this.tbrHourly)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbrDaily)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbrMonthly)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar tbrHourly;
        private System.Windows.Forms.TrackBar tbrDaily;
        private System.Windows.Forms.TrackBar tbrMonthly;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox6;
    }
}
