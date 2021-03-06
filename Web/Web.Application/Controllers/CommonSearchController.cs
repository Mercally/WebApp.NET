﻿using Business.BL.Common;
using Business.BL.Entities;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Application.ViewModels;

namespace Web.Application.Controllers
{
    [OutputCache(Duration = 0, Location = System.Web.UI.OutputCacheLocation.Client, NoStore = true)]
    public class CommonSearchController : Controller
    {
        [HttpGet]
        public PartialViewResult SearchPersona()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult ListSearchPersona(string Filtro)
        {
            Transaction Tran = new Transaction("User", PersonaBL.Search(Filtro));
            Tran.Execute();

            List<Persona> List;
            if (Tran.IsSuccess)
            {
                List = PersonaBL.GetData(Tran.GetQuery(0));
            }
            else
            {
                List = new List<Persona>();
            }
            return PartialView(List);
        }
    }
}