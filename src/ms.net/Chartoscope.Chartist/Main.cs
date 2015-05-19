using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chartoscope.Brokers;
using Chartoscope.Common;
using Chartoscope.Indicators;
using Chartoscope.Services;
using Chartoscope.Signals;
using Chartoscope.Strategies;
using Chartoscope.Toolbox;

namespace Chartoscope.Chartist
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        
        private void Main_Load(object sender, EventArgs e)
        {
            SignalCharting session = new SignalCharting(signalChart1, SpotForex.EURUSD, Timeframes.H1, @"data\EURUSD-M1.bar");
            //ChartingSession session = new ChartingSession(multiChart1, SpotForex.EURUSD, Timeframes.M15, @"data\EURUSD-M1.bar");
            //session.Register(new ParabolicSAR(Timeframes.M30, 5));
            //session.Register(new PSARandStochastic2(session.BarType));
            //session.Register(new PSARandADX(session.BarType));
            //session.Register(new FastMovingMACrossover(session.BarType));
            //session.Register(new SlowMovingMACrossover(session.BarType));
            //session.Register(new MA28And100Trading(session.BarType));
            //session.Register(new StochasticHighLow(session.BarType));
            //session.Register(new RSIHighLow(session.BarType));   
            //session.Register(new StochasticLinesCrossover(session.BarType));   
            //session.Register(new DoubleStochastic(session.BarType));   
            //session.Register(new EMABreakthrough(session.BarType));   
            //session.Register(new MyLineInTheSand(session.BarType));              
            //session.Register(new H4BollingerBandStrategy(session.BarType));  
            //session.Register(new WhiteSoldiersBlackCrows(session.BarType));  
            //session.Register(new ADXPower(session.BarType)); 
            //session.Register(new SimpleBalancedSystem(session.BarType));  
            //session.Register(new EurUsdSimpleSystem(session.BarType)); 
            //session.Register(new Simple5x5System(session.BarType)); 
            //session.Register(new KeySimplicity(session.BarType));
            //session.Register(new TeodosiSimpleSystem(session.BarType));
            //session.Register(new ArsalanADXandEMACross(session.BarType));
            //session.Register(new ArsalanADXandMACD(session.BarType));
            //session.Register(new ArsalanStochasticAndEMA(session.BarType));
            //session.Register(new ThoseFourIndicators(session.BarType));
            session.Register(new BestOfMACD(session.BarType));
            //BackgroundSignal session = new BackgroundSignal(SpotForex.EURUSD, Timeframes.D1, @"data\EURUSD-M1.bar");
            //session.Register(new MyLineInTheSand(session.BarType)); 
            //session.Start();

            //StrategyCharting session = new StrategyCharting(strategyChart1, SpotForex.EURUSD, Timeframes.H1, @"data\EURUSD-M1.bar");
            //session.Register(new FixedProfitSrategy(new H4BollingerBandStrategy(session.BarType)));
            //session.Register(new FixedProfitSrategy(new BestOfMACD(session.BarType)));
            session.Start();

        }
        
    }
}
