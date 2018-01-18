using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Web.Custom.DataNotation;

namespace Common.Entities
{
    public class Persona
    {
        public long Id { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Necesitamos nombres reales")]
        
        public string Nombre { get; set; }

        [DisplayName("Apellido")]
        [PlaceHolder("Apellido")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Necesitamos nombres reales")]
        public string Apellido { get; set; }

        [DisplayName("Edad")]
        [DefaultValue(18)]
        [Required(ErrorMessage = "Este campo es requerido")]
        [Range(18, 99, ErrorMessage = "Solo mayores de edad, pero no tan veteranos")]
        public short Edad { get; set; }

        [DisplayName("Correo")]
        [PlaceHolder("user@domain.com")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Debes ingresar una dirección de correo válida")]
        [RegularExpression("^(([^<>()\\[\\]\\\\.,;:\\s@\"]+(\\.[^<>()\\[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$", ErrorMessage = "Ingrese una dirección de correo real")]
        //string RegExp = ^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$
        [Remote("CorreoVal", "Persona", ErrorMessage = "El correo ingresado no es válido")]
        public string Correo { get; set; }

        [DisplayName("Estado")]
        [Required(ErrorMessage = "Este campo es requerido")]        
        public bool Estado { get; set; }

        public IEnumerable<Propietario> Propietario { get; set; }
    }
}
