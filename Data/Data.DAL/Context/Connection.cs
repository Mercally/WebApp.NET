using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.Common;

namespace Data.DAL.Context
{
    public class Connection
    {
        /// <summary>
        /// Establece y abre la conexión con la base de datos
        /// </summary>
        /// <returns></returns>
        public SqlConnection Connect()
        {
            SqlConnection Con = new SqlConnection("data source=DESKTOP-LJ7LCS7;initial catalog=AdminTLE;integrated security=True;MultipleActiveResultSets=True;");
            try
            {
                Con.Open();
            }
            catch (Exception)
            {
                // Exceptions
            }

            return Con;
        }
    }
}
