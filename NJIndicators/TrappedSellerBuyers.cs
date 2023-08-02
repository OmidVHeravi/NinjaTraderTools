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
	public class TrappedSellerBuyers : Indicator
	{
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Using Imabalance to highlight trapped sellers";
				Name										= "TrappedSellerBuyers";
				Calculate = Calculate.OnBarClose;
				IsOverlay = true;
				DrawOnPricePanel = true;
			}
			else if (State == State.Configure)
			{
				
			}
		}
		
		[Range(1, int.MaxValue), NinjaScriptProperty]
		[Display(Name="StackedCount", Description="Number of stacked imbalances to filter for", Order=1, GroupName="Parameters")]
		public int StackedCount { get; set; }

		[NinjaScriptProperty]
		[Display(Name="AllowNonStacked", Description="Allow any number of imbalances in a bar", Order=2, GroupName="Parameters")]
		public bool AllowNonStacked { get; set; }

		private int bidImbalanceCount = 0;
		private int askImbalanceCount = 0;

		public TrappedSellerBuyers()  // Replace YourIndicatorName with the actual name of your indicator
		{
    		// Set default values
    		StackedCount = 3;
    		AllowNonStacked = false;
		}
		
		protected override void OnBarUpdate()
		{
   	 	// Only proceed if there are at least 2 bars on the chart
    	if (CurrentBar < 1)
        	return;

    	NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

    	for (double price = Low[0]; price <= High[0]; price += TickSize)  // Loop over all possible prices within the bar
    	{
        	double bidVolume = barsType.Volumes[CurrentBar].GetBidVolumeForPrice(price);
        	double askVolume = barsType.Volumes[CurrentBar].GetAskVolumeForPrice(price);

        	if (bidVolume == 0)  // If there is no bid volume
        	{
            	bidImbalanceCount++;
            	askImbalanceCount = 0;  // Reset the count of ask imbalances
            	if (bidImbalanceCount >= StackedCount || AllowNonStacked)
            	{
                	Draw.Rectangle(this, "NoBidVolume" + CurrentBar + price, true, 0, price - TickSize, 1, price + TickSize, Brushes.Cyan, Brushes.Transparent, 60);
            	}
        	}

        	if (askVolume == 0)  // If there is no ask volume
        	{
            	askImbalanceCount++;
            	bidImbalanceCount = 0;  // Reset the count of bid imbalances
            	if (askImbalanceCount >= StackedCount || AllowNonStacked)
            	{
                	Draw.Rectangle(this, "NoAskVolume" + CurrentBar + price, true, 0, price - TickSize, 1, price + TickSize, Brushes.Red, Brushes.Transparent, 60);
            	}
        	}
   	 	}
		}
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private TrappedSellerBuyers[] cacheTrappedSellerBuyers;
		public TrappedSellerBuyers TrappedSellerBuyers(int stackedCount, bool allowNonStacked)
		{
			return TrappedSellerBuyers(Input, stackedCount, allowNonStacked);
		}

		public TrappedSellerBuyers TrappedSellerBuyers(ISeries<double> input, int stackedCount, bool allowNonStacked)
		{
			if (cacheTrappedSellerBuyers != null)
				for (int idx = 0; idx < cacheTrappedSellerBuyers.Length; idx++)
					if (cacheTrappedSellerBuyers[idx] != null && cacheTrappedSellerBuyers[idx].StackedCount == stackedCount && cacheTrappedSellerBuyers[idx].AllowNonStacked == allowNonStacked && cacheTrappedSellerBuyers[idx].EqualsInput(input))
						return cacheTrappedSellerBuyers[idx];
			return CacheIndicator<TrappedSellerBuyers>(new TrappedSellerBuyers(){ StackedCount = stackedCount, AllowNonStacked = allowNonStacked }, input, ref cacheTrappedSellerBuyers);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.TrappedSellerBuyers TrappedSellerBuyers(int stackedCount, bool allowNonStacked)
		{
			return indicator.TrappedSellerBuyers(Input, stackedCount, allowNonStacked);
		}

		public Indicators.TrappedSellerBuyers TrappedSellerBuyers(ISeries<double> input , int stackedCount, bool allowNonStacked)
		{
			return indicator.TrappedSellerBuyers(input, stackedCount, allowNonStacked);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.TrappedSellerBuyers TrappedSellerBuyers(int stackedCount, bool allowNonStacked)
		{
			return indicator.TrappedSellerBuyers(Input, stackedCount, allowNonStacked);
		}

		public Indicators.TrappedSellerBuyers TrappedSellerBuyers(ISeries<double> input , int stackedCount, bool allowNonStacked)
		{
			return indicator.TrappedSellerBuyers(input, stackedCount, allowNonStacked);
		}
	}
}

#endregion
