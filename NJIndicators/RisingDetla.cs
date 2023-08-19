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

namespace NinjaTrader.NinjaScript.Indicators
{
    public class RisingDelta : Indicator
    {
        private int consecutiveRisingDelta = 0;
        private double lastDelta = 0;

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"Detects four consecutive bars with rising delta.";
                Name = "RisingDelta";
                IsOverlay = true;
            }
            else if (State == State.Configure)
            {
                consecutiveRisingDelta = 0;
                lastDelta = 0;
            }
        }

        protected override void OnBarUpdate()
        {
            if (CurrentBar < 1) // Ensure we have at least one previous bar
                return;

            NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;
            double currentDelta = barsType.Volumes[CurrentBar].BarDelta;

			Print("Bar" + CurrentBar + "Delta =" + currentDelta);
			Print("Last Delta =" + lastDelta + "Consecutive Count =" + consecutiveRisingDelta); // Debug print
			
            if (currentDelta > lastDelta)
                consecutiveRisingDelta++;
            else
                consecutiveRisingDelta = 0; // Reset the counter

            lastDelta = currentDelta;

            // If we have four consecutive bars with rising delta
            if (consecutiveRisingDelta >= 4)
            {
                // Draw a diamond on the current bar
                Draw.Diamond(this, "risingDelta" + CurrentBar, true, 0, High[0] + 12 * TickSize, Brushes.Cyan);
                consecutiveRisingDelta = 0; // Reset the counter
            }
        }
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private RisingDelta[] cacheRisingDelta;
		public RisingDelta RisingDelta()
		{
			return RisingDelta(Input);
		}

		public RisingDelta RisingDelta(ISeries<double> input)
		{
			if (cacheRisingDelta != null)
				for (int idx = 0; idx < cacheRisingDelta.Length; idx++)
					if (cacheRisingDelta[idx] != null &&  cacheRisingDelta[idx].EqualsInput(input))
						return cacheRisingDelta[idx];
			return CacheIndicator<RisingDelta>(new RisingDelta(), input, ref cacheRisingDelta);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.RisingDelta RisingDelta()
		{
			return indicator.RisingDelta(Input);
		}

		public Indicators.RisingDelta RisingDelta(ISeries<double> input )
		{
			return indicator.RisingDelta(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.RisingDelta RisingDelta()
		{
			return indicator.RisingDelta(Input);
		}

		public Indicators.RisingDelta RisingDelta(ISeries<double> input )
		{
			return indicator.RisingDelta(input);
		}
	}
}

#endregion
