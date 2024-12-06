using Dentalcare.Areas.admin.Models;
using Dentalcare.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Dentalcare.Areas.dentist.Controllers
{
    public class CalendarsController : BaseController
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Calendars
        public ActionResult Index(int? month, int? year)
        {
            Account account = Session["Account"] as Account;
            string id = account.id;
            var person = db.People.FirstOrDefault(p => p.id == id);
            if (person == null)
            {
                return HttpNotFound();
            }

            int currentMonth = month ?? DateTime.Now.Month;
            int currentYear = year ?? DateTime.Now.Year;

            var schedules = db.Calendars
                .Where(c => c.Personid == id && c.timeStart.Year == currentYear && c.timeStart.Month == currentMonth)
                .ToList();

            ViewBag.PersonName = person.name;
            ViewBag.PersonRole = checkPersonRole(person.role);
            ViewBag.PersonId = person.id;

            ViewBag.Year = currentYear;
            ViewBag.Month = currentMonth;
            ViewBag.MonthName = new DateTime(currentYear, currentMonth, 1).ToString("MMMM");
            ViewBag.DaysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

            ViewBag.Schedules = schedules; // Truyền trực tiếp model Calendar

            return View();
        }



        public ActionResult Print(string id, int? month, int? year)
        {
            var person = db.People.FirstOrDefault(p => p.id == id);
            if (person == null)
            {
                return HttpNotFound();
            }

            int currentMonth = month ?? DateTime.Now.Month;
            int currentYear = year ?? DateTime.Now.Year;

            var schedules = db.Calendars
                .Where(c => c.Personid == id && c.timeStart.Year == currentYear && c.timeStart.Month == currentMonth)
                .ToList();

            ViewBag.PersonName = person.name;
            ViewBag.PersonRole = checkPersonRole(person.role);
            ViewBag.Year = currentYear;
            ViewBag.Month = currentMonth;
            ViewBag.MonthName = new DateTime(currentYear, currentMonth, 1).ToString("MMMM");
            ViewBag.DaysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            ViewBag.Schedules = schedules;

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public string checkPersonRole(int role)
        {
            if (role == 3)
            {
                return "Nha sĩ";
            }
            if (role == 2)
            {
                return "Lễ tân";
            }
            if (role == 4)
            {
                return "Phụ tá";
            }
            return "Khác";
        }
    }
}
