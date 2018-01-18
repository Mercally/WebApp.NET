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
        public static List<Persona> GetData(Query Query)
        {
            List<Persona> ListPersonas = new List<Persona>();
            try
            {
                DataTable Table = new DataTable();
                if (Query.IsResolve && Query.IsQuery)
                {
                    Table = Query.Result.ResultQuery;
                }
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

                foreach (var item in Query.Includes)
                {
                    switch (item)
                    {
                        case "Persona.Propietario":
                            var ListPropietarios = PropietarioBL.GetData(Query.FindFirst(Query.SubQuery, item));
                            ListPersonas = ListPersonas.Select(x => new Persona()
                            {
                                Propietario = ListPropietarios.Where(p => p.PersonaId == x.Id),
                                Apellido = x.Apellido,
                                Correo = x.Correo,
                                Edad = x.Edad,
                                Estado = x.Estado,
                                Id = x.Id,
                                Nombre = x.Nombre
                            }).ToList();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                // Exceptios
            }
            return ListPersonas;
        }

        public static Query GetAll(params string[] Includes)
        {
            Query QuerySelect = new Query()
            {
                RawQuery = "SELECT Id, Nombre, Apellido, Edad, Correo, Estado FROM rrhh.Persona;",
                Type = TypeCrud.Query
            };
            QuerySelect.Includes = Includes;
            List<Query> ListSubQuey = new List<Query>();
            foreach (var item in Includes)
            {
                Query SubQuery = null;
                switch (item)
                {
                    case "Persona.Propietario":
                        SubQuery = new Query()
                        {
                            RawQuery = "SELECT PR.Id, PR.PersonaId, PR.Estado FROM geo.Propietario AS PR " +
                                       "INNER JOIN rrhh.Persona AS PE ON (PE.Id = PR.PersonaId);",
                            Type = TypeCrud.Query
                        };
                        break;
                    default:
                        break;
                }
                SubQuery.NameInclude = item;
                ListSubQuey.Add(SubQuery);
            }
            QuerySelect.SubQuery = ListSubQuey.ToArray();

            return QuerySelect;
        }

        public static Query GetById(long Id)
        {
            Query QueryGetById = new Query()
            {
                RawQuery = "SELECT Id, Nombre, Apellido, Edad, Correo, Estado FROM rrhh.Persona WHERE Id = @Id;",
                Parameters = new List<SqlParameter>() { new SqlParameter("Id", Id) },
                Type = TypeCrud.Query
            };

            return QueryGetById;
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
            Query QueryDelete = new Query()
            {
                RawQuery = "DELETE rrhh.Persona WHERE Id = @Id;",
                Parameters = new List<SqlParameter>() {
                                new SqlParameter("Id", Id)
                                },
                Type = TypeCrud.Delete
            };

            return QueryDelete;
        }

        public static Query Search(string Filtro)
        {
            Query QueryGetById = new Query()
            {
                RawQuery = "SELECT Id, Nombre, Apellido, Edad, Correo, Estado FROM rrhh.Persona " +
                            "WHERE Nombre like '%'+@Filtro+'%' OR Apellido like '%'+@Filtro+'%' OR Correo like '%'+@Filtro+'%'",
                Parameters = new List<SqlParameter>() { new SqlParameter("Filtro", Filtro) },
                Type = TypeCrud.Query
            };

            return QueryGetById;
        }
    }
}
