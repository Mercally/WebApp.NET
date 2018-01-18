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
        /// Tipo de consulta CRUD
        /// </summary>
        public TypeCrud Type { get; set; }
        /// <summary>
        /// Determina la identificación del query
        /// </summary>
        public int IdQuery { get; set; }
        /// <summary>
        /// Determina si el query ya fue ejecutado, una vez Result esté inicializado
        /// </summary>
        public bool IsResolve { get; set; }
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
        /// Resultado del query
        /// </summary>
        public QueryResult Result { get; set; }


        /// <summary>
        /// Lista de sub querys de la consulta principal
        /// </summary>
        public Query[] SubQuery { get; set; }
        /// <summary>
        /// Tablas incluidas en la consulta
        /// </summary>
        public string[] Includes { get; set; }
        /// <summary>
        /// Nombre de la tabla que hace inclución
        /// </summary>
        public string NameInclude { get; set; }

        public Query()
        {
            this.SubQuery = new Query[0];
            this.Includes = new string[0];
            this.Parameters = new List<SqlParameter>();
        }

        /// <summary>
        /// Busca las coincidencias en la lista proporcionada
        /// </summary>
        /// <param name="pListQuery">Lista de Query donde se buscará</param>
        /// <param name="NameInclude">Busqueda por nombre de tabla incluida</param>
        /// <param name="IdQuery">Id del query dentro de la lista</param>
        /// <returns>Lista de Querys coincidentes</returns>
        public static List<Query> FindAll(Query[] pListQuery, string NameInclude = null, int? IdQuery = null)
        {
            SearchNameInclude.Clear();
            SearchIdQuery.Clear();
            List<Query> Query = new List<Common.Query>();
            var ListQuery = pListQuery.ToList();
            try
            {
                if (!string.IsNullOrEmpty(NameInclude))
                {
                    Query = FindRecursive(ListQuery, NameInclude);
                }
                else if (IdQuery.HasValue)
                {
                    Query = FindRecursive(ListQuery, IdQuery.Value);
                }
                else
                {
                    Query = new List<Common.Query>();
                }
            }
            catch (Exception)
            {
                // Exceptios
            }
            return Query;
        }

        /// <summary>
        /// Busca un Query en la lista proporcionada
        /// </summary>
        /// <param name="pListQuery">Lista de Query donde se buscará</param>
        /// <param name="IdQuery">Id del query dentro de la lista</param>
        /// <returns>Lista de Querys coincidentes</returns>
        public static Query FindFirst(Query[] pListQuery, string NameInclude)
        {
            SearchNameInclude.Clear();
            SearchIdQuery.Clear();
            List<Query> Query = new List<Common.Query>();
            var ListQuery = pListQuery.ToList();
            try
            {
                if (!string.IsNullOrEmpty(NameInclude))
                {
                    Query = FindRecursive(ListQuery, NameInclude);
                }
                else
                {
                    Query = new List<Common.Query>();
                }
            }
            catch (Exception)
            {
                // Exceptios
            }
            return Query.First();
        }

        /// <summary>
        /// Busca un Query en la lista proporcionada
        /// </summary>
        /// <param name="pListQuery">Lista de Query donde se buscará</param>
        /// <param name="NameInclude">Busqueda por nombre de tabla incluida</param>
        /// <param name="IdQuery">Id del query dentro de la lista</param>
        /// <returns>Lista de Querys coincidentes</returns>
        public static Query FindFirst(Query[] pListQuery, int IdQuery)
        {
            SearchNameInclude.Clear();
            SearchIdQuery.Clear();
            List<Query> Query = new List<Common.Query>();
            var ListQuery = pListQuery.ToList();
            try
            {
                Query = FindRecursive(ListQuery, IdQuery);
            }
            catch (Exception)
            {
                // Exceptios
            }
            return Query.First();
        }

        static List<Query> SearchNameInclude = new List<Query>();
        private static List<Query> FindRecursive(List<Query> ListQuery, string NameInclude)
        {
            if (ListQuery.Count() > 0)
            {
                SearchNameInclude.AddRange(
                    ListQuery.Where(x => x.NameInclude == NameInclude)
                    );

                foreach (var item in ListQuery)
                {
                    if (item.SubQuery.Count() > 0)
                    {
                        FindRecursive(item.SubQuery.ToList(), NameInclude);
                    }
                }
            }
            return SearchNameInclude;
        }

        static List<Query> SearchIdQuery = new List<Query>();
        private static List<Query> FindRecursive(List<Query> ListQuery, int IdQuery)
        {
            if (ListQuery.Count() > 0)
            {
                SearchIdQuery.AddRange(
                    ListQuery.Where(x => x.IdQuery == IdQuery)
                    );

                foreach (var item in ListQuery)
                {
                    if (item.SubQuery.Count() > 0)
                    {
                        FindRecursive(item.SubQuery.ToList(), IdQuery);
                    }
                }
            }
            return SearchIdQuery;
        }
    }
}
