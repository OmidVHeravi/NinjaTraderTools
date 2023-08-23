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
using NinjaTrader.Gui.Tools;

#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class BuySellVolumeRate : Indicator
{
    private Series<double> buyingVolumeRate;
    private Series<double> sellingVolumeRate;

    private double buyingVolumeCounter = 0;
    private double sellingVolumeCounter = 0;
	private double rate1 = 0;
	private double rate2 = 0;
    private DateTime lastTimeUpdate;
	private BuySellVolumeRate buySellVolumeRateIndicator;


    protected override void OnStateChange()
    {
        if (State == State.SetDefaults)
        {
            Description = "Tracks rate of change of buying and selling volume per second.";
            Name = "BuySellVolumeRate";
            Calculate = Calculate.OnEachTick; 

            buyingVolumeRate = new Series<double>(this);
            sellingVolumeRate = new Series<double>(this);
            
            AddPlot(Brushes.Green, "Buying Volume Rate");
            AddPlot(Brushes.Red, "Selling Volume Rate");
			
			OnlyShowRatio = true;
			
        }
    }
	
	[NinjaScriptProperty]
	[Display(Name="Only show ratio of Buy/Sell Volume", Description="Shows the ratio Buy/Sell Volume", Order=1, GroupName="Parameters")]
	public bool OnlyShowRatio { get; set; }

    protected override void OnBarUpdate()
    {
        if (CurrentBar == 0)
        {
            lastTimeUpdate = Time[0];
            return;
        }

        NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

        double bidVolume = barsType.Volumes[CurrentBar].GetBidVolumeForPrice(Close[0]);
        double askVolume = barsType.Volumes[CurrentBar].GetAskVolumeForPrice(Close[0]);

        buyingVolumeCounter += askVolume; 
        sellingVolumeCounter += bidVolume; 

        if (Time[0].Second != lastTimeUpdate.Second)
        {
            buyingVolumeRate[0] = buyingVolumeCounter; 
            sellingVolumeRate[0] = sellingVolumeCounter; 

            buyingVolumeCounter = 0;
            sellingVolumeCounter = 0;
            lastTimeUpdate = Time[0];
        } 
		
		if (!OnlyShowRatio)
		{
			//Values[0][0] = buyingVolumeRate[0];
        	//Values[1][0] = sellingVolumeRate[0];
			double rate1 = buyingVolumeRate[0] / sellingVolumeRate[0];
			double rate2 = buyingVolumeRate[1] / sellingVolumeRate[1];

			// Calculating the "derivative" or rate of change
			double rateChange = rate1 - rate2;

			// Apply the arctan transformation
			double transformedRateChange = Math.Atan(rateChange);

			Values[0][0] = transformedRateChange;
			
		}
		else {
			double rate1 = buyingVolumeRate[0] / sellingVolumeRate[0];
			double rate2 = buyingVolumeRate[1] / sellingVolumeRate[1];

			// Calculating the "derivative" or rate of change
			double rateChange = rate1 - rate2;

			// Apply the arctan transformation
			double transformedRateChange = Math.Atan(rateChange);

			Values[0][0] = transformedRateChange;


		}

       
    }
}

}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private BuySellVolumeRate[] cacheBuySellVolumeRate;
		public BuySellVolumeRate BuySellVolumeRate(bool onlyShowRatio)
		{
			return BuySellVolumeRate(Input, onlyShowRatio);
		}

		public BuySellVolumeRate BuySellVolumeRate(ISeries<double> input, bool onlyShowRatio)
		{
			if (cacheBuySellVolumeRate != null)
				for (int idx = 0; idx < cacheBuySellVolumeRate.Length; idx++)
					if (cacheBuySellVolumeRate[idx] != null && cacheBuySellVolumeRate[idx].OnlyShowRatio == onlyShowRatio && cacheBuySellVolumeRate[idx].EqualsInput(input))
						return cacheBuySellVolumeRate[idx];
			return CacheIndicator<BuySellVolumeRate>(new BuySellVolumeRate(){ OnlyShowRatio = onlyShowRatio }, input, ref cacheBuySellVolumeRate);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.BuySellVolumeRate BuySellVolumeRate(bool onlyShowRatio)
		{
			return indicator.BuySellVolumeRate(Input, onlyShowRatio);
		}

		public Indicators.BuySellVolumeRate BuySellVolumeRate(ISeries<double> input , bool onlyShowRatio)
		{
			return indicator.BuySellVolumeRate(input, onlyShowRatio);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.BuySellVolumeRate BuySellVolumeRate(bool onlyShowRatio)
		{
			return indicator.BuySellVolumeRate(Input, onlyShowRatio);
		}

		public Indicators.BuySellVolumeRate BuySellVolumeRate(ISeries<double> input , bool onlyShowRatio)
		{
			return indicator.BuySellVolumeRate(input, onlyShowRatio);
		}
	}
}

#endregion
