using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Business.BL.Entities;
using Business.BL.Common;
using System.Data;
using System.Linq;
using Common.Entities;

namespace Business.BL.Test.Entities
{
    [TestClass]
    public class PersonaBL_Test
    {
        [TestMethod]
        public void GetAll()
        {
            string User = "Prueba";

            Transaction Tran = new Transaction(new List<Query>() { PersonaBL.GetAll() }, User);

            Tran.Execute();
            List<Persona> ListPersonas = PersonaBL.GetData(Tran.ListQuery, 0);

            Assert.IsNotNull(ListPersonas);
        }
    }
}
