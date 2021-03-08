using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaWindowsService
{
    internal class Configs_Market
    {
        public string Name { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime NextSync { get; set; }
        public bool QuotesUpdatesActive { get; set; }
        public string WorkHours { get; set; }
        public int UTCOffset { get; set; }
        public string QuoteProviderName { get; set; }

        public static bool QuoteUpdateAvailabe(Configs_Market marketConfig, Func<DateTime,string,bool> isHoliday, DateTime currentDate)
        {
            bool result = false;            

            if(marketConfig.QuotesUpdatesActive)
            {
                if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    result = false;
                }
                else if (marketConfig.NextSync <= currentDate)
                {
                    if (!isHoliday(currentDate, marketConfig.Name))
                    {
                        int startHour = int.Parse(marketConfig.WorkHours.Split(';')[0].Split(':')[0]);
                        int startMinutes = int.Parse(marketConfig.WorkHours.Split(';')[0].Split(':')[1]);
                        int closeHour = int.Parse(marketConfig.WorkHours.Split(';')[1].Split(':')[0]);
                        int closeMinutes = int.Parse(marketConfig.WorkHours.Split(';')[1].Split(':')[1]);

                        if (currentDate.Hour < startHour)
                            result = false;
                        else if (currentDate.Hour == startHour && currentDate.Minute < startMinutes)
                            result = false;
                        else if (currentDate.Hour > closeHour)
                            result = false;
                        else if (currentDate.Hour == closeHour && currentDate.Minute > closeMinutes)
                            result = false;
                        else
                            result = true;
                    }
                }
            }
            return result;
        }
    }
}
