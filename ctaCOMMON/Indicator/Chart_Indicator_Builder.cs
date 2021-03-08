using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public static class Chart_Indicator_Builder
    {
        public static Chart_Indicator BuildIndicatorInstance(string indicator_name)
        {
            switch(indicator_name.ToLower())
            {
                case "boolingerbands":
                    return new BoolingerBands();
                case "ema":
                    return new EMA();
                case "ma":
                    return new MA();
                case "macd":
                    return new MACD();
                case "momentum":
                    return new Momentum();
                case "rsi":
                    return new RSI();
                case "stochasticoscillatorfast":
                    return new StochasticOscillatorFast();
                case "stochasticoscillatorslow":
                    return new StochasticOscillatorSlow();
                case "volumen":
                    return new Volume();
                case "roc":
                    return new ROC();
                case "williamsr":
                    return new WilliansR();
                case "obv":
                    return new OBV();
                case "volumeoscillator":
                    return new VolumeOscillator();
                case "crucemedias":
                    return new CruceMedias();
                case "coefcorrelacion":
                    return new CoefCorr();
                default:
                    return null;                    
            }
        }
    }
}
