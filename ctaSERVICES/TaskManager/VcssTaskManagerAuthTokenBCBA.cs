using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ctaCOMMON;
using ctaCOMMON.AdminModel;
using Newtonsoft.Json;

namespace ctaSERVICES.TaskManager
{
    public class VcssTaskManagerAuthTokenBCBA : VcssTaskManager
    {
        protected override VcssTaskCanExecuteResult CanExecute(VcssTaskModel vcssTask)
        {
            return VcssTaskCanExecuteResult.Execute;
        }

        protected override VcssTaskCanScheduleResult CanSchedule(VcssTaskInfoModel vcssTaskInfo)
        {   
            DateTime marketTime = DateTime.UtcNow.AddHours(vcssTaskInfo.Market.utc_offset);
            DateTime lastScheduled = VcssTaskManagerService.GetLastVcssTaskScheduledTime(vcssTaskInfo).AddMinutes(vcssTaskInfo.step);
            if (marketTime >= lastScheduled)
            {
                return VcssTaskCanScheduleResult.Schedule;                    
            }
            return VcssTaskCanScheduleResult.InvalidTime;           
        }

        protected override void ExecuteTask(VcssTaskModel vcssTask)
        {            
            string username = vcssTask.VcssTaskInfo.auth_token.Split('-')[0];
            string password = vcssTask.VcssTaskInfo.auth_token.Split('-')[1];
            string requestURL = String.Format(vcssTask.VcssTaskInfo.url, username, ComputeHash(password));

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestURL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    var strResponse = reader.ReadToEnd();
                    dynamic result = JsonConvert.DeserializeObject<dynamic>(strResponse);
                    if(result.Estado.ToString() != "Error")
                    {
                        VcssTaskManagerService.UpdateBOLSARAuthToken(result.Resultado.ToString());
                    }
                    else
                    {
                        VcssTaskManagerService.Log(vcssTask.VcssTaskInfo.name, vcssTask.data, vcssTask.scheduled_time, "Running", strResponse, DateTime.Now);
                    }
                }
            }            
        }

        protected override void ScheduleTask(VcssTaskInfoModel vcssTaskInfo, params string[] parameters)
        {            
            DateTime marketTime = DateTime.UtcNow.AddHours(vcssTaskInfo.Market.utc_offset);
            VcssTaskManagerService.ScheduleTask(vcssTaskInfo.Id, "", marketTime);                        
        }

        private string ComputeHash(string plainText)
        {
            Encoding en = Encoding.GetEncoding(1252);
            // Get the md5 provider 
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
            // Compute the local hash 
            Byte[] md5HashLocal = md5Provider.ComputeHash(en.GetBytes(plainText.Trim()));
            StringBuilder sbHexOutput = new StringBuilder("");
            foreach (Byte _eachChar in md5HashLocal)
            {
                sbHexOutput.AppendFormat("{0:X2}", _eachChar);
            }

            //return sbHexOutput.ToString();
            return "a1c71cae329c7e7ea7aecdeea6474ee8";
        }
    }
}
