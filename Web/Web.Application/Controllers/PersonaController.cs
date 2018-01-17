using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.BL.Common;
using Business.BL.Entities;
using Common.Entities;
using Web.Application.ViewModels;

namespace Web.Application.Controllers
{
    public class PersonaController : Controller
    {
        public JsonResponse JSONResponse { get; set; }

        [HttpGet]
        public ActionResult Index()
        {
            Transaction Tran = new Transaction(new List<Query>() { PersonaBL.GetAll() }, "User");
            Tran.Execute();
            List<Persona> ListPersona = PersonaBL.GetData(Tran.ListQuery, 0);
            ViewBag.List = ListPersona;

            return View();
        }

        [HttpPost]
        public PartialViewResult List()
        {
            Transaction Tran = new Transaction(new List<Query>() { PersonaBL.GetAll() }, "User");
            Tran.Execute();
            List<Persona> ListPersona = PersonaBL.GetData(Tran.ListQuery, 0);

            return PartialView(ListPersona);
        }

        [HttpGet]
        public PartialViewResult Add()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Add(Persona Persona)
        {
            JSONResponse = new JsonResponse();
            Transaction Tran = new Transaction(new List<Query> { PersonaBL.Create(Persona) }, "User");
            Tran.Execute();
            if (ModelState.IsValid)
            {
                Query Result = Tran.GetQuery(0);
                JSONResponse.IsSuccess = true;
                JSONResponse.Modal = new Modal()
                {
                    CloseModalId = "AddModal",
                    OpenModalId = "DetailsModal",
                    Ajax = new Ajax()
                    {
                        Url = Url.Action("Details", "Persona", new { id = Result.ResultScalar }),
                        UpdateElementId = "DivForDetails"
                    }
                };
                JSONResponse.ListRenderPartialViews.Add(new Ajax()
                {
                    UpdateElementId = "DivForList",
                    Async = false,
                    Method = "POST",
                    Url = Url.Action("List", "Persona", null)
                });
            }

            return Json(JSONResponse);
        }

        [HttpGet]
        public PartialViewResult Details(long id)
        {
            Transaction Tran = new Transaction(new List<Query>() { PersonaBL.GetById(id) }, "User");
            Tran.Execute();
            Persona Persona = PersonaBL.GetData(Tran.ListQuery, 0).First();

            return PartialView(Persona);
        }

        [HttpGet]
        public PartialViewResult Delete(long id)
        {
            Transaction Tran = new Transaction(new List<Query>() { PersonaBL.GetById(id) }, "User");
            Tran.Execute();
            Persona Persona = PersonaBL.GetData(Tran.ListQuery, 0).First();

            return PartialView(Persona);
        }

        [HttpPost]
        public JsonResult DeletePost(long id)
        {
            JSONResponse = new JsonResponse();
            Transaction Tran = new Transaction(new List<Query>() { PersonaBL.Delete(id) }, "User");
            Tran.Execute();

            if (Tran.IsSuccess)
            {
                JSONResponse.IsSuccess = true;
                JSONResponse.Modal = new Modal()
                {
                    CloseModalId = "DeleteModal"
                };
                JSONResponse.ListRenderPartialViews.Add(new Ajax()
                {
                    UpdateElementId = "DivForList",
                    Async = false,
                    Method = "POST",
                    Url = Url.Action("List", "Persona", null)
                });
            }

            return Json(JSONResponse);
        }

        [HttpGet]
        public PartialViewResult Edit(long id)
        {
            Transaction Tran = new Transaction(new List<Query>() { PersonaBL.GetById(id) }, "User");
            Tran.Execute();
            Persona Persona = PersonaBL.GetData(Tran.ListQuery, 0).FirstOrDefault();

            return PartialView(Persona);
        }

        [HttpPost]
        public JsonResult Edit(Persona Persona, string btnSubmit = null)
        {
            JSONResponse = new JsonResponse();
            if (ModelState.IsValid)
            {
                Transaction Tran = new Transaction(new List<Query>() { PersonaBL.Update(Persona) }, "User");
                Tran.Execute();
                if (Tran.IsSuccess)
                {
                    JSONResponse.IsSuccess = true;
                    JSONResponse.BtnSubmitId = btnSubmit;
                    JSONResponse.Modal = new Modal()
                    {
                        CloseModalId = "EditModal",
                        OpenModalId = "DetailsModal",
                        Ajax = new Ajax()
                        {
                            UpdateElementId = "DivForDetails",
                            Url = Url.Action("Details", "Persona", new { id = Persona.Id })
                        }
                    };
                    JSONResponse.ListRenderPartialViews.Add(new Ajax()
                    {
                        UpdateElementId = "DivForList",
                        Async = false,
                        Method = "POST",
                        Url = Url.Action("List", "Persona", null)
                    });
                }
            }
            return Json(JSONResponse);
        }

        [HttpGet]
        public JsonResult CorreoVal(string Correo)
        {
            if (Correo.Contains("gmail"))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error su correo no es válido.", JsonRequestBehavior.AllowGet);
            }
        }
    }
}