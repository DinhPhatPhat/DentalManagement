using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dentalcare.Models;

namespace Dentalcare.Areas.receptionist.Controllers
{
    public class AppointmentsController : BaseController
    {
        private clinicEntities db = new clinicEntities();

        // GET: receptionist/Appointments
        public ActionResult Index()
        {
            Account account = Session["Account"] as Account;
            string id = account.id;
            var person = db.People.FirstOrDefault(p => p.id == id);
            if (person == null)
            {
                return HttpNotFound();
            }

            var appointments = db.Appointments              
                .Include(a => a.Dentist)  
                .Include(a => a.Patient);

            return View(appointments.ToList());
        }


        // GET: dentist/Appointments/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.denid = new SelectList(db.Dentists, "id", "title", appointment.denid);
            ViewBag.patid = new SelectList(db.Patients, "id", "meta", appointment.patid);
            return View(appointment);
        }

        // POST: dentist/Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Appointment model, string date, string timeStart, string timeEnd)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var appointment = db.Appointments.Find(model.id);
                    if (appointment == null)
                    {
                        return HttpNotFound();
                    }

                    appointment.symptom = model.symptom;
                    appointment.state = model.state;
                    appointment.note = model.note;

                    appointment.timeStart = DateTime.Parse(date + " " + timeStart);
                    appointment.timeEnd = DateTime.Parse(date + " " + timeEnd);

                    db.Entry(appointment).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin");
                Appointment appointment2 = db.Appointments.Find(model.id);
                if (appointment2 == null)
                {
                    return HttpNotFound();
                }
                ViewBag.denid = new SelectList(db.Dentists, "id", "title", appointment2.denid);
                ViewBag.patid = new SelectList(db.Patients, "id", "meta", appointment2.patid);
                return View(appointment2);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin");
                Appointment appointment2 = db.Appointments.Find(model.id);
                if (appointment2 == null)
                {
                    return HttpNotFound();
                }
                ViewBag.denid = new SelectList(db.Dentists, "id", "title", appointment2.denid);
                ViewBag.patid = new SelectList(db.Patients, "id", "meta", appointment2.patid);
                return View(appointment2);
            }
            
        }

        private string GenerateNewAppointment()
        {
            var lastAppointment = db.Appointments
                                 .OrderByDescending(a => a.id)
                                 .Select(a => a.id)
                                 .FirstOrDefault();

            if (string.IsNullOrEmpty(lastAppointment))
                return "AP00000001";

            int numberPart = int.Parse(lastAppointment.Substring(2));

            numberPart++;

            return $"AP{numberPart:D8}";
        }

        // GET: receptionist/Appointments/Create
        public ActionResult Create()
        {
            ViewBag.Dentists = db.Dentists
                .Select(d => new { Id = d.Person.Account.id, Name = d.Person.name })
                .ToList();

            ViewBag.Patients = db.Patients
                .Select(p => new { Id = p.Person.Account.id, Name = p.Person.name })
                .ToList();

            return View();
        }

        // POST: receptionist/Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment appointment, string date, string timeStart, string timeEnd)
        {
            // Kiểm tra các giá trị đầu vào
            if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(timeStart) || string.IsNullOrEmpty(timeEnd))
            {
                ModelState.AddModelError("", "Ngày, giờ bắt đầu và giờ kết thúc không được để trống.");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        string id = GenerateNewAppointment();
                        // Nếu các giá trị không rỗng, tiến hành xử lý
                        var newAppointment = new Appointment
                        {
                            id = id,
                            able = true,
                            symptom = appointment.symptom,
                            state = "Chưa xong",
                            timeStart = DateTime.Parse(date + " " + timeStart),
                            timeEnd = DateTime.Parse(date + " " + timeEnd),
                            note = appointment.note,
                            hide = false,
                            meta = "lich-hen" + id,
                            datebegin = DateTime.Now,
                            denid = appointment.denid,
                            patid = appointment.patid

                        };



                        db.Appointments.Add(newAppointment);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi khi không thể chuyển đổi giá trị ngày giờ
                        ModelState.AddModelError("", $"Lỗi khi thêm, vui lòng kiểm tra đầy đủ thông tin");
                        ViewBag.Dentists = db.Dentists
                            .Select(d => new { Id = d.Person.Account.id, Name = d.Person.name })
                            .ToList();

                        ViewBag.Patients = db.Patients
                            .Select(p => new { Id = p.Person.Account.id, Name = p.Person.name })
                            .ToList();
                        return View();
                    }
                }

                ModelState.AddModelError("", $"Vui lòng kiểm tra đầy đủ thông tin");
                ViewBag.Dentists = db.Dentists
                    .Select(d => new { Id = d.Person.Account.id, Name = d.Person.name })
                    .ToList();

                ViewBag.Patients = db.Patients
                    .Select(p => new { Id = p.Person.Account.id, Name = p.Person.name })
                    .ToList();
                return View();

            }
            catch (DbEntityValidationException e)
            {

                ModelState.AddModelError("", $"Vui lòng kiểm tra đầy đủ thông tin");
                ViewBag.Dentists = db.Dentists
                    .Select(d => new { Id = d.Person.Account.id, Name = d.Person.name })
                    .ToList();

                ViewBag.Patients = db.Patients
                    .Select(p => new { Id = p.Person.Account.id, Name = p.Person.name })
                    .ToList();
                return View();
            }

        }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
