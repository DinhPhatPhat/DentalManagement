using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dentalcare.Areas.admin.Models; 
using Dentalcare.Models; 

namespace Dentalcare.Areas.dentist.Controllers
{
    public class DefaultController : BaseController
    {
        private clinicEntities db = new clinicEntities();
        // GET: admin/Default
        public ActionResult Index()
        {
            var model = new CreateDentistViewModel
            {
                account = Session["Account"] as Account,
                person = Session["Person"] as Person,
                dentist = Session["Dentist"] as Dentist
            };
            ViewBag.falid = new SelectList(db.Faculties, "id", "name", model.dentist.falid);
            ViewBag.id = new SelectList(db.People, "id", "meta", model.dentist.id);
            ViewBag.person = model.person;
            return View(model);

        }
    }
}