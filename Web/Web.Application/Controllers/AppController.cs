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
        public ActionResult Index() // Ssplash screen
        {
            return View();
        }

        public PartialViewResult Dashboard()
        {
            return PartialView();
        }
    }
}