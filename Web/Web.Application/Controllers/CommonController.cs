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
        public ActionResult NotFound()
        {
            return PartialView();
        }

        public ActionResult InternalError()
        {
            return PartialView();
        }
    }
}