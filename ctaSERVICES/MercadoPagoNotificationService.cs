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
    public static class MercadoPagoNotificationService
    {
        public static List<MercadoPagoNotificationModel> GetNotifications()
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                List<MercadoPagoNotificationModel> result = entities.MercadoPagoNotifications.Select(n => new MercadoPagoNotificationModel() { id = n.Id, action = n.action, api_version = n.api_version, data_content = n.data_content, data_id = n.data_id, date_created = n.date_created.Value, live_mode = n.live_mode.Value, obj_id = n.obj_id, type = n.type, user_id = n.user_id  }).ToList<MercadoPagoNotificationModel>();

                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
                return result;
            }
        }

        public static void SaveNotification(MercadoPagoNotificationModel model)
        {
            using (ctaDBEntities entities = new ctaDBEntities())
            {
                entities.MercadoPagoNotifications.Add(new MercadoPagoNotification() { action = model.action, api_version = model.api_version, data_content = model.data_content, data_id = model.data_id, date_created = model.date_created, live_mode = model.live_mode, obj_id = model.obj_id, type = model.type, user_id = model.user_id });
                entities.SaveChanges();
                
                if (!(entities.Database.Connection.State == ConnectionState.Closed))
                {
                    entities.Database.Connection.Close();
                }
            }
        }
    }
}
