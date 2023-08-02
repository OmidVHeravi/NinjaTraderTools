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
#endregion


//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class DDInd : Indicator
	{
		
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description = @"Delta Divergence Indicator";
				Name = "DeltaDivergence";
				Calculate = Calculate.OnBarClose;
				IsOverlay = true;
				DrawOnPricePanel = true;
			}
			else if (State == State.Configure)
			{
				
				//AddVolumetric("ES 09-23", BarsPeriodType.Range, 10, VolumetricDeltaType.BidAsk, 1);
			}
		}
		
		protected override void OnBarUpdate()
		{
    	// Only proceed if there are at least 2 bars on the chart
    	if (CurrentBar < 1)
        	return;

    	NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;
    	//double bidVolume = barsType.Volumes[CurrentBar].GetBidVolumeForPrice(Close[0]);
		//double askVolume = barsType.Volumes[CurrentBar].GetAskVolumeForPrice(Close[0]);
		//double delt = askVolume - bidVolume;
		double delt = barsType.Volumes[CurrentBar].BarDelta;

    	//Print("Delta: " + delt);

    	if ((Close[0] > Open[0]) && delt < 0)  // The bar closed up and the volume delta is negative
    	{
			Print("==========");
			Print("Time: " + Time[0]);
			Print("Delta: " + delt);
			Print("Price: " + (Close[0] - Open[0]));
        	Print("Drawing up arrow");
        	Draw.ArrowUp(this, "PositiveDeltaDivergence" + CurrentBar, true, 0, Low[0] - TickSize, Brushes.Cyan);
    	}

    	if ((Close[0] < Open[0]) && delt > 0)  // The bar closed down and the volume delta is positive
    	{
			Print("==========");
			Print("Time: " + Time[0]);
			Print("Delta: " + delt);
			Print("Price: " + (Close[0] - Open[0]));
        	Print("Drawing down arrow");
        	Draw.ArrowDown(this, "NegativeDeltaDivergence" + CurrentBar, true, 0, High[0] + TickSize, Brushes.Red);
    	}
}


	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private DDInd[] cacheDDInd;
		public DDInd DDInd()
		{
			return DDInd(Input);
		}

		public DDInd DDInd(ISeries<double> input)
		{
			if (cacheDDInd != null)
				for (int idx = 0; idx < cacheDDInd.Length; idx++)
					if (cacheDDInd[idx] != null &&  cacheDDInd[idx].EqualsInput(input))
						return cacheDDInd[idx];
			return CacheIndicator<DDInd>(new DDInd(), input, ref cacheDDInd);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.DDInd DDInd()
		{
			return indicator.DDInd(Input);
		}

		public Indicators.DDInd DDInd(ISeries<double> input )
		{
			return indicator.DDInd(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.DDInd DDInd()
		{
			return indicator.DDInd(Input);
		}

		public Indicators.DDInd DDInd(ISeries<double> input )
		{
			return indicator.DDInd(input);
		}
	}
}

#endregion
