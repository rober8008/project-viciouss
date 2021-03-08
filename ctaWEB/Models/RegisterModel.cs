using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ctaWEB.Models
{
    public class RegisterModel
    {
        public RegisterModel() { }

        [Required(ErrorMessage = "Nombre de Usuario es Requerido")]
        [DataType(DataType.Text)]
        //[System.Web.Mvc.Remote("VerifyNewUserName","Account")]
        public string username { get; set; }

        [Required(ErrorMessage = "Email es Requerido")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Formato de email incorrecto")]
        public string email { get; set; }

        [Required(ErrorMessage = "Contraseña es Requerida")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Confime su Contraseña")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Contraseñas no coinciden")]
        public string repeat_password { get; set; }        
    }
}