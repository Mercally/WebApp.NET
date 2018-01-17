using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business.BL.Common;
using Business.BL.Entities;
using Common.Entities;
using Web.Application.Data;
using Web.Application.ViewModels;

namespace Web.Application.Controllers
{
    public class PersonaTempController : Controller
    {
        public JsonResponse JSONResponse { get; set; }
        public static List<Persona> ListPersona { get; set; }

        [HttpGet]
        public ActionResult Index()
        {
            using (var Lista = new Personas())
            {
                ListPersona = ListPersona ?? Lista.Persona;
                ViewBag.List = ListPersona;
            }

            return View();
        }

        [HttpPost]
        public PartialViewResult ListTemp()
        {
            using (var Lista = new Personas())
            {
                ListPersona = ListPersona ?? Lista.Persona;
            }

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
            if (ModelState.IsValid)
            {
                using (var List = new Personas())
                {
                    List.Add(Persona);
                    ListPersona = List.Persona;
                }
                if (Persona.Id > 0)
                {
                    JSONResponse.IsSuccess = true;
                    JSONResponse.Modal = new Modal()
                    {
                        CloseModalId = "AddModal",
                        OpenModalId = "DetailsModal",
                        Ajax = new Ajax()
                        {
                            Url = Url.Action("Details", "PersonaTemp", new { id = Persona.Id }),
                            UpdateElementId = "DivForDetails"
                        }
                    };
                    JSONResponse.ListRenderPartialViews.Add(new Ajax()
                    {
                        UpdateElementId = "DivForList",
                        Async = false,
                        Method = "POST",
                        Url = Url.Action("List", "PersonaTemp", null)
                    });
                }
            }

            return Json(JSONResponse);
        }

        [HttpGet]
        public PartialViewResult Details(long id)
        {
            Persona Persona;
            using (var List = new Personas())
            {
                Persona = ListPersona.Single(x => x.Id == id);
            }

            return PartialView(Persona);
        }

        [HttpGet]
        public PartialViewResult Delete(long id)
        {
            Persona Persona;
            using (var List = new Personas())
            {
                Persona = ListPersona.Single(x => x.Id == id);
            }

            return PartialView(Persona);
        }

        [HttpPost]
        public JsonResult DeletePost(long id)
        {
            JSONResponse = new JsonResponse();
            bool IsSuccess = false;
            using (var List = new Personas())
            {
                IsSuccess = List.Remove(id);
                ListPersona = List.Persona;
            }
            if (IsSuccess)
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
                    Url = Url.Action("List", "PersonaTemp", null)
                });
            }

            return Json(JSONResponse);
        }

        [HttpGet]
        public PartialViewResult Edit(long id)
        {
            Persona Persona;
            using (var List = new Personas())
            {
                Persona = ListPersona.Single(x => x.Id == id);
            }

            return PartialView(Persona);
        }

        [HttpPost]
        public JsonResult Edit(Persona Persona, string btnSubmit = null)
        {
            JSONResponse = new JsonResponse();
            if (ModelState.IsValid)
            {
                bool IsSuccess = false;
                using (var List = new Personas())
                {
                    IsSuccess = List.Edit(Persona);
                    ListPersona = List.Persona;
                }
                if (IsSuccess)
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
                            Url = Url.Action("Details", "PersonaTemp", new { id = Persona.Id })
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
            if (!Correo.Contains(" ") && Correo.Contains("@"))
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