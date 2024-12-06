using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dentalcare.Areas.admin.Models; 
using Dentalcare.Models; 

namespace Dentalcare.Areas.receptionist.Controllers
{
    public class DefaultController : BaseController
    {
        private clinicEntities db = new clinicEntities();
        // GET: receptionist/Default
        public ActionResult Index()
        {
            var model = new CreateReceptionistViewModel
            {
                account = Session["Account"] as Account,
                person = Session["Person"] as Person,
                receptionist = Session["Receptionist"] as Receptionist
            };
            ViewBag.id = new SelectList(db.People, "id", "meta", model.receptionist.id);
            ViewBag.person = model.person;
            return View(model);

        }
    }
}