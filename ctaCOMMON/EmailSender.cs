using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ctaCOMMON
{
    public static class EmailSender
    {
        //Email service configuration
        public static string SMTP_SERVER = "mail.viciouss.com";
        public static string SMTP_USER = "postmaster@viciouss.com";
        public static string SMTP_PASSWORD = "abc123$$$";
        public static int SMTP_PORT = 25;
        public static bool SMTP_SSL = false;



        public static void SendMarketCloseEmail(string market_name, DateTime date, string status)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("postmaster@viciouss.com");

            //set the content 
            mail.Subject = "From VicioUSS";
            mail.Body = market_name + " is closed now. Time:" + date + " Status: " + status;            
            
            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendErrorGettingSymbols(string message, string market_name, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("postmaster@viciouss.com");

            //set the content 
            mail.Subject = "VicioUSS --> ErrorGettingSymbols --> " + date;
            mail.Body = market_name + " " + message;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendErrorGettingSymbolsStocks(string message, string market_name, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("postmaster@viciouss.com");
            mail.CC.Add("rfernandez@viciouss.com");

            //set the content 
            mail.Subject = "VicioUSS --> ErrorGettingSymbolsStocks --> " + date;
            mail.Body = market_name + " " + message;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendErrorSavingChangesInDataBase(string message, string market_name, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("postmaster@viciouss.com");

            //set the content 
            mail.Subject = "VicioUSS --> ErrorSavingChangesInDataBase --> " + date;
            mail.Body = market_name + " " + message;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendReportUpdated(DateTime date, string message)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("rfernandez@viciouss.com");

            //set the content 
            mail.Subject = "From VicioUSS";
            mail.Body = "Report Updated. Time:" + date + " Message: " + message;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendIntradiarySaved(string market_name, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("postmaster@viciouss.com");
            //mail.CC.Add("rfernandez@viciouss.com");

            //set the content 
            mail.Subject = "From VicioUSS";
            mail.Body = market_name + " IntradiarySaved. Time:" + date;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendErrorUpdatingReport(string message, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("rfernandez@viciouss.com");

            //set the content 
            mail.Subject = "VicioUSS --> ErrorUpdatingReport --> " + date;
            mail.Body = message;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendNoDataForReportStock(string symbol, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("rfernandez@viciouss.com");

            //set the content 
            mail.Subject = "VicioUSS --> ErrorUpdatingReportData --> No data for stock: " + symbol + "for date: " + date;
            mail.Body = "There was no data to generate report for the stock: " + symbol + "for date: " + date;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static string SendForgotPSWMail(string email, string secretPass, string user)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com"); //welcome@viciouss.com
            mail.To.Add(email);

            //set the content 
            mail.Subject = "Olvido de contraseña de Viciouss";
            mail.Body = "Sus credenciales para el sitio viciouss son: \n" +
                        "User: " + user + "\n" +
                        "Contraseña: " + secretPass + "\n";

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }

            return "Success";
        }

        public static void SendErrorUpdatingReportData(string message, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("rfernandez@viciouss.com");

            //set the content 
            mail.Subject = "VicioUSS --> ErrorUpdatingReportData --> " + date;
            mail.Body = message;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendErrorUpdatingHistorical(string message, string market_name, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("postmaster@viciouss.com");
            mail.CC.Add("rfernandez@viciouss.com");

            //set the content 
            mail.Subject = "VicioUSS --> ErrorUpdatingHistorical --> " + date;
            mail.Body = market_name + " " + message;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendErrorUpdatingIntradiary(string message, string market_name, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("postmaster@viciouss.com");
            mail.CC.Add("rfernandez@viciouss.com");

            //set the content 
            mail.Subject = "VicioUSS --> ErrorUpdatingIntradiary --> " + date;
            mail.Body = market_name + " " + message;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendHistoricalUpdated(string market_name, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("postmaster@viciouss.com");

            //set the content 
            mail.Subject = "From VicioUSS";
            mail.Body = market_name + " HistoricalUpdated. Time:" + date;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendErrorClearIntradiaryData(string message, string market_name, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("postmaster@viciouss.com");

            //set the content 
            mail.Subject = "VicioUSS --> ErrorClearIntradiaryData --> " + date;
            mail.Body = market_name + " " + message;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void DeletingIntradiaryData(string message, string market_name, DateTime date)
        {
            //create the mail message 
            MailMessage mail = new MailMessage();

            //set the addresses 
            mail.From = new MailAddress("postmaster@viciouss.com");
            mail.To.Add("postmaster@viciouss.com");

            //set the content 
            mail.Subject = "VicioUSS --> DeletingIntradiaryData --> " + date;
            mail.Body = market_name + " " + message;

            //send the message 
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }
        }

        public static void SendConfirmationMail(string email, string secretCode, string imgPath)
        {
            //create the mail message 
            MailMessage mail = GetMailWithImage(email, secretCode, imgPath);//new MailMessage();
            SmtpClient smtp = new SmtpClient("mail.viciouss.com");

            NetworkCredential Credentials = new NetworkCredential("postmaster@viciouss.com", "abc123$$$");
            smtp.Credentials = Credentials;

            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) { }

            //mail.Body = "Welcome to VICIOUSS!! \n\n" +
            //            "Gracias por registrarse en Viciouss. Su cuenta esta creada pero debe ser activada antes de que pueda usarla. \n\n" +
            //            "Para activar su cuenta seleccione el siguiente link o cópielo y péguelo en su browser: \n\n" +
            //            baseUrl + "User/DoActivate?token=" + secretCode + " \n\n" +
            //            "Con su cuenta activada usted va a poder iniciar sesión en " + baseUrl + " usando las credenciales que utilizó al registrarse.";
        }

        private static MailMessage GetMailWithImage(string email, string secretCode, string imgPath)
        {
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;

            //get embedded image
            LinkedResource image = new LinkedResource(imgPath + "mail_bienvenida.jpg");
            LinkedResource facebook = new LinkedResource(imgPath + "/icon/facebook_003.png");
            LinkedResource linkedIn = new LinkedResource(imgPath + "/icon/linkedin_003.png");
            LinkedResource instagram = new LinkedResource(imgPath + "/icon/instagram_003.png");
            LinkedResource twitter = new LinkedResource(imgPath + "/icon/twitter_003.png");

            string baseUrl = string.Format("{0}{1}/", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
            string htmlBody = "<html>" +
                                "<body style='font-size:20px;'>" +
                                    "<h3 style='text-align:center;color:rgb(20, 112, 99)'> Welcome to Viciouss</h3>" +
                                    "<p style='color:rgb(20, 112, 99)'> Gracias por registrarse . Para comenzar a disfrutar de esta nueva expericencia para sus finanzas, " +
                                    "haga click en el siguiente link para activar su cuenta:</p>" +
                                    "<p style='color:rgb(20, 112, 99)'>" + baseUrl + "User/DoActivate?token=" + secretCode + "</p>" +
                                    "<p style='color:rgb(20, 112, 99)'> A partir de allí podrá crear Watchlists de activos, recibir informes y alertas y" +
                                    "acceder a contenido de calidad garantizada, mejorando sus técnicas de trading!</p>" +
                                    "<p style='color:rgb(20, 112, 99)'> Siguenos en las redes sociales </p>" +
                                    "<a style='padding-left:10px;text-decoration:none;' href='https://www.facebook.com/Viciouss-139562596717053' target='_blank'>" +
                                        "<img src='cid:" + facebook.ContentId + @"' /> " +
                                    "</a>" +
                                    "<a style='padding-left:10px;text-decoration:none;' href='https://www.linkedin.com/company/viciouss' target='_blank'>" +
                                        "<img src='cid:" + linkedIn.ContentId + @"' /> " +
                                    "</a>" +
                                    "<a style='padding-left:10px;text-decoration:none;' href='https://twitter.com/Viciouss_mkt' target='_blank'>" +
                                        "<img src='cid:" + twitter.ContentId + @"' /> " +
                                    "</a>" +
                                    "<a style='padding-left:10px;text-decoration:none;' href='https://www.instagram.com/vicioussok/?hl=es-la' target='_blank'>" +
                                        "<img src='cid:" + instagram.ContentId + @"' /> " +
                                    "</a>" +
                                    "<p style='color:rgb(20, 112, 99);padding-top:15px;'>PD: Ten SUERTE con Viciouss.</p>" +
                                    "<img style='width:85%' src='cid:" + image.ContentId + @"'/>" +
                                "</body>" +
                            "</html> ";

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

            avHtml.LinkedResources.Add(image);
            avHtml.LinkedResources.Add(facebook);
            avHtml.LinkedResources.Add(linkedIn);
            avHtml.LinkedResources.Add(twitter);
            avHtml.LinkedResources.Add(instagram);

            mail.AlternateViews.Add(avHtml);

            //set the addresses 
            mail.From = new MailAddress("welcome@viciouss.com");
            mail.To.Add(email);
            mail.Subject = "Link de Activación de Cuenta de Viciouss";

            return mail;
        }

        /*
         * Generic email sender
         */
        public static void SendHTMLEmail(string to, string replyTo, string subject, string message)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(SMTP_USER);
            mail.To.Add(to);
            mail.ReplyToList.Add(new MailAddress(replyTo, "reply-to"));
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = message;

            SmtpClient smtp = new SmtpClient(SMTP_SERVER, SMTP_PORT);
            NetworkCredential Credentials = new NetworkCredential(SMTP_USER, SMTP_PASSWORD);
            smtp.Credentials = Credentials;
            smtp.EnableSsl = SMTP_SSL;

            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex) {
                //TODO Detect failure, log it or something
                throw ex;
            }
        }
    }
}
