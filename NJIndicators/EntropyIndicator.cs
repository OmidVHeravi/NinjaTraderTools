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
using NinjaTrader.Data;
using NinjaTrader.NinjaScript.Strategies;
using NinjaTrader.NinjaScript.Indicators;
#endregion

namespace NinjaTrader.NinjaScript.Indicators
{
    public class EntropyIndicator : Indicator
    {
        private double[] volumeBins;
        private int numberOfBins = 3; // You can adjust this as needed
        private double minVolume;
        private double maxVolume;
		
		private Series<double> entropyValues;


        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"Entropy indicator based on volume.";
                Name = "EntropyIndicator";
                Calculate = Calculate.OnEachTick;
                IsOverlay = false;
                DisplayInDataBox = true;
                DrawOnPricePanel = true;
                DrawHorizontalGridLines = true;
                DrawVerticalGridLines = true;
                PaintPriceMarkers = true;
                ScaleJustification = NinjaTrader.Gui.Chart.ScaleJustification.Right;
				

				AddPlot(Brushes.Blue, "Entropy");
            }
            else if (State == State.Configure)
            {
            }
            else if (State == State.DataLoaded)
            {
				Print("Init");
                volumeBins = new double[numberOfBins];
				
				volumeBins = new double[numberOfBins];
        		entropyValues = new Series<double>(this, MaximumBarsLookBack.Infinite);

            }
        }

        protected override void OnBarUpdate()
        {
			
			Print("Current Bar: " + CurrentBar);
			
			
            if (CurrentBar <= numberOfBins)
            {
                return;
            }

            // Reset bins
            for (int i = 0; i < numberOfBins; i++)
            {
				Print("1");
                volumeBins[i] = 0;
            }
			
			Print("Bins reset.");

            // Determine min and max volume for range
            minVolume = double.MaxValue;
            maxVolume = double.MinValue;

            for (int i = 0; i < numberOfBins; i++)
            {
				Print("2");
				if (i >= Volume.Count)
				{
    				Print("Error: Trying to access out-of-bounds index in Volume series. Current index: " + i);
    				return;
				}

                double volume = Volume[i];
                if (volume < minVolume) minVolume = volume;
                if (volume > maxVolume) maxVolume = volume;
            }

            double range = maxVolume - minVolume;
            if (range == 0) return;

            // Populate bins
            for (int i = 0; i < numberOfBins; i++)
            {
				Print("A");
				if (i >= Volume.Count)
				{
   					 Print("Error: Trying to access out-of-bounds index in Volume series. Current index: " + i);
   					 return;
				}

                int binIndex = (int)((Volume[i] - minVolume) / range * (numberOfBins - 1));
				Print("B");
				Print("Bin index for bar " + i + " is: " + binIndex);
                volumeBins[binIndex]++;
				Print("C");
            }

            // Calculate entropy
           	double totalVolume = 0;
			
			for (int i = 0; i < numberOfBins; i++)
			{
				Print("D");
    			totalVolume += Volume[i];
				Print("E");
			}

            double entropy = 0;

            Print("Total volume: " + totalVolume);

			for (int i = 0; i < numberOfBins; i++)
			{
				Print("i: " + i);
    			Print("Accessing volumeBins[" + i + "] of " + volumeBins.Length);
				if (i >= volumeBins.Length)
			{
    			Print("Error: Trying to access out-of-bounds index in volumeBins. Current index: " + i);
    			return;
			}
				
			double probability = volumeBins[i] / totalVolume;

    		//double probability = volumeBins[i] / totalVolume;
    		Print("Probability for bin " + i + ": " + probability);
    		if (probability > 0)  // only compute if probability is not zero
    			{
        			double logValue = Math.Log(probability, 10);
        			Print("Log Value for bin " + i + ": " + logValue);
        			entropy -= probability * logValue;
    			}
			}


			Print("F");

			//Print("Value is: " + Value[0]);			
           	// Assign entropy to custom series
    		entropyValues[0] = entropy;

    		// Also update the plot
    		Values[0][0] = 100 * entropy;
			
			Print("G");

			
        }

        #region Properties

        [Browsable(false)]
        [XmlIgnore()]
        public Series<double> Value
        {
            get { return Values[0]; }
        }

        #endregion
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private EntropyIndicator[] cacheEntropyIndicator;
		public EntropyIndicator EntropyIndicator()
		{
			return EntropyIndicator(Input);
		}

		public EntropyIndicator EntropyIndicator(ISeries<double> input)
		{
			if (cacheEntropyIndicator != null)
				for (int idx = 0; idx < cacheEntropyIndicator.Length; idx++)
					if (cacheEntropyIndicator[idx] != null &&  cacheEntropyIndicator[idx].EqualsInput(input))
						return cacheEntropyIndicator[idx];
			return CacheIndicator<EntropyIndicator>(new EntropyIndicator(), input, ref cacheEntropyIndicator);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.EntropyIndicator EntropyIndicator()
		{
			return indicator.EntropyIndicator(Input);
		}

		public Indicators.EntropyIndicator EntropyIndicator(ISeries<double> input )
		{
			return indicator.EntropyIndicator(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.EntropyIndicator EntropyIndicator()
		{
			return indicator.EntropyIndicator(Input);
		}

		public Indicators.EntropyIndicator EntropyIndicator(ISeries<double> input )
		{
			return indicator.EntropyIndicator(input);
		}
	}
}

#endregion
