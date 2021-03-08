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
    public static class ReportTypeService
    {
        public static List<ReportTypeModel> GetReportTypes()
        {
            List<ReportTypeModel> result = new List<ReportTypeModel>();

            using (ctaDBEntities entities = new ctaDBEntities())
            {
                result = entities.Report_Type.Select(s => new ReportTypeModel() { Id = s.Id, name = s.name, active = s.active }).ToList();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }

            return result;
        }

        public static void CreateReportType(ref ReportTypeModel reportTypeModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Report_Type st = new Report_Type() { name = reportTypeModel.name, active = reportTypeModel.active };
                entities.Report_Type.Add(st);
                entities.SaveChanges();

                reportTypeModel.Id = st.Id;

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }            
        }

        public static void UpdateReportType(ReportTypeModel reportTypeModel)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Report_Type st = entities.Report_Type.Where(s => s.Id == reportTypeModel.Id).FirstOrDefault();
                if (st != null)
                {
                    st.name = reportTypeModel.name;
                    st.active = reportTypeModel.active;

                    entities.SaveChanges();
                }

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }

        public static void DeleteReportType(int reporttypeID)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                Report_Type st = entities.Report_Type.Where(s => s.Id == reporttypeID).FirstOrDefault();
                if (st != null)
                {
                    entities.Report_Type.Remove(st);
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
