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
    public class PrintSpots : Indicator
    {
        private double volumeThreshold = 10; // Example threshold, can be adjusted

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = "Indicator for Small, Zero Prints and Thin Spots";
                Name = "PrintSpots";
                IsOverlay = true;
            }
            else if (State == State.Configure)
            {
            }
        }

        protected override void OnBarUpdate()
        {
            // Example logic for Zero Prints
            if (Volume[0] == 0) 
			{
    			DrawOnPricePanel = false;  // Not drawing directly on price panel
    			Draw.Region(this, "ZeroPrint" + CurrentBar, 0, 0, High, Low, Brushes.Transparent, Brushes.Gray, 50);
			}


            // Logic for Small Prints and Thin Spots can be added similarly
			// Example logic for Small Prints
			if (Volume[0] <= VolumeThreshold) 	
			{
    			DrawOnPricePanel = false;  // Not drawing directly on price panel
    			Draw.Region(this, "SmallPrint" + CurrentBar, 0, 0, High, Low, Brushes.Transparent, Brushes.Blue, 50);
			}

			// Example logic for Thin Spots
			NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;
			double bidVolume = barsType.Volumes[CurrentBar].GetBidVolumeForPrice(Close[0]);
			double askVolume = barsType.Volumes[CurrentBar].GetAskVolumeForPrice(Close[0]);

			// Assuming bid volume being significantly larger than ask volume indicates a thin spot (and vice-versa). 
			// The "2" here is arbitrary and represents a "significantly larger" ratio. Adjust as needed.
			if (bidVolume > 2 * askVolume || askVolume > 2 * bidVolume) 
			{
    			DrawOnPricePanel = false;
    	Draw.Region(this, "ThinSpot" + CurrentBar, 0, 0, High, Low, Brushes.Transparent, Brushes.Purple, 50);
}

        }

        #region Properties

        [NinjaScriptProperty]
        [Range(1, int.MaxValue)]
        [Display(Name="Volume Threshold", Description="Threshold for small prints", Order=1, GroupName="Parameters")]
        public double VolumeThreshold
        {
            get { return volumeThreshold; }
            set { volumeThreshold = value; }
        }

        // Additional properties for enabling/disabling Zero Prints, Small Prints, and Thin Spots can be added here.

        #endregion
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private PrintSpots[] cachePrintSpots;
		public PrintSpots PrintSpots(double volumeThreshold)
		{
			return PrintSpots(Input, volumeThreshold);
		}

		public PrintSpots PrintSpots(ISeries<double> input, double volumeThreshold)
		{
			if (cachePrintSpots != null)
				for (int idx = 0; idx < cachePrintSpots.Length; idx++)
					if (cachePrintSpots[idx] != null && cachePrintSpots[idx].VolumeThreshold == volumeThreshold && cachePrintSpots[idx].EqualsInput(input))
						return cachePrintSpots[idx];
			return CacheIndicator<PrintSpots>(new PrintSpots(){ VolumeThreshold = volumeThreshold }, input, ref cachePrintSpots);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.PrintSpots PrintSpots(double volumeThreshold)
		{
			return indicator.PrintSpots(Input, volumeThreshold);
		}

		public Indicators.PrintSpots PrintSpots(ISeries<double> input , double volumeThreshold)
		{
			return indicator.PrintSpots(input, volumeThreshold);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.PrintSpots PrintSpots(double volumeThreshold)
		{
			return indicator.PrintSpots(Input, volumeThreshold);
		}

		public Indicators.PrintSpots PrintSpots(ISeries<double> input , double volumeThreshold)
		{
			return indicator.PrintSpots(input, volumeThreshold);
		}
	}
}

#endregion
