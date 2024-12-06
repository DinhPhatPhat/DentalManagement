using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dentalcare.Areas.dentist.Controllers;
using Dentalcare.Models;

namespace Dentalcare.Areas.dentist.Controllers
{
    public class AppointmentsController : BaseController
    {
        private clinicEntities db = new clinicEntities();

        // GET: dentist/Appointments
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
                .Where(c => c.denid == id)
                .Include(a => a.Dentist)  
                .Include(a => a.Patient); 

            ViewBag.Person = person;
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

            return View(model);
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
