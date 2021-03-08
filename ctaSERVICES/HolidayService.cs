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
    public static class HolidayService
    {
        public static List<HolidayModel> GetHolidays()
        {
            List<HolidayModel> result = new List<HolidayModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Holidays.OrderBy(x => x.market_id).OrderBy(y => y.date).Select(s => new HolidayModel() { date = s.date, duration = s.duration, market_id = s.market_id, Id = s.Id, market_name = s.Market.name }).ToList();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static void CreateHoliday(HolidayModel holidayModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Holiday hol = new Holiday() { date = holidayModel.date, duration = holidayModel.duration, market_id = holidayModel.market_id };
                entities.Holidays.Add(hol);
                entities.SaveChanges();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void UpdateHoliday(HolidayModel holidayModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Holiday hol = entities.Holidays.Where(s => s.Id == holidayModel.Id).FirstOrDefault();
                if (hol != null)
                {
                    hol.duration = holidayModel.duration;
                    hol.date = holidayModel.date;
                    hol.market_id = holidayModel.market_id;
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void DeleteHoliday(HolidayModel holidayModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Holiday hol = entities.Holidays.Where(s => s.Id == holidayModel.Id).FirstOrDefault();
                if (hol != null)
                {
                    entities.Holidays.Remove(hol);
                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static bool IsHoliday(int marketId, DateTime date)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Holiday hol = entities.Holidays.Where(s => s.market_id == marketId && s.date.Year == date.Year && s.date.Month == date.Month && s.date.Day == date.Day).FirstOrDefault();
                return (hol != null);                
            }
        }
    }
}
