using Business.BL.Common;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BL.Entities
{
    public class PropietarioBL
    {
        public static List<Propietario> GetData(Query Query)
        {
            List<Propietario> ListPropietario = new List<Propietario>();
            try
            {
                DataTable Table = new DataTable();
                if (Query.IsResolve && Query.IsQuery)
                {
                    Table = Query.Result.ResultQuery;
                }

                foreach (DataRow Row in Table.Rows)
                {
                    ListPropietario.Add(new Propietario()
                    {
                        Id = Row.IsNull("Id") ? 0 : long.Parse(Row["Id"].ToString()),
                        PersonaId = Row.IsNull("PersonaId") ? 0 : long.Parse(Row["PersonaId"].ToString()),
                        Estado = Row.IsNull("Estado") ? true : bool.Parse(Row["Estado"].ToString())
                    });
                }

                int IndexSubQuery = 0;
                foreach (var item in Query.Includes)
                {
                    switch (item)
                    {
                        case "Propietario.Persona":
                            List<Persona> Persona = PersonaBL.GetData(Query.FindFirst(Query.SubQuery, item));
                            ListPropietario = ListPropietario.Select(x => new Propietario()
                            {
                                Persona = Persona.FirstOrDefault(y => y.Id == x.PersonaId),
                                PersonaId = x.PersonaId,
                                Estado = x.Estado,
                                Id = x.Id,
                            }).ToList();
                            break;
                        default:
                            break;
                    }
                    IndexSubQuery++;
                }
            }
            catch (Exception)
            {
                // Exceptios
            }
            return ListPropietario;
        }

        public static Query GetAll(params string[] Includes)
        {
            Query QuerySelect = new Query()
            {
                RawQuery = "SELECT Id, PersonaId, Estado FROM geo.Propietario;",
                Type = TypeCrud.Query
            };
            QuerySelect.Includes = Includes;
            List<Query> ListSubQuey = new List<Query>();
            foreach (var item in Includes)
            {
                Query SubQuery = null;
                switch (item)
                {
                    case "Propietario.Persona":
                        SubQuery = new Query()
                        {
                            RawQuery = "SELECT PE.Id, PE.Nombre, PE.Apellido, PE.Edad, PE.Correo, PE.Estado FROM rrhh.Persona AS PE " +
                                       "INNER JOIN geo.Propietario AS PR ON(PE.Id = PR.PersonaId);",
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
                RawQuery = "SELECT Id, PersonaId, Estado FROM geo.Propietario WHERE Id = @Id;",
                Parameters = new List<SqlParameter>() { new SqlParameter("Id", Id) },
                Type = TypeCrud.Query
            };

            return QueryGetById;
        }

        public static Query Create(Propietario pPropietario)
        {
            Query QueryCreate = new Query()
            {
                RawQuery = "INSERT INTO geo.Propietario(PersonaId, Estado) " +
                           "VALUES(@PersonaId, @Estado); SELECT SCOPE_IDENTITY();",
                Parameters = new List<SqlParameter>() {
                                new SqlParameter("PersonaId", pPropietario.PersonaId),
                                new SqlParameter("Estado", pPropietario.Estado)
                            },
                Type = TypeCrud.Create
            };

            return QueryCreate;
        }

        public static Query Update(Propietario pPropietario)
        {
            Query QueryUpdate = new Query()
            {
                RawQuery = "UPDATE geo.Propietario SET PersonaId=@PersonaId, Estado=@Estado WHERE Id = @Id;",
                Parameters = new List<SqlParameter>() {
                                new SqlParameter("PersonaId", pPropietario.PersonaId),
                                new SqlParameter("Estado", pPropietario.Estado)
                                },
                Type = TypeCrud.Update
            };

            return QueryUpdate;
        }

        public static Query Delete(long Id)
        {
            Query QueryDelete = new Query()
            {
                RawQuery = "DELETE geo.Propietario WHERE Id = @Id;",
                Parameters = new List<SqlParameter>() {
                                new SqlParameter("Id", Id)
                                },
                Type = TypeCrud.Delete
            };

            return QueryDelete;
        }
    }
}
