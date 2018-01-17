using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Business.BL.Common;
using Common.Entities;
using System.Data;

namespace Business.BL.Entities
{
    public class PersonaBL
    {
        public static List<Persona> GetData(List<Query> ListQuery, int IdQuery)
        {
            List<Persona> ListPersonas = new List<Persona>();
            try
            {
                DataTable Table = ListQuery.Single(x => x.IdQuery == IdQuery).ResultQuery;

                foreach (DataRow Row in Table.Rows)
                {
                    ListPersonas.Add(new Persona()
                    {
                        Apellido = Row.IsNull("Apellido") ? "" : Row["Apellido"].ToString(),
                        Correo = Row.IsNull("Correo") ? "" : Row["Correo"].ToString(),
                        Edad = Row.IsNull("Edad") ? (short)0 : short.Parse(Row["Edad"].ToString()),
                        Estado = Row.IsNull("Estado") ? true : bool.Parse(Row["Estado"].ToString()),
                        Id = Row.IsNull("Id") ? 0 : long.Parse(Row["Id"].ToString()),
                        Nombre = Row.IsNull("Nombre") ? "" : Row["Nombre"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                // Exceptios
            }
            return ListPersonas;
        }

        public static Query GetAll()
        {
            Query QueryCreate = new Query()
            {
                RawQuery = "SELECT Id, Nombre, Apellido, Edad, Correo, Estado FROM rrhh.Persona;",
                Parameters = new List<SqlParameter>(),
                Type = TypeCrud.Query
            };

            return QueryCreate;
        }

        public static Query GetById(long Id)
        {
            Query QueryCreate = new Query()
            {
                RawQuery = "SELECT Id, Nombre, Apellido, Edad, Correo, Estado FROM rrhh.Persona WHERE Id = @Id;",
                Parameters = new List<SqlParameter>() { new SqlParameter("Id", Id) },
                Type = TypeCrud.Query
            };

            return QueryCreate;
        }

        public static Query Create(Persona pPersona)
        {
            Query QueryCreate = new Query()
            {
                RawQuery = "INSERT INTO rrhh.Persona(Nombre, Apellido, Edad, Correo, Estado) " +
                           "VALUES(@Nombre, @Apellido, @Edad, @Correo, @Estado); SELECT SCOPE_IDENTITY();",
                Parameters = new List<SqlParameter>() {
                                new SqlParameter("@Nombre", pPersona.Nombre),
                                new SqlParameter("@Apellido", pPersona.Apellido),
                                new SqlParameter("@Edad", pPersona.Edad),
                                new SqlParameter("@Correo", pPersona.Correo),
                                new SqlParameter("@Estado", pPersona.Estado)
                            },
                Type = TypeCrud.Create
            };

            return QueryCreate;
        }

        public static Query Update(Persona pPersona)
        {
            Query QueryUpdate = new Query()
            {
                RawQuery = "UPDATE rrhh.Persona SET Nombre=@Nombre, Apellido=@Apellido, Edad=@Edad, Correo=@Correo, Estado=@Estado WHERE Id = @Id;",
                Parameters = new List<SqlParameter>() {
                                new SqlParameter("@Nombre", pPersona.Nombre),
                                new SqlParameter("@Apellido", pPersona.Apellido),
                                new SqlParameter("@Edad", pPersona.Edad),
                                new SqlParameter("@Correo", pPersona.Correo),
                                new SqlParameter("@Estado", pPersona.Estado),
                                new SqlParameter("@Id", pPersona.Id)
                                },
                Type = TypeCrud.Update
            };

            return QueryUpdate;
        }

        public static Query Delete(long Id)
        {
            Query QueryUpdate = new Query()
            {
                RawQuery = "DELETE rrhh.Persona WHERE Id = @Id;",
                Parameters = new List<SqlParameter>() {
                                new SqlParameter("Id", Id)
                                },
                Type = TypeCrud.Delete
            };

            return QueryUpdate;
        }
    }
}
