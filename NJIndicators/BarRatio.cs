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
    public class BarRatio : Indicator
    {

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = "Computes the Bar Ratio based on volume.";
                Name = "BarRatio";
                Calculate = Calculate.OnBarClose;
                IsOverlay = true;
                DrawOnPricePanel = true;
                DisplayInDataBox = true;
                IsAutoScale = false;
				

        		OnlyShowRatioForBarColor = true;
            }
        }
		
		[NinjaScriptProperty]
		[Display(Name="Only show ratio for bar color", Description="Show bullish ratio only for green bars and bearish ratio only for red bars", Order=1, GroupName="Parameters")]
		public bool OnlyShowRatioForBarColor { get; set; }

		
		protected override void OnBarUpdate()
{
    NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

    double highestPrice = High[0];
    double lowestPrice = Low[0];

    double firstLowestBid = double.MaxValue;
    double secondLowestBid = double.MaxValue;
    double firstHighestAsk = 0;
    double secondHighestAsk = 0;
	
	for (double price = lowestPrice; price <= highestPrice; price += TickSize)
{
    double bidVolume = barsType.Volumes[CurrentBar].GetBidVolumeForPrice(price);
    double askVolume = barsType.Volumes[CurrentBar].GetAskVolumeForPrice(price);

    // Identify two lowest bids
    if (bidVolume < firstLowestBid)
    {
        secondLowestBid = firstLowestBid;
        firstLowestBid = bidVolume;
    }
    else if (bidVolume < secondLowestBid && bidVolume != firstLowestBid)
    {
        secondLowestBid = bidVolume;
    }

    // Identify two highest asks
    if (askVolume > firstHighestAsk)
    {
        secondHighestAsk = firstHighestAsk;
        firstHighestAsk = askVolume;
    }
    else if (askVolume > secondHighestAsk && askVolume != firstHighestAsk)
    {
        secondHighestAsk = askVolume;
    }
}

    // For bullish bars
    if (Close[0] > Open[0] && secondLowestBid != 0) // Added a check to prevent division by zero
    {
		if (!OnlyShowRatioForBarColor || (OnlyShowRatioForBarColor && Close[0] > Open[0]))
    {
		double bullishRatio = firstLowestBid / secondLowestBid;
        Draw.Text(this, "BullRatio" + CurrentBar, bullishRatio.ToString("0.00"), 0, High[0] + 3 * TickSize, Brushes.Green);
    }
       
    }

    // For bearish bars
    if (Close[0] < Open[0] && secondHighestAsk != 0) // Added a check to prevent division by zero
    {
		 if (!OnlyShowRatioForBarColor || (OnlyShowRatioForBarColor && Close[0] < Open[0]))
    {
		double bearishRatio = firstHighestAsk / secondHighestAsk;
        Draw.Text(this, "BearRatio" + CurrentBar, bearishRatio.ToString("0.00"), 0, Low[0] - 3 * TickSize, Brushes.Red);
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
		private BarRatio[] cacheBarRatio;
		public BarRatio BarRatio(bool onlyShowRatioForBarColor)
		{
			return BarRatio(Input, onlyShowRatioForBarColor);
		}

		public BarRatio BarRatio(ISeries<double> input, bool onlyShowRatioForBarColor)
		{
			if (cacheBarRatio != null)
				for (int idx = 0; idx < cacheBarRatio.Length; idx++)
					if (cacheBarRatio[idx] != null && cacheBarRatio[idx].OnlyShowRatioForBarColor == onlyShowRatioForBarColor && cacheBarRatio[idx].EqualsInput(input))
						return cacheBarRatio[idx];
			return CacheIndicator<BarRatio>(new BarRatio(){ OnlyShowRatioForBarColor = onlyShowRatioForBarColor }, input, ref cacheBarRatio);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.BarRatio BarRatio(bool onlyShowRatioForBarColor)
		{
			return indicator.BarRatio(Input, onlyShowRatioForBarColor);
		}

		public Indicators.BarRatio BarRatio(ISeries<double> input , bool onlyShowRatioForBarColor)
		{
			return indicator.BarRatio(input, onlyShowRatioForBarColor);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.BarRatio BarRatio(bool onlyShowRatioForBarColor)
		{
			return indicator.BarRatio(Input, onlyShowRatioForBarColor);
		}

		public Indicators.BarRatio BarRatio(ISeries<double> input , bool onlyShowRatioForBarColor)
		{
			return indicator.BarRatio(input, onlyShowRatioForBarColor);
		}
	}
}

#endregion
