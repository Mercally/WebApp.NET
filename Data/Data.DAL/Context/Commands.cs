using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.Common;
using System.Data;

namespace Data.DAL.Context
{
    internal class Commands
    {
        /// <summary>
        /// Retorna una tabla de acuerdo a la consulta proporcionada
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="ListParameters"></param>
        /// <param name="Connection"></param>
        /// <param name="Transaction"></param>
        /// <returns></returns>
        public DataTable ExecuteQuery(string Query, SqlParameter[] ListParameters, SqlConnection Connection, SqlTransaction Transaction)
        {
            DataTable DataTable = new DataTable();
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }
                SqlCommand cmd = new SqlCommand(Query, Connection, Transaction);
                cmd.Parameters.AddRange(ListParameters);

                SqlDataReader sqldr = cmd.ExecuteReader();
                DataTable.Load(sqldr);

                return DataTable;
            }
            catch (Exception)
            {
                // Excepciones
            }
            return DataTable;
        }

        /// <summary>
        /// Ejecuta comandos delete y update
        /// </summary>
        /// <param name="NonQuery"></param>
        /// <param name="Connection"></param>
        /// <param name="Transaction"></param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string NonQuery, SqlParameter[] ListParameters, SqlConnection Connection, SqlTransaction Transaction)
        {
            bool Result = false;
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }
                SqlCommand cmd = new SqlCommand(NonQuery, Connection, Transaction);
                cmd.Parameters.AddRange(ListParameters);                
                int Changes = cmd.ExecuteNonQuery();
                Result = Changes > 0;
            }
            catch (Exception)
            {
                // Excepciones
            }
            return Result;
        }

        /// <summary>
        /// Ejecuta un comando que devuelve un solo valor
        /// </summary>
        /// <param name="NonQuery"></param>
        /// <param name="Connection"></param>
        /// <param name="Transaction"></param>
        /// <returns></returns>
        public object ExecuteScalar(string NonQuery, SqlParameter[] ListParameters, SqlConnection Connection, SqlTransaction Transaction)
        {
            object Result = null;
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                }
                SqlCommand cmd = new SqlCommand(NonQuery, Connection, Transaction);
                cmd.Parameters.AddRange(ListParameters);

                Result = cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                // Excepciones
            }
            return Result;
        }
    }
}
