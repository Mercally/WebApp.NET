using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BL.Common
{
    public class QueryResult
    {
        /// <summary>
        /// Resultado de la consulta escalar
        /// </summary>
        public object ResultScalar { get; set; }
        /// <summary>
        /// El resultado de la no consulta non-query
        /// </summary>
        public bool ResultNoQuery { get; set; }
        /// <summary>
        /// Resultado de la consulta en su forma DataTable
        /// </summary>
        public DataTable ResultQuery { get; set; }
    }
}
