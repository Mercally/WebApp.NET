using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities.Cat;
using Common.Entities.Neg;
using Common.Entities.Seg;

namespace Common.Entities.Com
{
    public class Boleta
    {
        public int Id { get; set; }
        public string NumeroBoleta { get; set; }
        public DateTime FechaEntera { get; set; }
        public decimal HoraEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public decimal HoraSalida { get; set; }
        public decimal TiempoEfectivo { get; set; }
        public int TiempoInvertidoEn { get; set; }
        public int ProyectoId { get; set; }
        public int ClienteId { get; set; }
        public int UsuarioId { get; set; }
        public int DepartamentoId { get; set; }
        public bool EsActivo { get; set; }

        public Catalogo TiempoInvertido { get; set; }
        public Proyecto Proyecto { get; set; }
        public Cliente Cliente { get; set; }
        public Usuario Usuario { get; set; }
        public Catalogo Departamento { get; set; }
        public List<Visita> Visitas { get; set; }
    }
}
