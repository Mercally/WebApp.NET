using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BL.Common
{
    public static class TransactionExtensions
    {
        /// <summary>
        /// Obtiene una consulta resuelta o no de la transacción
        /// </summary>
        /// <param name="Trans"></param>
        /// <param name="IdQuery">Identificador del query a extraer</param>
        /// <returns></returns>
        public static Query GetQuery(this Transaction Trans, int IdQuery)
        {
            Query query = Trans.ListQuery.FirstOrDefault(x => x.IdQuery == IdQuery);
            
            return query;
        }

        /// <summary>
        /// Obtiene una consulta resuelta o no de la transacción
        /// </summary>
        /// <param name="Trans"></param>
        /// <param name="IdQuery">Identificador del query a extraer</param>
        /// <returns></returns>
        public static Query GetQueryResult(this Transaction Trans, int IdQuery)
        {
            Query query = Trans.ListQuery.FirstOrDefault(x => x.IdQuery == IdQuery);
            Trans.ExecuteQuery(query);

            return query;
        }
    }
}
