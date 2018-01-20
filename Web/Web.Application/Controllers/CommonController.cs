using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Application.ViewModels;
using System.Web.Script.Serialization;

namespace Web.Application.Controllers
{
    public class CommonController : Controller
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        public JsonResponse JSONResponse { get; set; }

        public ActionResult NotFound()
        {
            JSONResponse = new JsonResponse()
            {
                Header = new Header()
                {
                    Title = "404 Error Page Not Found",
                    ListLocation = new List<Location>() {
                     new Location() { IsActive = false, Name = "Dashboard", Url = Url.Action("Index", "App") },
                     new Location() { IsActive = true, Name = "404 Error Page Not Found", Url = Url.Action("NotFound", "Common") }
                    }
                }
            };
            
            string json = jss.Serialize(JSONResponse);
            HttpContext.Response.AddHeader("customresponse", json);
            return PartialView();
        }

        public ActionResult InternalError()
        {
            JSONResponse = new JsonResponse()
            {
                Header = new Header()
                {
                    Title = "505 Error Internal Error",
                    ListLocation = new List<Location>() {
                     new Location() { IsActive = false, Name = "Dashboard", Url = Url.Action("Index", "App") },
                     new Location() { IsActive = true, Name = "505 Error Internal Error", Url = Url.Action("InternalError", "Common") }
                    }
                }
            };

            string json = jss.Serialize(JSONResponse);
            HttpContext.Response.AddHeader("customresponse", json);
            return PartialView();
        }
    }
}