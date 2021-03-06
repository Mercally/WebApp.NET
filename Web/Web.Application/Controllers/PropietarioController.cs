﻿using Business.BL.Common;
using Business.BL.Entities;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.Application.ViewModels;

namespace Web.Application.Controllers
{
    [OutputCache(Duration = 0, Location = System.Web.UI.OutputCacheLocation.Client, NoStore = true)]
    public class PropietarioController : Controller
    {
        public JsonResponse JSONResponse { get; set; }

        [HttpGet]
        public ActionResult Index()
        {
            Transaction Tran = new Transaction("User", PropietarioBL.GetAll("Propietario.Persona"));
            Tran.Execute();
            if (Tran.IsSuccess)
            {
                ViewBag.List = PropietarioBL.GetData(Query.FindFirst(Tran.ListQuery, 0));
            }
            else
            {
                ViewBag.List = new List<Propietario>();
            }
            return View();
        }

        [HttpPost]
        public PartialViewResult List()
        {
            Transaction Tran = new Transaction("User", PropietarioBL.GetAll("Propietario.Persona"));
            Tran.Execute();

            List<Propietario> List;
            if (Tran.IsSuccess)
            {
                List = PropietarioBL.GetData(Query.FindFirst(Tran.ListQuery, 0));
            }
            else
            {
                List = new List<Propietario>();
            }
            return PartialView(List);
        }

        [HttpGet]
        public PartialViewResult Details(long id)
        {
            Transaction Tran = new Transaction("User", PropietarioBL.GetById(id));
            Tran.Execute();

            Propietario Propietario = null;
            if (Tran.IsSuccess)
            {
                Propietario = PropietarioBL.GetData(Query.FindFirst(Tran.ListQuery, 0)).FirstOrDefault();
            }
            return PartialView(Propietario);
        }

        [HttpGet]
        public PartialViewResult Add()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Add(Propietario Propietario)
        {
            JSONResponse = new JsonResponse();
            if (ModelState.IsValid)
            {
                Transaction Tran = new Transaction("User", PropietarioBL.Create(Propietario));
                Tran.Execute();

                if (Tran.IsSuccess)
                {
                    var Result = Tran.GetQuery(0).Result;
                    JSONResponse.IsSuccess = true;
                    JSONResponse.Modal = new Modal()
                    {
                        CloseModalId = "AddModal",
                        OpenModalId = "DetailsModal",
                        Ajax = new Ajax()
                        {
                            Url = Url.Action("Details", "Propietario", new { id = Result.ResultScalar }),
                            UpdateElementId = "DivForDetails"
                        }
                    };
                    JSONResponse.ListRenderPartialViews.Add(new Ajax()
                    {
                        UpdateElementId = "DivForList",
                        Async = false,
                        Method = "POST",
                        Url = Url.Action("List", "Propietario", null)
                    });
                }
            }
            return Json(JSONResponse);
        }

        [HttpGet]
        public PartialViewResult Edit(long id)
        {
            Transaction Tran = new Transaction("User", PropietarioBL.GetById(id, "Propietario.Persona"));
            Tran.Execute();

            Propietario Propietario = new Propietario();
            if (Tran.IsSuccess)
            {
                Propietario = PropietarioBL.GetData(Tran.GetQuery(0)).FirstOrDefault();
            }

            PropietarioViewModel PVM = new PropietarioViewModel()
            {
                Estado = Propietario.Estado,
                Id = Propietario.Id,
                Nombre = Propietario.Persona.Nombre + " " + Propietario.Persona.Apellido,
                PersonaId = Propietario.PersonaId
            };

            return PartialView(PVM);
        }

        [HttpPost]
        public JsonResult Edit(Propietario Propietario)
        {
            JSONResponse = new JsonResponse();
            Transaction Tran = new Transaction("User", PropietarioBL.Update(Propietario));

            if (Tran.IsSuccess)
            {
                JSONResponse.IsSuccess = true;
                JSONResponse.Modal = new Modal()
                {
                    CloseModalId = "EditModal",
                    OpenModalId = "DetailsModal",
                    Ajax = new Ajax()
                    {
                        Url = Url.Action("Details", "Propietario", new { id = Propietario.Id }),
                        UpdateElementId = "DivForDetails"
                    }
                };
            }
            return Json(JSONResponse);
        }
    }
}