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
	public class AboveBelowPOC : Indicator
{
    private SharpDX.Direct2D1.Brush brushAbovePOC;
    private SharpDX.Direct2D1.Brush brushBelowPOC;

    protected override void OnStateChange()
{
    if (State == State.SetDefaults)
    {
        Description = "Highlights bars based on their open/close relation to POC";
        Name = "AboveBelowPOC";
        IsOverlay = true;
        DrawOnPricePanel = true;
    }
    else if (State == State.Configure)
    {
        // You can configure other things here, but maybe not the brushes yet.
    }
}


    protected override void OnBarUpdate()
    {
        if (CurrentBar < 1) // Ensure we have at least two bars
            return;

        NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

   		double pocPrice = double.MinValue;
    	double maxVolume = double.MinValue;
		TimeSpan barDuration = Time[0] - Time[1]; 
		TimeSpan halfBarDuration = TimeSpan.FromTicks(barDuration.Ticks / 2);
		DateTime adjustedStartTime = Time[1].Add(halfBarDuration);
		DateTime adjustedEndTime = Time[0].Add(halfBarDuration);


    	for (double price = Low[0]; price <= High[0]; price += TickSize)
    	{
       	 		double volumeAtPrice = barsType.Volumes[CurrentBar].GetBidVolumeForPrice(price) + barsType.Volumes[CurrentBar].GetAskVolumeForPrice(price);
        	if (volumeAtPrice > maxVolume)
        {
            maxVolume = volumeAtPrice;
            pocPrice = price;
        }
    	}

 	 if (Open[0] > pocPrice && Close[0] > pocPrice)
    {
		
        Draw.RegionHighlightX(this, "regionAbovePOC" + CurrentBar, adjustedStartTime, adjustedEndTime, Brushes.White, Brushes.LimeGreen, 20);

    }
    else if (Open[0] < pocPrice && Close[0] < pocPrice)
    {
		Draw.RegionHighlightX(this, "regionBelowPOC" + CurrentBar, adjustedStartTime, adjustedEndTime, Brushes.White, Brushes.Red, 20);

    }
	
	}
}


}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private AboveBelowPOC[] cacheAboveBelowPOC;
		public AboveBelowPOC AboveBelowPOC()
		{
			return AboveBelowPOC(Input);
		}

		public AboveBelowPOC AboveBelowPOC(ISeries<double> input)
		{
			if (cacheAboveBelowPOC != null)
				for (int idx = 0; idx < cacheAboveBelowPOC.Length; idx++)
					if (cacheAboveBelowPOC[idx] != null &&  cacheAboveBelowPOC[idx].EqualsInput(input))
						return cacheAboveBelowPOC[idx];
			return CacheIndicator<AboveBelowPOC>(new AboveBelowPOC(), input, ref cacheAboveBelowPOC);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.AboveBelowPOC AboveBelowPOC()
		{
			return indicator.AboveBelowPOC(Input);
		}

		public Indicators.AboveBelowPOC AboveBelowPOC(ISeries<double> input )
		{
			return indicator.AboveBelowPOC(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.AboveBelowPOC AboveBelowPOC()
		{
			return indicator.AboveBelowPOC(Input);
		}

		public Indicators.AboveBelowPOC AboveBelowPOC(ISeries<double> input )
		{
			return indicator.AboveBelowPOC(input);
		}
	}
}

#endregion
