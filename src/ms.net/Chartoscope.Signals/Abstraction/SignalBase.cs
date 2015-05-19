using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chartoscope.Common;
using Chartoscope.Indicators;

namespace Chartoscope.Signals
{
    public class SignalBase: ISignal
    {
        private List<AnalyticsItem> registeredAnalytics = null;

        private bool isInPosition = false;

        public bool IsInPosition { get { return isInPosition; } }

        public bool IsOutPosition { get { return !isInPosition; } }

        private PositionMode position;

        public PositionMode Position { get { return position; } }

        protected BarItemType barType = null;

        public BarItemType BarType { get { return barType; } }

        public List<AnalyticsItem> RegisteredAnalytics { get { return this.registeredAnalytics; } }

        private long signalCounter = 0;

        public SignalBase()
        {
            this.registeredAnalytics = new List<AnalyticsItem>();
        }

        public event SignalDelegates.EntrySignalHandler EnterPosition;
       
        public event SignalDelegates.ExitSignalHandler ExitPosition;

        void ISignal.InPositionState(PriceBars priceBar, BarItemType barType)
        {
            InPosition(position, priceBar, barType);
        }

        void ISignal.OutPositionState(PriceBars priceBar, BarItemType barType)
        {
            OutPosition(priceBar, barType);
        }

        protected virtual void InPosition(PositionMode position, PriceBars priceBar, BarItemType barType)
        {
        }

        protected virtual void OutPosition(PriceBars priceBar, BarItemType barType)
        {
        }

        protected void Enter(PositionMode position)
        {
            if (EnterPosition != null)
            {
                signalCounter++;
                EnterPosition(this, signalCounter, position);
                this.position = position;
                this.isInPosition = true;
            }
        }

        protected void Exit()
        {
            if (ExitPosition != null)
            {
                signalCounter++;
                ExitPosition(this, signalCounter);
                this.isInPosition = false;
            }
        }


        protected void Register(params object[] instances)
        {
            foreach (object instance in instances)
            {
                if (instance is IIndicatorCore)
                {
                    this.registeredAnalytics.Add(new AnalyticsItem(AnalyticsTypeOption.Indicator, instance));
                }
            }
        }

        protected string identityCode = null;

        public string IdentityCode
        {
            get { return identityCode; }
        }

        public bool CrossesBelow(double startMain, double endMain, double startSub, double endSub)
        {
            return (startMain > startSub && endMain < endSub);
        }

        public bool CrossesAbove(double startMain, double endMain, double startSub, double endSub)
        {
            return (startMain < startSub && endMain > endSub);
        }


        public void ForceExit()
        {
            this.Exit();
        }
    }
}
