using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Application.ViewModels;
using System.Web.Script.Serialization;

namespace Web.Application.Controllers
{
    [OutputCache(Duration = 0, Location = System.Web.UI.OutputCacheLocation.Client, NoStore = true)]
    public class AppController : Controller
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        public JsonResponse JSONResponse { get; set; }

        public ActionResult Index() // Ssplash screen
        {

            return View();
        }

        public PartialViewResult Dashboard()
        {
            JSONResponse = new JsonResponse()
            {
                Header = new Header()
                {
                    Title = "Dashboard",
                    ListLocation = new List<Location>() {
                     new Location() { IsActive = true, Name = "Dashboard", Url = Url.Action("Index", "App") }
                    }
                }
            };

            string json = jss.Serialize(JSONResponse);
            HttpContext.Response.AddHeader("customresponse", json);
            return PartialView();
        }
    }
}