using ctaCOMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctaSERVICES
{
    public class ContactService
    {
        public static string CTA_CONTACT_EMAIL = "contact@viciouss.com";
        public static string CTA_CONTACT_SUBJECT = "[Viciouss] Contacto desde site web";

        /*
         * Saves contact data and send email to site admin
         */
        public static void DoContact(string nombre, string email, string telefono, string mensaje)
        {
            //@TODO [Optional] Save contact to db

            mensaje = "Nombre: " + nombre + "<br />" +
                "Telefono: " + telefono + "<br />" +
                "Email: " + email + "<br />" +
                "Mensaje: <br />" + mensaje;
            EmailSender.SendHTMLEmail(CTA_CONTACT_EMAIL, email, CTA_CONTACT_SUBJECT, mensaje);
        }
    }
}
