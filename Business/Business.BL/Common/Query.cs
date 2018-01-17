using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BL.Common
{
    public class Query
    {
        /// <summary>
        /// Consulta en forma de cadena T-SQL
        /// </summary>
        public string RawQuery { get; set; }
        /// <summary>
        /// Lista de parámetros según nombre en RawQuery
        /// </summary>
        public List<SqlParameter> Parameters { get; set; }
        /// <summary>
        /// Propiedad donde se asignará el resultado de la consulta escalar
        /// </summary>
        public object ResultScalar { get; set; }
        /// <summary>
        /// Propiedad donde se asignará el resultado de la no consulta
        /// </summary>
        public bool ResultNoQuery { get; set; }
        /// <summary>
        /// Propiedad donde se asignará el resultado de la consulta
        /// </summary>
        public DataTable ResultQuery { get; set; }
        /// <summary>
        /// Tipo de consulta CRUD
        /// </summary>
        public TypeCrud Type { get; set; }
        /// <summary>
        /// Determina si la consulta retorna un valor único
        /// </summary>
        public bool IsScalar { get; set; }
        /// <summary>
        /// Determina si la consulta retorna datos de tabla
        /// </summary>
        public bool IsQuery { get; set; }
        /// <summary>
        /// Determina si no es consulta que retorne datos
        /// </summary>
        public bool IsNonQuery { get; set; }
        /// <summary>
        /// Determina la identificación del query
        /// </summary>
        public int IdQuery { get; set; }
        /// <summary>
        /// Determina si el query ya fue ejecutado
        /// </summary>
        public bool IsResolve { get; set; }
    }
}
