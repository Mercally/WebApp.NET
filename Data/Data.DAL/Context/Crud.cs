using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DAL.Context;

namespace Data.DAL.Context
{
    public class Crud
    {
        public string User { get; private set; }
        public SqlConnection Connection { get; private set; }
        public SqlTransaction Transaction { get; private set; }

        public Crud(SqlConnection Connection, SqlTransaction Transaction, string User)
        {
            this.Connection = Connection;
            this.Transaction = Transaction;
            this.User = User;
        }

        private Commands Command = new Commands();

        public long Create(string Query, List<SqlParameter> ListParameters)
        {
            long ResultId = 0;
            var Result = Command.ExecuteScalar(Query, ListParameters.ToArray(), Connection, Transaction);
            long.TryParse(Result.ToString(), out ResultId);

            return ResultId;
        }

        public bool Delete(string Query, List<SqlParameter> ListParameters)
        {
            bool Result = Command.ExecuteNonQuery(Query, ListParameters.ToArray(), Connection, Transaction);
            return Result;
        }

        public DataTable Query(string Query, List<SqlParameter> ListParameters)
        {
            DataTable Result = Command.ExecuteQuery(Query, ListParameters.ToArray(), Connection, Transaction);
            return Result;
        }

        public bool Update(string Query, List<SqlParameter> ListParameters)
        {
            bool Result = Command.ExecuteNonQuery(Query, ListParameters.ToArray(), Connection, Transaction);
            return Result;
        }
    }
}
