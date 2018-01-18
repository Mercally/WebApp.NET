using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Propietario
    {
        public long Id { get; set; }
        public long PersonaId { get; set; }
        public bool Estado { get; set; }

        public Persona Persona { get; set; }
    }
}
