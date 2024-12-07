using Dentalcare.Areas.admin.Models;
using Dentalcare.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Dentalcare.Areas.admin.Controllers
{
    public class CalendarsController : BaseController
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Calendars
        public ActionResult Index(string id, int? month, int? year)
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
            ViewBag.PersonId = person.id;

            ViewBag.Year = currentYear;
            ViewBag.Month = currentMonth;
            ViewBag.MonthName = new DateTime(currentYear, currentMonth, 1).ToString("MMMM");
            ViewBag.DaysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

            ViewBag.Schedules = schedules; // Truyền trực tiếp model Calendar

            return View();
        }



        // GET: admin/Calendars/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calendar calendar = db.Calendars.Find(id);
            if (calendar == null)
            {
                return HttpNotFound();
            }
            return View(calendar);
        }

        // GET: admin/Calendars/Create
        public ActionResult Create(string id)
        {
            var person = db.People.FirstOrDefault(p => p.id == id);
            if (person == null)
            {
                return HttpNotFound();
            }

            ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.PersonRole = checkPersonRole(person.role);
            ViewBag.PersonName = person.name;
            ViewBag.PersonId = person.id;

            return View();
        }


        private string GenerateNewCalendar()
        {
            var lastCalendarId = db.Calendars
                                 .OrderByDescending(a => a.id)
                                 .Select(a => a.id)
                                 .FirstOrDefault();

            if (string.IsNullOrEmpty(lastCalendarId))
                return "CA00000001";

            int numberPart = int.Parse(lastCalendarId.Substring(2));

            numberPart++;

            return $"CA{numberPart:D8}";
        }

        // POST: admin/Calendars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Personid, string shift, DateTime workingDate)
        {
            try
            {
                Person person = db.People.FirstOrDefault(a => a.id == Personid);
                if (ModelState.IsValid) {
                    if (string.IsNullOrEmpty(Personid) || string.IsNullOrEmpty(shift))
                    {
                        ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                        ViewBag.PersonId = Personid;
                        ViewBag.PersonName = person.name;
                        ViewBag.PersonRole = checkPersonRole(person.role);
                        ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
                        return View();
                    }


                    if (person == null)
                    {
                        ModelState.AddModelError("Personid", "Người dùng không tồn tại.");
                        ViewBag.PersonId = Personid;
                        ViewBag.PersonName = person.name;
                        ViewBag.PersonRole = checkPersonRole(person.role);
                        return View();
                    }

                    if (workingDate == DateTime.MinValue)
                    {
                        ModelState.AddModelError("workingDate", "Vui lòng chọn ngày làm việc.");
                        ViewBag.PersonId = Personid;
                        ViewBag.PersonName = person.name;
                        ViewBag.PersonRole = checkPersonRole(person.role);
                        ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
                        return View();
                    }

                    DateTime timeStart, timeEnd;

                    if (shift == "morning")
                    {
                        timeStart = workingDate.AddHours(7.5); // 7:30 AM
                        timeEnd = workingDate.AddHours(12);   // 12:00 PM
                    }
                    else if (shift == "afternoon")
                    {
                        timeStart = workingDate.AddHours(13.5); // 1:30 PM
                        timeEnd = workingDate.AddHours(17);     // 5:00 PM
                    }
                    else
                    {
                        ModelState.AddModelError("Chọn ca làm", "Ca làm việc không hợp lệ.");
                        ViewBag.PersonId = Personid;
                        ViewBag.PersonName = person.name;
                        ViewBag.PersonRole = checkPersonRole(person.role);
                        ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
                        return View();
                    }

                    // Kiểm tra lịch trùng
                    bool isDuplicate = db.Calendars.Any(c =>
                        c.Personid == Personid &&
                        DbFunctions.TruncateTime(c.timeStart) == workingDate.Date &&
                        c.timeStart == timeStart);

                    if (isDuplicate)
                    {
                        ModelState.AddModelError("", "Lịch làm việc đã tồn tại cho ca này.");
                        ViewBag.PersonId = Personid;
                        ViewBag.PersonName = person.name;
                        ViewBag.PersonRole = checkPersonRole(person.role);
                        ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
                        return View();
                    }

                    // Thêm lịch mới
                    var calendar = new Calendar
                    {
                        id = GenerateNewCalendar(),
                        Personid = Personid,
                        timeStart = timeStart,
                        timeEnd = timeEnd,
                        datebegin = DateTime.Now,
                        able = true,
                        hide = false,
                        meta = $"lich-lam-viec-{timeStart}",
                    };

                    db.Calendars.Add(calendar);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { id = Personid, month = DateTime.Now.Month, year = DateTime.Now.Year });
                }
                    ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin.");
                    ViewBag.PersonId = Personid;
                    ViewBag.PersonName = person.name;
                    ViewBag.PersonRole = checkPersonRole(person.role);
                    return RedirectToAction("Index", new { id = Personid, month = DateTime.Now.Month, year = DateTime.Now.Year });

                   
              }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: admin/Calendars/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calendar calendar = db.Calendars.Find(id);
            if (calendar == null)
            {
                return HttpNotFound();
            }
            ViewBag.Personid = new SelectList(db.People, "id", "meta", calendar.Personid);
            return View(calendar);
        }

        // POST: admin/Calendars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,timeStart,timeEnd,able,hide,meta,order,datebegin,Personid")] Calendar calendar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calendar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Personid = new SelectList(db.People, "id", "meta", calendar.Personid);
            return View(calendar);
        }

        // GET: admin/Calendars/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Calendar calendar = db.Calendars.Find(id);
            if (calendar == null)
            {
                return HttpNotFound();
            }
            return View(calendar);
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

        // POST: admin/Calendars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Calendar calendar = db.Calendars.Find(id);
            string personId = calendar.Personid;
            db.Calendars.Remove(calendar);
            db.SaveChanges();
            System.Diagnostics.Debug.WriteLine($"Property {personId}");
            return RedirectToAction("Index", new { id = personId });
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
