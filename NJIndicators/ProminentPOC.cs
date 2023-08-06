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
namespace NinjaTrader.NinjaScript.Indicators{
public class ProminentPOC : Indicator
{
    private double[] volumes;
    private double[] prices;
	private Series<double> pocSeries;

    protected override void OnStateChange()
    {
        if (State == State.SetDefaults)
        {
            Description = "Identifies Prominent POC levels";
            Name = "ProminentPOC";
            IsOverlay = true;
            DrawOnPricePanel = true;
        }
        else if (State == State.Configure)
        {
			pocSeries = new Series<double>(this, MaximumBarsLookBack.Infinite);
        }
    }

    protected override void OnBarUpdate()
    {
		// Only proceed if there are at least 2 bars on the chart
    	if (CurrentBar < 1)
        	return;
		
        NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

        volumes = new double[(int)((High[0] - Low[0]) / TickSize) + 1];
        prices = new double[volumes.Length];

        for (int i = 0; i < volumes.Length; i++)
        {
            double price = Low[0] + i * TickSize;
            prices[i] = price;
            volumes[i] = barsType.Volumes[CurrentBar].GetBidVolumeForPrice(price) + barsType.Volumes[CurrentBar].GetAskVolumeForPrice(price);
        }

       int maxVolumeIndex = Array.IndexOf(volumes, volumes.Max());

//        if (prices[maxVolumeIndex] == High[0] || prices[maxVolumeIndex] == Low[0])
//        {
//			Stroke stroke = new Stroke(Brushes.Red, 2);
//            Draw.Line(this, "POC" + CurrentBar, 0, prices[maxVolumeIndex], -5, prices[maxVolumeIndex], Brushes.Red);
			
//			// Print timestamp and price level where line is drawn
//            Print("Line drawn at time " + Time[0].ToString() + " at price level " + prices[maxVolumeIndex]);
//        }
		
		pocSeries[0] = prices[maxVolumeIndex];

        // Check if the current POC matches the previous bar's POC
        if (CurrentBar > 0 && pocSeries[0] == pocSeries[1])
        {
            Draw.Rectangle(this, "POCMatch" + CurrentBar, true, 1, pocSeries[0] + TickSize, -1, pocSeries[0] - TickSize, Brushes.Transparent, Brushes.Cyan, 20);
        }
    }
}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private ProminentPOC[] cacheProminentPOC;
		public ProminentPOC ProminentPOC()
		{
			return ProminentPOC(Input);
		}

		public ProminentPOC ProminentPOC(ISeries<double> input)
		{
			if (cacheProminentPOC != null)
				for (int idx = 0; idx < cacheProminentPOC.Length; idx++)
					if (cacheProminentPOC[idx] != null &&  cacheProminentPOC[idx].EqualsInput(input))
						return cacheProminentPOC[idx];
			return CacheIndicator<ProminentPOC>(new ProminentPOC(), input, ref cacheProminentPOC);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.ProminentPOC ProminentPOC()
		{
			return indicator.ProminentPOC(Input);
		}

		public Indicators.ProminentPOC ProminentPOC(ISeries<double> input )
		{
			return indicator.ProminentPOC(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.ProminentPOC ProminentPOC()
		{
			return indicator.ProminentPOC(Input);
		}

		public Indicators.ProminentPOC ProminentPOC(ISeries<double> input )
		{
			return indicator.ProminentPOC(input);
		}
	}
}

#endregion
