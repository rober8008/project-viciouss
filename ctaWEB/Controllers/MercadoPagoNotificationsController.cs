using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json;
using ctaCOMMON.AdminModel;
using ctaSERVICES;

namespace ctaWEB.Controllers
{
    [AllowAnonymous]
    public class MercadoPagoNotificationsController : Controller
    {
        // GET: MercadoPagoNotifications
        [HttpPost]
        public ActionResult Notification(MercadoPagoNotification not)
        {
            string data_id = Request.QueryString["data.id"];            
            if (not.data != null) data_id = not.data.id;

            DateTime date_created = DateTime.Now;
            DateTime.TryParse(not.date_created, out date_created);   
            
            bool live_mode = false;
            bool.TryParse(not.live_mode, out live_mode);

            MercadoPagoNotificationModel model = new MercadoPagoNotificationModel() { action = not.action, api_version = not.api_version, data_id = data_id, date_created = date_created.Date, obj_id = not.id, live_mode = live_mode, type = not.type, user_id = not.user_id, data_content = JsonConvert.SerializeObject(not) };

            MercadoPagoNotificationService.SaveNotification(model);

            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }
    }

    public class MercadoPagoNotification
    {
        [JsonProperty]
        public string id { get; set; }
        [JsonProperty]
        public string live_mode { get; set; }
        [JsonProperty]
        public string type { get; set; }
        [JsonProperty]
        public string date_created { get; set; }
        [JsonProperty]
        public string user_id { get; set; }
        [JsonProperty]
        public string api_version { get; set; }
        [JsonProperty]
        public string action { get; set; }
        [JsonProperty]
        public MercadoPagoNotificationData data { get; set; }
    }

    public class MercadoPagoNotificationData
    {
        [JsonProperty]
        public string id { get; set; }
    }
}