using Common.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Application.Data
{
    public class Personas : IDisposable, IList<Persona>
    {
        public List<Persona> Persona { get; set; }

        public int Count => Persona.Count;

        public bool IsReadOnly => false;

        public Persona this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Personas()
        {
            Persona = new List<Persona>()
            {
            new Persona(){ Apellido = "Acevedo Manríquez", Correo = "amanriquez@gmail.com", Edad = 19, Estado = true, Id = 1, Nombre = "María Mireya" },
            new Persona(){ Apellido = "Acevedo Mejía", Correo = "qmejia@hotmail.com", Edad = 54, Estado = true, Id = 2, Nombre = "Enrique" },
            new Persona(){ Apellido = "Aguilar Dorantes", Correo = "adorantes@gmail.com", Edad = 22, Estado = true, Id = 3, Nombre = "María Ofelia" },
            new Persona(){ Apellido = "Álvarez Villanueva", Correo = "valvarez@outlook.com", Edad = 19, Estado = true, Id = 4, Nombre = "Salvador" },
            new Persona(){ Apellido = "Amaya Salvador", Correo = "amaya12@hotmail.es", Edad = 33, Estado = true, Id = 5, Nombre = "Arturo Ramiro" },
            new Persona(){ Apellido = "Angulo Garfias", Correo = "angugafias@live.es", Edad = 23, Estado = true, Id = 6, Nombre = "Raúl" },
            new Persona(){ Apellido = "Ballesteros Gutiérrez", Correo = "ballesteros_199@gmail.com", Edad = 19, Estado = true, Id = 7, Nombre = "Rubio Raúl" },
            new Persona(){ Apellido = "Baltazar Cedeño", Correo = "balta_cedño@hotmail.com", Edad = 38, Estado = true, Id = 8, Nombre = "Luis Rubén" },
            new Persona(){ Apellido = "Bautista Vázquez", Correo = "1899bernal@gmail.com", Edad = 84, Estado = true, Id = 9, Nombre = "Juan Miguel de Jesús" },
            new Persona(){ Apellido = "Bernal Rosales", Correo = "rosalitosbernal@gmail.com", Edad = 18, Estado = true, Id = 10, Nombre = "Gigliola Taide" },
            new Persona(){ Apellido = "Cámara Contreras", Correo = "contrerascast_2@hotmail.com", Edad = 19, Estado = true, Id = 11, Nombre = "Juan de Dios" },
            new Persona(){ Apellido = "García Ledesma", Correo = "glesma@live.com", Edad = 33, Estado = true, Id = 12, Nombre = "César Armando" },
            new Persona(){ Apellido = "Castillo Martínez", Correo = "cast_martiny@gmail.com", Edad = 54, Estado = true, Id = 13, Nombre = "" },
            new Persona(){ Apellido = "Castro Borbón", Correo = "castro@hotmail.com", Edad = 67, Estado = true, Id = 14, Nombre = "Guadalupe Victoriana" },
            new Persona(){ Apellido = "De León Sánchez", Correo = "degrrrsanchez@gmail.com", Edad = 99, Estado = true, Id = 15, Nombre = "David" },
            new Persona(){ Apellido = "Del Toro Arreola", Correo = "del_toro_arre@hotmail.es", Edad = 39, Estado = true, Id = 16, Nombre = "Norma" },
            new Persona(){ Apellido = "Díaz Cruz", Correo = "diassscruzzz@gmail.com", Edad = 45, Estado = true, Id = 17, Nombre = "Héctor Federico" }
            };
        }

        public bool Edit(Persona item)
        {
            try
            {
                if (item.Id < 1)
                {
                    return false;
                }

                Persona.Single(x => x.Id == item.Id).Apellido = item.Apellido;
                Persona.Single(x => x.Id == item.Id).Correo = item.Correo;
                Persona.Single(x => x.Id == item.Id).Edad = item.Edad;
                Persona.Single(x => x.Id == item.Id).Estado = item.Estado;
                Persona.Single(x => x.Id == item.Id).Nombre = item.Nombre;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {

        }

        public int IndexOf(Persona item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, Persona item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(Persona item)
        {
            try
            {
                long index = Persona.Max(x => x.Id) + 1;
                item.Id = index;
                Persona.Add(item);
            }
            catch (Exception)
            {
                item.Id = 0;
            }
        }

        public void Clear()
        {
            Persona.Clear();
        }

        public bool Contains(Persona item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Persona[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Persona item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(long Id)
        {
            try
            {
                return Persona.Remove(
                    Persona.Single(x => x.Id == Id)
                    );
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerator<Persona> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}