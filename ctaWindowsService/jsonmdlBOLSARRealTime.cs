using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaWindowsService
{
    internal class jsonmdlBOLSARRealTime
    {
        [JsonProperty("Apertura")]
        public string strApertura { get; set; }

        [JsonProperty("Cantidad_Nominal_Compra")]        
        public string strCantidad_Nominal_Compra { get; set; }

        [JsonProperty("Cantidad_Nominal_Venta")]
        public string strCantidad_Nominal_Venta { get; set; }

        [JsonProperty("Cantidad_Operaciones")]
        public string strCantidad_Operaciones { get; set; }

        [JsonProperty("Cierre_Anterior")]
        public string strCierre_Anterior { get; set; }

        [JsonProperty("Denominacion")]
        public string strDenominacion { get; set; }

        [JsonProperty("Estado")]
        public string strEstado { get; set; }

        [JsonProperty("Ex")]
        public string strEx { get; set; }

        [JsonProperty("Hora_Cotizacion")]
        public string strHora_Cotizacion { get; set; }

        [JsonProperty("Maximo")]
        public string strMaximo { get; set; }

        [JsonProperty("Minimo")]
        public string strMinimo { get; set; }

        [JsonProperty("Monto_Operado_Pesos")]
        public string strMonto_Operado_Pesos { get; set; }

        [JsonProperty("Precio_Compra")]
        public string strPrecio_Compra { get; set; }

        [JsonProperty("Precio_Promedio")]
        public string strPrecio_Promedio { get; set; }

        [JsonProperty("Precio_Promedio_Ponderado")]
        public string strPrecio_Promedio_Ponderado { get; set; }

        [JsonProperty("Precio_Venta")]
        public string strPrecio_Venta { get; set; }

        [JsonProperty("Simbolo")]
        public string strSimbolo { get; set; }

        [JsonProperty("Tendencia")]
        public string strTendencia { get; set; }

        [JsonProperty("Tipo_Liquidacion")]
        public string strTipo_Liquidacion { get; set; }

        [JsonProperty("Ultimo")]
        public string strUltimo { get; set; }

        [JsonProperty("Variacion")]
        public string strVariacion { get; set; }

        [JsonProperty("Vencimiento")]
        public string strVencimiento { get; set; }

        [JsonProperty("Volumen_Nominal")]
        public string strVolumen_Nominal { get; set; }

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
        public int CantidadNominalCompra
        {
            get
            {
                int result = 0;
                int.TryParse(this.strCantidad_Nominal_Compra, out result);
                return result;
            }
        }

        [JsonIgnore]
        public int CantidadNominalVenta
        {
            get
            {
                int result = 0;
                int.TryParse(this.strCantidad_Nominal_Venta, out result);
                return result;
            }
        }

        [JsonIgnore]
        public int CantidadOperaciones
        {
            get
            {
                int result = 0;
                int.TryParse(this.strCantidad_Operaciones, out result);
                return result;
            }
        }

        [JsonIgnore]
        public double CierreAnterior
        {
            get
            {
                double result = 0;
                double.TryParse(this.strCierre_Anterior, out result);
                return Math.Round(result,2);
            }
        }

        [JsonIgnore]
        public string Denominacion
        {
            get
            {
                return this.strDenominacion;
            }
        }

        [JsonIgnore]
        public string Estado
        {
            get
            {
                return this.strEstado;
            }
        }

        [JsonIgnore]
        public string Ex
        {
            get
            {
                return this.strEx;
            }
        }

        [JsonIgnore]
        public string Hora_Cotizacion
        {
            get
            {
                return this.strHora_Cotizacion;
            }
        }

        [JsonIgnore]
        public double Maximo
        {
            get
            {
                double result = 0;
                double.TryParse(this.strMaximo, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public double Minimo
        {
            get
            {
                double result = 0;
                double.TryParse(this.strMinimo, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public int MontoOperadoPesos
        {
            get
            {
                int result = 0;
                int.TryParse(this.strMonto_Operado_Pesos, out result);
                return result;
            }
        }

        [JsonIgnore]
        public double PrecioCompra
        {
            get
            {
                double result = 0;
                double.TryParse(this.strPrecio_Compra, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public double PrecioPromedio
        {
            get
            {
                double result = 0;
                double.TryParse(this.strPrecio_Promedio, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public double PrecioPromedioPonderado
        {
            get
            {
                double result = 0;
                double.TryParse(this.strPrecio_Promedio_Ponderado, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public double PrecioVenta
        {
            get
            {
                double result = 0;
                double.TryParse(this.strPrecio_Venta, out result);
                return Math.Round(result, 2);
            }
        }

        [JsonIgnore]
        public string Simbolo
        {
            get
            {
                return this.strSimbolo;
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
        public string TipoLiquidacion
        {
            get
            {
                return this.strTipo_Liquidacion;
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

        [JsonIgnore]
        public string Vencimiento
        {
            get
            {
                return this.strVencimiento;
            }
        }

        [JsonIgnore]
        public int VolumenNominal
        {
            get
            {
                int result = 0;
                int.TryParse(this.strVolumen_Nominal, out result);
                return result;
            }
        }
    }
}
