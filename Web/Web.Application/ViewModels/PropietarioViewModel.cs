using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Application.ViewModels
{
    public class PropietarioViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage ="Debe seleccionar una persona")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debe seleccionar una persona")]
        public long PersonaId { get; set; }
        public bool Estado { get; set; }
    }
}