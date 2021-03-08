using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaCOMMON.DataParser
{
    public class jsonmdlBOLSARIndex
    {
        [JsonProperty("Apertura")]
        public string strApertura { get; set; }
        [JsonProperty("Cierre")]
        public string strCierre { get; set; }
        [JsonProperty("Hora")]
        public string strHora { get; set; }
        [JsonProperty("Maximo_Valor")]
        public string strMaximo_Valor { get; set; }
        [JsonProperty("Minimo_Valor")]
        public string strMinimo_Valor { get; set; }
        [JsonProperty("Nombre")]
        public string strNombre { get; set; }
        [JsonProperty("Promedio_Diario")]
        public string strPromedio_Diario { get; set; }
        [JsonProperty("Tendencia")]
        public string strTendencia { get; set; }
        [JsonProperty("Ultimo")]
        public string strUltimo { get; set; }
        [JsonProperty("Variacion")]
        public string strVariacion { get; set; }

        public string Symbol
        {
            get
            {
                if (this.Nombre == "MERVAL")
                    return "MERV";
                else if (this.Nombre == "MERVAL 25")
                    return "IM25";
                else if (this.Nombre == "M.AR")
                    return "IAR";
                else
                    return "";
            }
        }

        [JsonIgnore]
        public double Apertura
        {
            get
            {
                double result = 0;
                double.TryParse(this.strApertura, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public double Cierre
        {
            get
            {
                double result = 0;
                double.TryParse(this.strCierre, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public double Maximo_Valor
        {
            get
            {
                double result = 0;
                double.TryParse(this.strMaximo_Valor, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public double Minimo_Valor
        {
            get
            {
                double result = 0;
                double.TryParse(this.strMinimo_Valor, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public string Nombre
        {
            get
            {
                return this.strNombre;
            }
        }

        [JsonIgnore]
        public double Promedio_Diario
        {
            get
            {
                double result = 0;
                double.TryParse(this.strPromedio_Diario, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public int Tendencia
        {
            get
            {
                int result = 0;
                int.TryParse(this.strTendencia, out result);
                return result;
            }
        }

        [JsonIgnore]
        public double Ultimo
        {
            get
            {
                double result = 0;
                double.TryParse(this.strUltimo, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public double Variacion
        {
            get
            {
                double result = 0;
                double.TryParse(this.strVariacion, out result);
                return Math.Round(result, 2);
            }
        }
        
    }
}
