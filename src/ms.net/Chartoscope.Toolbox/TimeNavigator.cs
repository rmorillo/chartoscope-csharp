using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chartoscope.Toolbox
{
    public partial class TimeNavigator : UserControl
    {
        private DateTime rangeBegin;
        private DateTime rangeEnd;
        private DateTime beginMonth;
        private DateTime endMonth;
        private DateTime currentMonth;
        private DateTime beginDay;
        private DateTime endDay;
        private DateTime currentDay;
        private DateTime beginHour;
        private DateTime endHour;
        private DateTime currentHour;

        public event ChartingDelegates.ChangeCurrentDateHandler ChangeCurrentDate;

        private DateTime currentDate;

        public DateTime CurrentDate
        {
            get { return currentDate; }
            set 
            { 
                currentDate = value;
                SetDateTime(value);
            }
        }
        

        public TimeNavigator()
        {
            InitializeComponent();
        }

        public void Initialize(DateTime rangeBegin, DateTime rangeEnd)
        {
            this.rangeBegin = rangeBegin;
            this.rangeEnd = rangeEnd;

            this.beginMonth = new DateTime(rangeBegin.Year, rangeBegin.Month, 1);
            this.endMonth = new DateTime(rangeEnd.Year, rangeEnd.Month, 1);
            tbrMonthly.Maximum = MonthDifference(this.beginMonth, this.endMonth) + 1;
            //this.currentMonth = this.beginMonth;

            this.beginDay = this.beginMonth;
            this.endDay = new DateTime(this.beginDay.Year, this.beginMonth.Month, DateTime.DaysInMonth(this.beginDay.Year, this.beginDay.Month));
            tbrDaily.Maximum = DateTime.DaysInMonth(this.beginDay.Year, this.beginDay.Month) - 1;
            //this.currentDay = this.beginDay;

            this.beginHour = new DateTime(this.beginDay.Year, this.beginDay.Month, this.beginDay.Day, 0, 0, 0);
            this.endHour = new DateTime(this.beginDay.Year, this.beginDay.Month, this.beginDay.Day, 23, 0, 0);
            tbrHourly.Maximum = 23;
            //this.currentHour = this.beginHour;

            SetDateTime(rangeBegin);
        }

        private void SetDateTime(DateTime newDateTime)
        {
            this.currentMonth = new DateTime(newDateTime.Year, newDateTime.Month, 1);
            tbrMonthly.Value = MonthDifference(this.beginMonth, this.currentMonth);
            this.currentDay = new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day);
            tbrDaily.Value = newDateTime.Day - 1;
            this.currentHour = new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day, newDateTime.Hour, 0 , 0);
            tbrHourly.Value = newDateTime.Hour;
        }

        private int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }

        private void tbrHourly_Scroll(object sender, EventArgs e)
        {
            this.currentHour = new DateTime(this.currentDay.Year, this.currentDay.Month, this.currentDay.Day, tbrHourly.Value<24? tbrHourly.Value: 0, 0, 0);
            this.currentDate = this.currentHour;

            if (this.ChangeCurrentDate != null)
            {
                ChangeCurrentDate(this.currentDate);
            }
        }

        private void tbrDaily_Scroll(object sender, EventArgs e)
        {
            this.currentDay = new DateTime(this.currentMonth.Year, this.currentMonth.Month, tbrDaily.Value + 1);
            this.currentDate = new DateTime(this.currentDay.Year, this.currentDay.Month, this.currentDay.Day, this.currentHour.Hour, 0, 0);

            if (this.ChangeCurrentDate != null)
            {
                ChangeCurrentDate(this.currentDate);
            }
        }

        private void tbrMonthly_Scroll(object sender, EventArgs e)
        {
            DateTime targetDate= this.beginMonth.AddMonths(tbrMonthly.Value);
            this.currentMonth = new DateTime(targetDate.Year, targetDate.Month, 1);

            int targetDay = this.currentDay.Day;

            if (targetDay == DateTime.DaysInMonth(this.currentDay.Year, this.currentDay.Month))
            {
                targetDay = DateTime.DaysInMonth(targetDate.Year, targetDate.Month);
                tbrDaily.Maximum = targetDay - 1;
                tbrDaily.Value = targetDay - 1;
            }
            else
            {
                tbrDaily.Maximum = DateTime.DaysInMonth(targetDate.Year, targetDate.Month) - 1;
            }
                       
            this.currentDate = new DateTime(this.currentMonth.Year, this.currentMonth.Month, targetDay, this.currentHour.Hour, 0, 0);

            if (this.ChangeCurrentDate != null)
            {
                ChangeCurrentDate(this.currentDate);
            }
        }
    }
}
