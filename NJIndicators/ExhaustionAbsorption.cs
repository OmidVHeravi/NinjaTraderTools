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
	public class ExhaustionAbsorption : Indicator
{
    private double[] volumes;
    private double[] prices;
    private double prevHighVolumePrice = double.NaN;

    protected override void OnStateChange()
    {
        if (State == State.SetDefaults)
        {
            Description = "Identifies Exhaustion and Absorption levels";
            Name = "ExhaustionAbsorption";
            IsOverlay = true;
            DrawOnPricePanel = true;
        }
        else if (State == State.Configure)
        {
        }
    }

    protected override void OnBarUpdate()
    {
        NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

        volumes = new double[(int)((High[0] - Low[0]) / TickSize) + 1];
        prices = new double[volumes.Length];

        double highBidVolumePrice = double.NaN;
        double highAskVolumePrice = double.NaN;
        double highBidVolume = 0;
        double highAskVolume = 0;

        for (int i = 0; i < volumes.Length; i++)
        {
            double price = Low[0] + i * TickSize;
            prices[i] = price;

            double bidVolume = barsType.Volumes[CurrentBar].GetBidVolumeForPrice(price);
            double askVolume = barsType.Volumes[CurrentBar].GetAskVolumeForPrice(price);
            volumes[i] = bidVolume + askVolume;

            if (bidVolume > highBidVolume)
            {
                highBidVolume = bidVolume;
                highBidVolumePrice = price;
            }
            if (askVolume > highAskVolume)
            {
                highAskVolume = askVolume;
                highAskVolumePrice = price;
            }
        }

        int maxVolumeIndex = Array.IndexOf(volumes, volumes.Max());

        // Exhaustion indicator
        if (highBidVolumePrice == High[0])  // If the high bid volume is at the high of the move
        {
            Draw.Dot(this, "ExhaustionTop" + CurrentBar, false, 0, High[0] + 2 * TickSize, Brushes.Red);
        }
        if (highAskVolumePrice == Low[0])  // If the high ask volume is at the low of the move
        {
            Draw.Dot(this, "ExhaustionBottom" + CurrentBar, false, 0, Low[0] - 2 * TickSize, Brushes.Green);
        }

        // Absorption indicator
        if (CurrentBar > 0 && (High[0] == prevHighVolumePrice || Low[0] == prevHighVolumePrice))  // If the high or low of the current bar is at the high volume price level of the previous bar
        {
            Draw.Dot(this, "Absorption" + CurrentBar, false, 0, prevHighVolumePrice, Brushes.Blue);
        }

        prevHighVolumePrice = prices[maxVolumeIndex];
    }
}

}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private ExhaustionAbsorption[] cacheExhaustionAbsorption;
		public ExhaustionAbsorption ExhaustionAbsorption()
		{
			return ExhaustionAbsorption(Input);
		}

		public ExhaustionAbsorption ExhaustionAbsorption(ISeries<double> input)
		{
			if (cacheExhaustionAbsorption != null)
				for (int idx = 0; idx < cacheExhaustionAbsorption.Length; idx++)
					if (cacheExhaustionAbsorption[idx] != null &&  cacheExhaustionAbsorption[idx].EqualsInput(input))
						return cacheExhaustionAbsorption[idx];
			return CacheIndicator<ExhaustionAbsorption>(new ExhaustionAbsorption(), input, ref cacheExhaustionAbsorption);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.ExhaustionAbsorption ExhaustionAbsorption()
		{
			return indicator.ExhaustionAbsorption(Input);
		}

		public Indicators.ExhaustionAbsorption ExhaustionAbsorption(ISeries<double> input )
		{
			return indicator.ExhaustionAbsorption(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.ExhaustionAbsorption ExhaustionAbsorption()
		{
			return indicator.ExhaustionAbsorption(Input);
		}

		public Indicators.ExhaustionAbsorption ExhaustionAbsorption(ISeries<double> input )
		{
			return indicator.ExhaustionAbsorption(input);
		}
	}
}

#endregion
