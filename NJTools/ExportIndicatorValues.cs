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
                volumeRateIndicator = BuySellVolumeRate(Input, false);; // Initialize the indicator instance
            }
            else if (State == State.DataLoaded)
            {
                string path = @"C:\Users\omid\Desktop\Data\BuySellVolumeRate.csv";
                sw = new StreamWriter(path, true);
                sw.WriteLine("DateTime, BuySellVolumeRateValue, Close, Open"); // Write the header once
            }
            else if (State == State.Terminated)
            {
                sw.Close(); // Close the StreamWriter when the strategy terminates
            }
        }

        protected override void OnBarUpdate()
        {
            if (CurrentBar < 1) // Ensure we have at least one bar
                return;

            // Export the current bar's data
            sw.WriteLine(Time[CurrentBar].ToString("yyyy-MM-dd HH:mm:ss") + ", " + volumeRateIndicator.Values[0][0] + ", " + Close[0] + ", " + Open[0]);
        }
    }
}
