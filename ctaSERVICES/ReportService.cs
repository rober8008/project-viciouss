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
    public static class ReportService
    {
        public static List<ReportModel> GetReports()
        {
            List<ReportModel> result = new List<ReportModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Reports.Select(t => new ReportModel() { Id = t.Id, active = t.active, description = t.description, title = t.title, type = t.type, url = t.url }).ToList();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static void CreateReport(ref ReportModel reportModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Report report = new Report() { Id = reportModel.Id, active = reportModel.active, description = reportModel.description, title = reportModel.title, type = reportModel.type, url = reportModel.url };

                entities.Reports.Add(report);
                entities.SaveChanges();
                reportModel.Id = report.Id;

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void UpdateReport(ReportModel reportModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Report report = entities.Reports.Where(s => s.Id == reportModel.Id).FirstOrDefault();
                if (report != null)
                {
                    report.Id = reportModel.Id;
                    report.active = reportModel.active;
                    report.description = reportModel.description; 
                    report.title = reportModel.title; 
                    report.type = reportModel.type; 
                    report.url = reportModel.url;

                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }
    }
}
