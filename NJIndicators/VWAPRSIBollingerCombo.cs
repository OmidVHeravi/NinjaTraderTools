#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui.Tools;
using NinjaTrader.NinjaScript;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript.Strategies;
using NinjaTrader.NinjaScript.Indicators;
#endregion

// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.NinjaScript.Indicators
{
    public class VWAPRSIBollingerCombo : Indicator
    {
        private OrderFlowVWAP vwap;
		private Bollinger bollinger;
		private RSI rsi;

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"VWAP, RSI, and Bollinger Bands Combo Indicator";
                Name = "VWAPRSIBollingerCombo";
                Calculate = Calculate.OnEachTick;
                IsOverlay = true;
                DisplayInDataBox = true;
                DrawOnPricePanel = true;
                DrawHorizontalGridLines = true;
                DrawVerticalGridLines = true;
                PaintPriceMarkers = true;
                ScaleJustification = NinjaTrader.Gui.Chart.ScaleJustification.Right;
            }
            else if (State == State.Configure)
   			 {
        		// Create an instance of the VWAP, Bollinger Bands, and RSI
        		vwap = OrderFlowVWAP(VWAPResolution.Standard, Bars.TradingHours, VWAPStandardDeviations.Two, 1, 2, 3);
        		bollinger = Bollinger(2, 21);
        		rsi = RSI(14, 2);
			}
        }

        protected override void OnBarUpdate()
        {
//			Print("Bar" + Time[0]);
//    		Print("VWAP:" + vwap.VWAP[0]);
//    		Print("Close:" + Close[0]);
//    		Print("RSI:" + rsi.Value[0]);
//    		Print("Bollinger Upper:" + bollinger.Upper[0]);
//    		Print("Bollinger Lower:" + bollinger.Lower[0]);
			
    if (CurrentBar < 15) return;

    double vwapValue = vwap.VWAP[13];  // Notice I used [15] because we're looking at the VWAP from 15 bars ago.
    double upperBand = bollinger.Upper[0];
    double lowerBand = bollinger.Lower[0];
    double rsiValue = rsi[0];

    // Bullish Condition
    if (Close[0] > vwapValue && rsiValue < 40 && Close[0] < lowerBand)
    {
		Print(" Buy Bar" + Time[0]);
        Draw.Text(this, "B" + CurrentBar, "B", 0, Low[0] - TickSize * 5, Brushes.Cyan);
    }

    // Bearish Condition
    if (Close[0] < vwapValue && rsiValue > 60 && Close[0] > upperBand)
    {
		Print(" Sell Bar" + Time[0]);
        Draw.Text(this, "S" + CurrentBar, "S", 0, High[0] + TickSize * 5, Brushes.Red);
    }
	
		}


		}

//        #region Properties
//        [Browsable(false)]
//        [XmlIgnore()]
//        public Series<double> Value
//        {
//            get { return Values[0]; }
//        }
//        #endregion
 

}

//This namespace holds Indicators in this folder and is required. Do not change it. 

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private VWAPRSIBollingerCombo[] cacheVWAPRSIBollingerCombo;
		public VWAPRSIBollingerCombo VWAPRSIBollingerCombo()
		{
			return VWAPRSIBollingerCombo(Input);
		}

		public VWAPRSIBollingerCombo VWAPRSIBollingerCombo(ISeries<double> input)
		{
			if (cacheVWAPRSIBollingerCombo != null)
				for (int idx = 0; idx < cacheVWAPRSIBollingerCombo.Length; idx++)
					if (cacheVWAPRSIBollingerCombo[idx] != null &&  cacheVWAPRSIBollingerCombo[idx].EqualsInput(input))
						return cacheVWAPRSIBollingerCombo[idx];
			return CacheIndicator<VWAPRSIBollingerCombo>(new VWAPRSIBollingerCombo(), input, ref cacheVWAPRSIBollingerCombo);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.VWAPRSIBollingerCombo VWAPRSIBollingerCombo()
		{
			return indicator.VWAPRSIBollingerCombo(Input);
		}

		public Indicators.VWAPRSIBollingerCombo VWAPRSIBollingerCombo(ISeries<double> input )
		{
			return indicator.VWAPRSIBollingerCombo(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.VWAPRSIBollingerCombo VWAPRSIBollingerCombo()
		{
			return indicator.VWAPRSIBollingerCombo(Input);
		}

		public Indicators.VWAPRSIBollingerCombo VWAPRSIBollingerCombo(ISeries<double> input )
		{
			return indicator.VWAPRSIBollingerCombo(input);
		}
	}
}

#endregion