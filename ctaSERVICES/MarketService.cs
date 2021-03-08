using ctaCOMMON.AdminModel;
using ctaDATAMODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES
{
    public static class MarketService
    {
        public static string GetMarketName(int market_id)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                string result = entities.Markets.Where(m => m.Id == market_id).Select(m => m.name).FirstOrDefault();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
                return result;
            }
        }
        
        public static bool IsMarketOpen(int marketID, DateTime time, out string status)
        {
            bool result = true;
            status = "O";

            if (time.DayOfWeek == DayOfWeek.Saturday || time.DayOfWeek == DayOfWeek.Sunday)
            {
                status = "W";
                return false;
            }

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();                

                var isHoliday = entities.Holidays.Where(h => h.market_id == marketID && h.date.Year == time.Year && h.date.Month == time.Month && h.date.Day == time.Day);

                if (isHoliday.Count() > 0)
                {
                    status = "H";
                    result = false;
                }
                else
                {
                    string workHours = entities.Markets.Where(m => m.Id == marketID).Select(m => m.work_hours).FirstOrDefault();

                    if (workHours != null)
                    {
                        int startHour = int.Parse(workHours.Split(';')[0].Split(':')[0]);
                        int startMinutes = int.Parse(workHours.Split(';')[0].Split(':')[1]);
                        int closeHour = int.Parse(workHours.Split(';')[1].Split(':')[0]);
                        int closeMinutes = int.Parse(workHours.Split(';')[1].Split(':')[1]);

                        status = "C";
                        if (time.Hour < startHour)
                            result = false;
                        else if (time.Hour == startHour && time.Minute < startMinutes)
                            result = false;
                        else if (time.Hour > closeHour)
                            result = false;
                        else if (time.Hour == closeHour && time.Minute > closeMinutes)
                            result = false;
                        else
                            status = "O";
                    }
                }

                entities.Database.Connection.Close();                
            }            
            return result;
        }

        public static bool IsMarketOpen(int marketId)
        {
            bool result = true;
            DateTime time = MarketService.GetMarketCurrentLocalTime(marketId);

            if (time.DayOfWeek == DayOfWeek.Saturday || time.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.Database.Connection.Open();

                var isHoliday = entities.Holidays.Where(h => h.market_id == marketId && h.date.Year == time.Year && h.date.Month == time.Month && h.date.Day == time.Day);

                if (isHoliday.Count() > 0)
                {
                    result = false;
                }
                else
                {
                    string workHours = entities.Markets.Where(m => m.Id == marketId).Select(m => m.work_hours).FirstOrDefault();

                    if (workHours != null)
                    {
                        int startHour = int.Parse(workHours.Split(';')[0].Split(':')[0]);
                        int startMinutes = int.Parse(workHours.Split(';')[0].Split(':')[1]);
                        int closeHour = int.Parse(workHours.Split(';')[1].Split(':')[0]);
                        int closeMinutes = int.Parse(workHours.Split(';')[1].Split(':')[1]);

                        if (time.Hour < startHour)
                            result = false;
                        else if (time.Hour == startHour && time.Minute < startMinutes)
                            result = false;
                        else if (time.Hour > closeHour)
                            result = false;
                        else if (time.Hour == closeHour && time.Minute > closeMinutes)
                            result = false;                        
                    }
                }

                entities.Database.Connection.Close();
            }
            return result;
        }        

        public static List<MarketModel> GetMarkets()
        {
            List<MarketModel> result = new List<MarketModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Markets.Select(s => new MarketModel() { Id = s.Id, name = s.name, work_hours = s.work_hours }).ToList();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static void CreateMarket(MarketModel marketModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Market mk = new Market() { name = marketModel.name, work_hours = marketModel.work_hours };
                entities.Markets.Add(mk);
                entities.SaveChanges();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }            
        }

        public static void UpdateMarket(MarketModel marketModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Market mk = entities.Markets.Where(m => m.Id == marketModel.Id).FirstOrDefault();

                if(mk != null)
                {
                    mk.name = marketModel.name;
                    mk.work_hours = marketModel.work_hours;
                    
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }   
        }

        public static void DeleteMarket(int marketID)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Market mk = entities.Markets.Where(m => m.Id == marketID).FirstOrDefault();

                if (mk != null)
                {
                    entities.Markets.Remove(mk);                    
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }  
        }

        public static DateTime GetMarketCurrentLocalTime(string marketName)
        {
            int utcoffset = MarketService.GetMarkets().Where(m => m.name == marketName).First().utc_offset;
            return DateTime.UtcNow.AddHours(utcoffset);
        }

        public static DateTime GetMarketCurrentLocalTime(int marketId)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                int utcoffset = entities.Markets.Where(m => m.Id == marketId).First().utc_offset;
                return DateTime.UtcNow.AddHours(utcoffset);
            }
        }

        public static DateTime GetMarketLocalTime(int marketId, DateTime dateToConvert)
        {
            using(ctaDBEntities entities = new ctaDBEntities())
            {
                Market market = entities.Markets.Where(m => m.Id == marketId).FirstOrDefault();
                if(market != null)
                {
                    return dateToConvert.ToUniversalTime().AddHours(market.utc_offset);
                }
                return dateToConvert;
            }
        }

        public static string GetYahooID(int marketId)
        {
            if(marketId == 1)
                return ".BA";
            else
                return "";
        }
    }
}
