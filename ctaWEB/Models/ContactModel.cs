using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ctaWEB.Models
{
    public class ContactModel
    {
        public ContactModel() { }

        [Required(ErrorMessage = "Nombre requerido")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Email es requerido")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        //Fixed email regular expression
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$", ErrorMessage = "Formato de email incorrecto")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Teléfono es requerido")]
        //[StringLength(10)]
        //[DataType(DataType.PhoneNumber)]
        ////[RegularExpression("^[0-9]{10}$", ErrorMessage = "Formato de email incorrecto")]
        //[Display(Name = "Teléfono")]
        //public string Telefono { get; set; }

        [Required(ErrorMessage = "El mensaje es requerido")]
        [StringLength(500)]
        [DataType(DataType.Text)]
        [Display(Name = "Mensaje")]
        public string Mensaje { get; set; }
    }
}