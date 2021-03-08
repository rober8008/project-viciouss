using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES.Reporting
{
    public class ReportData_Retriever
    {
        public List<Stock_Report> GetReportData(DateTime date)
        {
            List<Stock_Report> result;

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Stock_Report.Where(x => x.Date_Round == date)
                                              .Include(x => x.Stock)
                                              .Include(x => x.Stock.Stock_Type)
                                              .Include(x => x.Stock.MarketIndex_Stock)
                                              .OrderBy(x => x.Stock.symbol)
                                              .ToList();

                entities.Database.Connection.Close();
            }

            return result;
        }

        private List<Stock_Report> GetAllReportData()
        {
            var data = this.GetReportData(DateTime.Now.Date);

            for (int days = 1; !data.Any(); days++)
            {
                data = this.GetReportData(DateTime.Now.AddDays(-days).Date);
            }

            return data;
        }

        public (IEnumerable<Stock_Report> Indices, IEnumerable<Stock_Report> ADRs, IEnumerable<Stock_Report> BYMAs, IEnumerable<Stock_Report> Bonos, IEnumerable<Stock_Report> CEDEARs) 
            GetReportData()
        {
            var allData = this.GetAllReportData();

            return
            (
                Indices: allData.Where(x => x.Stock.type_id == 4),
                ADRs: allData.Where(x => x.Stock.MarketIndex_Stock.Any(y => y.marketindex_id == 5)),
                BYMAs: allData.Where(x => x.Stock.type_id == 1 && x.Stock.market_id == 1 && !x.Stock.MarketIndex_Stock.Any(y => y.marketindex_id == 6)),
                Bonos: allData.Where(x => x.Stock.type_id == 2),
                CEDEARs: allData.Where(x => x.Stock.type_id == 1 && x.Stock.market_id == 1 && x.Stock.MarketIndex_Stock.Any(y => y.marketindex_id == 6))
            );
        }
    }
}
