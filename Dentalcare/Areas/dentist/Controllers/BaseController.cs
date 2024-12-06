using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dentalcare.Areas.dentist.Controllers
{
    public class BaseController : Controller
    {
        // GET: dentist/Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Dentist"] == null)
            {
                filterContext.Result = new RedirectResult(Url.Action("Home", "Default", new { area = "" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}