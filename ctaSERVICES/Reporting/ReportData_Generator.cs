using ctaDATAMODEL;
using ctaCOMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MoreLinq;
using ctaCOMMON.Indicator;
using ctaCOMMON.Charts;

namespace ctaSERVICES.Reporting
{
    public class ReportData_Generator
    {
        public Indicator_Calculator Calculator;

        public ReportData_Generator()
        {
            this.Calculator = new Indicator_Calculator();
        }

        private void DeleteReportData(string batch)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Stock_Report.RemoveRange(entities.Stock_Report.Where(q => q.Stock.technical_report_batch == batch || batch == "ALL"));
                entities.SaveChanges();                
            }
        }

        public void DeleteReportData()
        {
            this.DeleteReportData("ALL");
        }

        public void GenerateReportData(string batch)
        {
            this.DeleteReportData(batch);
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                var stocks = entities.Stocks.Where(s => s.technical_report_batch == batch).ToList();

                //this.CalculateValues(stocks, entities);
                foreach (Stock stock in stocks)
                {
                    Stock_Report stockReport = this.GetStockReportData(stock);
                    if(stockReport != null)
                        entities.Stock_Report.Add(stockReport);
                }
                
                entities.SaveChanges();
            }                
        }

        private Stock_Report GetStockReportData(Stock stock)
        {
            try
            {
                List<Candel> candels = stock.Stock_Quote.Select(sq => new Candel()
                {
                    Date = sq.date_round,
                    Close = sq.closing,
                    Maximun = sq.maximun,
                    Minimun = sq.minimun,
                    Open = sq.opening,
                    Visible = true,
                    Volume = (double)sq.volume
                }).ToList();

                if (candels.Any())
                {
                    MA ma20 = new MA(candels, 20, "");
                    MA ma50 = new MA(candels, 50, "");
                    MA ma200 = new MA(candels, 200, "");
                    EMA ema12 = new EMA(candels, 12, "", false);
                    EMA ema20 = new EMA(candels, 20, "", false);
                    EMA ema26 = new EMA(candels, 26, "", false);
                    RSI rsi14 = new RSI(candels, 14, 0, 0, "", "");
                    Momentum momentum12 = new Momentum(candels, 12, "", "");
                    StochasticOscillatorFast soFast = new StochasticOscillatorFast(candels, 14, 0, 0, "", "", "");
                    MACD macd1226 = new MACD(candels, 12, 26, 9, "", "");
                    BoolingerBands boolinger20 = new BoolingerBands(candels, 20, "");
                    WilliansR williansr14 = new WilliansR(candels, 14, 0, 0, "", "", "");

                    Stock_Report result = new Stock_Report();
                    result.Stock_ID = stock.Id;
                    result.Price = stock.Stock_Quote.Last().closing;
                    result.Date_Round = stock.Stock_Quote.Last().date_round;
                    result.MA20 = ma20.Series[0].Data.Last().Value;
                    result.MA50 = ma50.Series[0].Data.Last().Value;
                    result.MA200 = ma200.Series[0].Data.Last().Value;
                    result.RSI14 = rsi14.Series[0].Data.Last().Value;
                    result.Momentum12 = momentum12.Series[0].Data.Last().Value;
                    result.SOFast_k14 = (soFast.Series[0].Data.Any()) ? soFast.Series[0].Data.Last().Value : 0;
                    result.SOFast_d3 = (soFast.Series[1].Data.Any()) ? soFast.Series[1].Data.Last().Value : 0;
                    result.EMA12 = ema12.Series[0].Data.Last().Value;
                    result.EMA26 = ema26.Series[0].Data.Last().Value;
                    result.MACD2612 = macd1226.Series[0].Data.Last().Value;
                    result.MA9 = macd1226.Series[1].Data.Last().Value;
                    result.EMA20 = ema20.Series[0].Data.Last().Value;
                    result.BoolUP = boolinger20.Series[1].Data.Last().Value;
                    result.BoolLOW = boolinger20.Series[2].Data.Last().Value;
                    result.WilliansR = (williansr14.Series[0].Data.Any()) ? williansr14.Series[0].Data.Last().Value : 0;

                    return result;
                }
                return null;
            }

            catch(Exception ex)
            {
                throw ex;
            }            
        }      
    }
}
