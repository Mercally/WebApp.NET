using Data.DAL.Context;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BL.Common
{
    public class Transaction
    {
        /// <summary>
        /// Lista de consultas a ejecutarse en la transacción
        /// </summary>
        public Query[] ListQuery { get; private set; }
        /// <summary>
        /// Determina si la transacción fue guardada
        /// </summary>
        public bool IsCommited { get; private set; }
        /// <summary>
        /// Determina si la transacción fue reversada
        /// </summary>
        public bool IsRollBacked { get; private set; }
        /// <summary>
        /// Determina si la transaccion fue exitosa
        /// </summary>
        public bool IsSuccess { get; private set; }
        /// <summary>
        /// Determina si la transacción fue interrumpida por una consulta con valor inesperado o error ocurrido
        /// </summary>
        public bool IsBroken { get; private set; }
        /// <summary>
        /// Determina cuál Id del query ocacionó la ruptura del código, por defecto es -1
        /// </summary>
        public int BrokenId { get; set; }
        /// <summary>
        /// Usuario del sistema
        /// </summary>
        public string User { get; private set; }

        private Crud Crud { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ListQuery">Lista de consultas a ejecutar</param>
        /// <param name="User">Nombr del usuario que hace la transacción</param>
        public Transaction(string User, params Query[] ListQuery)
        {
            this.ListQuery = ListQuery ?? new Query[0];
            this.User = User;
        }

        private int IdQuery = 0;

        /// <summary>
        /// Ejecuta la transacción actual
        /// </summary>
        public void Execute()
        {
            int BrokenId = -1;
            using (SqlConnection Con = new Connection().Connect())
            {
                using (SqlTransaction Tran = Con.BeginTransaction())
                {
                    try
                    {
                        Crud = new Crud(Con, Tran, this.User);
                        foreach (var Query in ListQuery)
                        {
                            ExecuteQuery(Query);
                            if (IsBroken)
                            {
                                BrokenId = IdQuery;
                                break;
                            }
                        }

                        if (IsBroken)
                        {
                            Tran.Rollback();
                            IsRollBacked = true;
                            IsSuccess = false;
                        }
                        else
                        {
                            Tran.Commit();
                            IsSuccess = IsCommited = true;
                        }
                    }
                    catch (Exception)
                    {
                        Tran.Rollback();
                        IsBroken = IsRollBacked = true;
                        IsSuccess = false;
                        // Exceptions
                    }
                }
            }
        }

        private void ExecuteQuery(Query Query)
        {
            if (Query != null)
            {
                Query.Result = new QueryResult();
                switch (Query.Type)
                {
                    case TypeCrud.Create:
                        Query.Result.ResultScalar = Crud.Create(Query.RawQuery, Query.Parameters);
                        Query.IsScalar = true;
                        IsBroken = long.Parse(Query.Result.ResultScalar.ToString()) < 1;
                        break;
                    case TypeCrud.Delete:
                        Query.Result.ResultNoQuery = Crud.Delete(Query.RawQuery, Query.Parameters);
                        Query.IsNonQuery = true;
                        IsBroken = !Query.Result.ResultNoQuery;
                        break;
                    case TypeCrud.Update:
                        Query.Result.ResultNoQuery = Crud.Update(Query.RawQuery, Query.Parameters);
                        Query.IsNonQuery = true;
                        IsBroken = !Query.Result.ResultNoQuery;
                        break;
                    case TypeCrud.Query:
                        Query.Result.ResultQuery = Crud.Query(Query.RawQuery, Query.Parameters);
                        Query.IsQuery = true;
                        IsBroken = Query.Result.ResultQuery is null;
                        break;
                    default:
                        break;
                }
                Query.IsResolve = true;
                Query.IdQuery = IdQuery;
                if (IsBroken)
                {
                    Query.IsResolve = false;
                    return;
                }
                IdQuery++;
                if (Query.SubQuery != null)
                {
                    foreach (var SubQuery in Query.SubQuery)
                    {
                        ExecuteQuery(SubQuery);
                    }
                }
            }
        }
    }
}
