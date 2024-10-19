using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dentalcare.Controllers
{
    public class BaseController : Controller
    {
        //BaseController: Controller tạo ra cho mấy controller khác kế thừa,
        //Controller nào kế thừa sẽ lấy được ViewBag.MenuItems để truyền về cho Views/Shared/MyLayout.cshtml
        //Mục đích là lấy menu động (Sau này có thể phát triển để lấy thêm Footer động)
        protected clinicEntities db = new clinicEntities();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var menuItems = db.Menus
                              .Where(m => m.hide == false)
                              .OrderBy(m => m.order)
                              .ToList();
            ViewBag.MenuItems = menuItems;

            //var footerItems = db.
            //...
            //ViewBag.FooterItems = footerItems
                
            var facultyItems = db.Faculties.ToList();
            ViewBag.FacultyItems = facultyItems;

            base.OnActionExecuting(filterContext);


        }
    }
}