using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dentalcare.Areas.admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Admin"] == null)
            {
                filterContext.Result = new RedirectResult(Url.Action("Home", "Default", new { area = "" }));
            }    
            base.OnActionExecuting(filterContext);
        }
    }
}