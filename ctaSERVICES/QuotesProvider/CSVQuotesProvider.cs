using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES.QuotesProvider
{
    public class CSVQuotesProvider
    {
        public CSVQuotesProvider()
        { }

        public void ReadData(Stream csvdata, string filename)
        {
            string market_name = Path.GetFileNameWithoutExtension(filename);
            Dictionary<string,int> cache = new Dictionary<string,int>();
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open(); 
                try
                {
                    StreamReader reader = new StreamReader(csvdata);
                    reader.ReadLine();//avoid headers
                    string line = reader.ReadLine();
                    int stock_id = 0; 
                    while(line != null)
                    {
                        try
                        {
                            string[] cols = line.Split(',');
                            if (!(String.IsNullOrEmpty(cols[4]) || String.IsNullOrEmpty(cols[6]) || String.IsNullOrEmpty(cols[7]) || String.IsNullOrEmpty(cols[8])))
                            {
                                string symbol = cols[0].Trim();
                                if(!cache.Keys.Contains(cols[0].Trim()))
                                {
                                    var currentStock = entities.Stocks.Where(s => s.symbol == symbol && s.Market.name == market_name).FirstOrDefault();
                                    if (currentStock != null)
                                    {
                                        cache[cols[0].Trim()] = currentStock.Id;
                                    }
                                    else
                                    {
                                        cache[cols[0].Trim()] = 0;
                                        line = reader.ReadLine();
                                        continue;
                                    }
                                }
                                else if (cache[cols[0].Trim()] == 0)
                                {
                                    line = reader.ReadLine();
                                    continue; 
                                }
                                stock_id = cache[cols[0].Trim()];

                                string[] split_date = cols[1].Split('/');
                                DateTime date = DateTime.Parse(split_date[1]+"/"+split_date[0]+"/"+split_date[2]);
                                double open = double.Parse(cols[6]);
                                double high = double.Parse(cols[7]);
                                double low = double.Parse(cols[8]);
                                double close = double.Parse(cols[4]);
                                decimal volu = (String.IsNullOrEmpty(cols[9])) ? 0 : decimal.Parse(cols[9]);
                                

                                bool delete = false;
                                var sqs = entities.Stock_Quote.Where(s => s.stock_id == stock_id && s.date_round.Year == date.Year && s.date_round.Month == date.Month && s.date_round.Day == date.Day);
                                foreach (Stock_Quote sq1 in sqs)
                                {
                                    entities.Stock_Quote.Remove(sq1);
                                    delete = true;
                                }
                                if (delete)
                                {
                                    entities.SaveChanges();
                                }

                                Stock_Quote sq = new Stock_Quote();
                                sq.stock_id = stock_id;
                                sq.date_round = date;
                                sq.opening = open;                                
                                sq.closing = close;
                                sq.maximun = high;
                                sq.minimun = low;
                                sq.volume = volu;

                                entities.Stock_Quote.Add(sq);
                                entities.SaveChanges();
                            }

                            line = reader.ReadLine();
                        }
                        catch (Exception ex) { }
                    }
                }
                catch (Exception ex) { }
                

                if (entities.Database.Connection.State != System.Data.ConnectionState.Closed)
                    entities.Database.Connection.Close();
            }  
        } 
    }
}
