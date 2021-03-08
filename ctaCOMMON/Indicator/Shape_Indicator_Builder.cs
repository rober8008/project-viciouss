using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.Indicator
{
    public static class Shape_Indicator_Builder
    {
        public static Shape_Indicator BuildShapeInstance(string shape_name)
        {
            switch (shape_name.ToLower())
            {
                case "line":
                    return new Line();
                case "text":
                    return new Text();
                case "fibbonaccilinelow":
                    return new FibbonacciLinesLow();
                case "fibbonaccilinehigh":
                    return new FibbonacciLinesHigh();
                case "fibbonacciarcslow":
                    return new FibbonacciArcsLow();
                case "fibbonacciarcshigh":
                    return new FibbonacciArcsHigh();
                case "poligone":
                    return new Poligone();                
                default:
                    return null;
            }
        }
    }
}
