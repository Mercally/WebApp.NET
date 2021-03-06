﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Application.ViewModels
{
    public class SearchPersona
    {
        [Required]
        [MinLength(5, ErrorMessage = "Ingrese un filtro con más de 5 caracteres")]
        [MaxLength(100)]
        public string Filtro { get; set; }
    }
}