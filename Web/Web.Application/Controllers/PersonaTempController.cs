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
            var Personas = new Personas();

            ListPersona = ListPersona ?? Personas.ListPersonas;
            ViewBag.List = ListPersona;

            return View();
        }

        [HttpPost]
        public PartialViewResult List()
        {
            var Personas = new Personas();
            ListPersona = ListPersona ?? Personas.ListPersonas;

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
                try
                {
                    Persona.Id = ListPersona.Max(x => x.Id) + 1;
                    ListPersona.Add(Persona);
                }
                catch (Exception)
                {
                    Persona.Id = 0;
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
            Persona Persona = ListPersona.Single(x => x.Id == id);
            return PartialView(Persona);
        }

        [HttpGet]
        public PartialViewResult Delete(long id)
        {
            Persona Persona = ListPersona.Single(x => x.Id == id);
            return PartialView(Persona);
        }

        [HttpPost]
        public JsonResult DeletePost(long id)
        {
            JSONResponse = new JsonResponse();
            bool IsSuccess = false;
            try
            {
                IsSuccess = ListPersona.Remove(ListPersona.Single(x => x.Id == id));
            }
            catch (Exception)
            {
                IsSuccess = false;
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
            Persona Persona = ListPersona.Single(x => x.Id == id);
            return PartialView(Persona);
        }

        [HttpPost]
        public JsonResult Edit(Persona Persona, string btnSubmit = null)
        {
            JSONResponse = new JsonResponse();
            if (ModelState.IsValid)
            {
                bool IsSuccess = false;
                try
                {
                    ListPersona.Single(x => x.Id == Persona.Id).Apellido = Persona.Apellido;
                    ListPersona.Single(x => x.Id == Persona.Id).Correo = Persona.Correo;
                    ListPersona.Single(x => x.Id == Persona.Id).Edad = Persona.Edad;
                    ListPersona.Single(x => x.Id == Persona.Id).Estado = Persona.Estado;
                    ListPersona.Single(x => x.Id == Persona.Id).Nombre = Persona.Nombre;
                    IsSuccess = true;
                }
                catch (Exception)
                {
                    IsSuccess = false;
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
                        Url = Url.Action("List", "PersonaTemp", null)
                    });
                }
            }

            return Json(JSONResponse);
        }

        [HttpGet]
        public JsonResult CorreoVal(string Correo)
        {
            if (Correo.Contains("@"))
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