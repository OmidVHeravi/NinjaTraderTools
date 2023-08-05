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
	public class POC_EMA : Indicator
{
    private double[] volumes;
    private double[] prices;
    private Series<double> pocSeries;
    private EMA ema;

    [Range(1, int.MaxValue), NinjaScriptProperty]
    [Display(Name="Period", Description="Period for the EMA", Order=1, GroupName="Parameters")]
    public int Period { get; set; }

    [XmlIgnore]
    [Display(ResourceType = typeof(Custom.Resource), Name = "EMA line color", Description = "Sets the color of the EMA line", GroupName = "Visual", Order = 0)]
    public Brush EmaColor { get; set; }

    [Browsable(false)]
    public string EmaColorSerializable
    {
        get { return Serialize.BrushToString(EmaColor); }
        set { EmaColor = Serialize.StringToBrush(value); }
    }

    [Range(1, int.MaxValue), NinjaScriptProperty]
    [Display(Name="Line width", Description="Width of the EMA line", Order=2, GroupName="Parameters")]
    public int LineWidth { get; set; }

    protected override void OnStateChange()
    {
        if (State == State.SetDefaults)
        {
            Description = "EMA of POCs";
            Name = "POC_EMA";
            IsOverlay = true;
            DrawOnPricePanel = true;
            Period = 14;
            EmaColor = Brushes.Red;
            LineWidth = 2;
        }
        else if (State == State.DataLoaded)
        {
            pocSeries = new Series<double>(this, MaximumBarsLookBack.Infinite);
            ema = EMA(pocSeries, Period);
        }
    }

    protected override void OnBarUpdate()
    {
        NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType barsType = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

        volumes = new double[(int)((High[0] - Low[0]) / TickSize) + 1];
        prices = new double[volumes.Length];

        for (int i = 0; i < volumes.Length; i++)
        {
            double price = Low[0] + i * TickSize;
            prices[i] = price;
            volumes[i] = barsType.Volumes[CurrentBar].GetBidVolumeForPrice(price) + barsType.Volumes[CurrentBar].GetAskVolumeForPrice(price);
        }

        int maxVolumeIndex = Array.IndexOf(volumes, volumes.Max());

        pocSeries[0] = prices[maxVolumeIndex];

         if (CurrentBar >= Period && !double.IsNaN(ema[0]) && !double.IsNaN(ema[1]))
    {
        if (ema[0] >= Low[0] && ema[0] <= High[0] && ema[1] >= Low[1] && ema[1] <= High[1])
        {
            Draw.Line(this, "POC_EMA" + CurrentBar, false, 0, ema[0], -1, ema[1], EmaColor, DashStyleHelper.Solid, LineWidth);
        }
        else
        {
            Draw.Dot(this, "POC_EMA" + CurrentBar, false, 0, ema[0], EmaColor);
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
		private POC_EMA[] cachePOC_EMA;
		public POC_EMA POC_EMA(int period, int lineWidth)
		{
			return POC_EMA(Input, period, lineWidth);
		}

		public POC_EMA POC_EMA(ISeries<double> input, int period, int lineWidth)
		{
			if (cachePOC_EMA != null)
				for (int idx = 0; idx < cachePOC_EMA.Length; idx++)
					if (cachePOC_EMA[idx] != null && cachePOC_EMA[idx].Period == period && cachePOC_EMA[idx].LineWidth == lineWidth && cachePOC_EMA[idx].EqualsInput(input))
						return cachePOC_EMA[idx];
			return CacheIndicator<POC_EMA>(new POC_EMA(){ Period = period, LineWidth = lineWidth }, input, ref cachePOC_EMA);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.POC_EMA POC_EMA(int period, int lineWidth)
		{
			return indicator.POC_EMA(Input, period, lineWidth);
		}

		public Indicators.POC_EMA POC_EMA(ISeries<double> input , int period, int lineWidth)
		{
			return indicator.POC_EMA(input, period, lineWidth);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.POC_EMA POC_EMA(int period, int lineWidth)
		{
			return indicator.POC_EMA(Input, period, lineWidth);
		}

		public Indicators.POC_EMA POC_EMA(ISeries<double> input , int period, int lineWidth)
		{
			return indicator.POC_EMA(input, period, lineWidth);
		}
	}
}

#endregion
