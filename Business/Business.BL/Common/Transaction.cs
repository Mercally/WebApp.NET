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
        public List<Query> ListQuery { get; private set; }
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
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ListQuery">Lista de consultas a ejecutar</param>
        /// <param name="User">Nombr del usuario que hace la transacción</param>
        public Transaction(List<Query> ListQuery, string User)
        {
            this.ListQuery = ListQuery ?? new List<Query>();
            this.User = User;
        }

        /// <summary>
        /// Ejecuta la transacción actual
        /// </summary>
        public void Execute()
        {
            int Index = 0; BrokenId = -1;
            using (SqlConnection Con = new Connection().Connect())
            {
                using (SqlTransaction Tran = Con.BeginTransaction())
                {
                    try
                    {
                        Crud Crud = new Crud(Con, Tran, User);
                        foreach (var Query in ListQuery)
                        {
                            ExecuteQuery(Query, Crud);

                            Query.IdQuery = Index;
                            if (IsBroken)
                            {
                                BrokenId = Index;
                                break;
                            }
                            Index++;
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
                    catch (Exception ex)
                    {
                        Tran.Rollback();
                        IsBroken = IsRollBacked = true;
                        IsSuccess = false;
                        // Exceptions
                    }
                }
            }
        }

        private void ExecuteQuery(Query Query, Crud Crud)
        {
            switch (Query.Type)
            {
                case TypeCrud.Create:
                    Query.ResultScalar = Crud.Create(Query.RawQuery, Query.Parameters);
                    Query.IsScalar = true;
                    IsBroken = long.Parse(Query.ResultScalar.ToString()) < 1;
                    break;
                case TypeCrud.Delete:
                    Query.ResultNoQuery = Crud.Delete(Query.RawQuery, Query.Parameters);
                    Query.IsNonQuery = true;
                    IsBroken = !Query.ResultNoQuery;
                    break;
                case TypeCrud.Update:
                    Query.ResultNoQuery = Crud.Update(Query.RawQuery, Query.Parameters);
                    Query.IsNonQuery = true;
                    IsBroken = !Query.ResultNoQuery;
                    break;
                case TypeCrud.Query:
                    Query.ResultQuery = Crud.Query(Query.RawQuery, Query.Parameters);
                    Query.IsQuery = true;
                    IsBroken = Query.ResultQuery is null;
                    break;
                default:
                    break;
            }
            Query.IsResolve = true;
        }

        public void ExecuteQuery(Query Query)
        {
            using (SqlConnection Con = new Connection().Connect())
            {
                using (SqlTransaction Tran = Con.BeginTransaction())
                {
                    Crud Crud = new Crud(Con, Tran, User);
                    ExecuteQuery(Query, Crud);
                }
            }
        }
    }
}
