using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Business.BL.Common;
using Business.BL.Entities;
using Common.Entities;
using Web.Application.ViewModels;

namespace Web.Application.Controllers
{
    [OutputCache(Duration = 0, Location = System.Web.UI.OutputCacheLocation.Client, NoStore = true)]
    public class PersonaController : Controller
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        public JsonResponse JSONResponse { get; set; }

        [HttpGet]
        public PartialViewResult Index()
        {
            Transaction Tran = new Transaction("User", PersonaBL.GetAll("Persona.Propietario"));
            Tran.Execute();

            if (Tran.IsSuccess)
            {
                ViewBag.List = PersonaBL.GetData(Query.FindFirst(Tran.ListQuery, 0));
            }
            else
            {
                ViewBag.List = new List<Persona>();
            }

            JSONResponse = new JsonResponse()
            {
                Header = new Header()
                {
                    Title = "Persona Transaccional",
                    ListLocation = new List<Location>() {
                     new Location() { IsActive = false, Name = "Dashboard", Url = Url.Action("Index", "App") },
                     new Location() { IsActive = true, Name = "Persona Transaccional", Url = Url.Action("Index", "Persona") }
                    }
                }
            };

            string json = jss.Serialize(JSONResponse);
            HttpContext.Response.AddHeader("customresponse", json);
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult List()
        {
            Transaction Tran = new Transaction("User", PersonaBL.GetAll("Persona.Propietario"));
            Tran.Execute();

            List<Persona> ListPersona;
            if (Tran.IsSuccess)
            {
                ListPersona = PersonaBL.GetData(Query.FindFirst(Tran.ListQuery, 0));
            }
            else
            {
                ListPersona = new List<Persona>();
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
                Transaction Tran = new Transaction("User", PersonaBL.Create(Persona));
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
            }
            return Json(JSONResponse);
        }

        [HttpGet]
        public PartialViewResult Details(long id)
        {
            Transaction Tran = new Transaction("User", PersonaBL.GetById(id));
            Tran.Execute();

            Persona Persona;
            if (Tran.IsSuccess)
            {
                Persona = PersonaBL.GetData(Query.FindFirst(Tran.ListQuery, 0)).FirstOrDefault();
            }
            else
            {
                Persona = null;
            }
            return PartialView(Persona);
        }

        [HttpGet]
        public PartialViewResult Delete(long id)
        {
            Transaction Tran = new Transaction("User", PersonaBL.GetById(id));
            Tran.Execute();

            Persona Persona;
            if (Tran.IsSuccess)
            {
                Persona = PersonaBL.GetData(Query.FindFirst(Tran.ListQuery, 0)).FirstOrDefault();
            }
            else
            {
                Persona = null;
            }
            return PartialView(Persona);
        }

        [HttpPost]
        public JsonResult DeletePost(long id)
        {
            JSONResponse = new JsonResponse();
            Transaction Tran = new Transaction("User", PersonaBL.Delete(id));
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
            Transaction Tran = new Transaction("User", PersonaBL.GetById(id));
            Tran.Execute();

            Persona Persona;
            if (Tran.IsSuccess)
            {
                Persona = PersonaBL.GetData(Query.FindFirst(Tran.ListQuery, 0)).FirstOrDefault();
            }
            else
            {
                Persona = null;
            }
            return PartialView(Persona);
        }

        [HttpPost]
        public JsonResult Edit(Persona Persona, string btnSubmit = null)
        {
            JSONResponse = new JsonResponse();
            if (ModelState.IsValid)
            {
                Transaction Tran = new Transaction("User", PersonaBL.Update(Persona));
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