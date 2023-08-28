using System;
using System.IO;
using NinjaTrader.Cbi;
using NinjaTrader.Gui.Tools;
using NinjaTrader.NinjaScript;
using NinjaTrader.Data;

namespace NinjaTrader.NinjaScript.Strategies
{
    public class ExportIndicatorValues : Strategy
    {
        private Indicators.BuySellVolumeRate volumeRateIndicator;
		private Indicators.POC_EMA pocEMA1;
		private Indicators.POC_EMA pocEMA2;
		private Indicators.EntropyIndicator EntropyVal;

        private StreamWriter sw;

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"Exports indicator values to CSV";
                Name = "ExportIndicatorValues";
            }
            else if (State == State.Configure)
            {
                volumeRateIndicator = BuySellVolumeRate(Input, true);
				if (volumeRateIndicator == null){ Print("This1");}
				pocEMA1 = POC_EMA(21,1);
				pocEMA2 = POC_EMA(4,1);
				EntropyVal = EntropyIndicator(Input);
				if (EntropyVal == null){ Print("This2");}
            }
            else if (State == State.DataLoaded)
            {
				
                string path = @"C:\Users\omid\Desktop\Data\datavals.csv";
                sw = new StreamWriter(path, true);
                sw.WriteLine(" PreviousBarClose, CurrentBarClose, POCDiff, EntropyValue, BuySellRate"); // Write the header once
            }
            else if (State == State.Terminated)
            {
                if (sw != null)
				{
    				sw.Close();
				}

            }
        }
		
		protected override void OnBarUpdate()
		{
           	if (CurrentBar < BarsRequiredToPlot)
                return;

            double pocDifference = pocEMA1[0] - pocEMA2[0];
			Print("pocDifference" + pocDifference);
			Print("EntropyVal" + EntropyVal[0]);
			Print("volumeRateIndicator" + volumeRateIndicator[0]);
			
			sw.WriteLine(Close[1] + ", " + Close[0] + ", " + pocDifference + ", " + EntropyVal[0] + ", " + volumeRateIndicator[0]);

        }
    }
}
