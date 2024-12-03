using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dentalcare.Models;

namespace Dentalcare.Areas.admin.Controllers
{
    public class PrescriptionsController : Controller
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Prescriptions
        public ActionResult Index()
        {
            var prescriptions = db.Prescriptions.Include(p => p.Bill).Include(p => p.Dentist).Include(p => p.Patient);
            return View(prescriptions.ToList());
        }

        // GET: admin/Prescriptions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // GET: admin/Prescriptions/Create
        public ActionResult Create()
        {
            ViewBag.billid = new SelectList(db.Bills, "id", "meta");
            ViewBag.denid = new SelectList(db.Dentists, "id", "title");
            ViewBag.patid = new SelectList(db.Patients, "id", "meta");
            return View();
        }

        // POST: admin/Prescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "note,price,hide,meta,order,datebegin,denid,patid,billid,new_order,able")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Prescriptions.Add(prescription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.billid = new SelectList(db.Bills, "id", "meta", prescription.billid);
            ViewBag.denid = new SelectList(db.Dentists, "id", "title", prescription.denid);
            ViewBag.patid = new SelectList(db.Patients, "id", "meta", prescription.patid);
            return View(prescription);
        }

        // GET: admin/Prescriptions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            ViewBag.billid = new SelectList(db.Bills, "id", "meta", prescription.billid);
            ViewBag.denid = new SelectList(db.Dentists, "id", "title", prescription.denid);
            ViewBag.patid = new SelectList(db.Patients, "id", "meta", prescription.patid);
            return View(prescription);
        }

        // POST: admin/Prescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "note,price,hide,meta,order,datebegin,denid,patid,billid,new_order,able")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.billid = new SelectList(db.Bills, "id", "meta", prescription.billid);
            ViewBag.denid = new SelectList(db.Dentists, "id", "title", prescription.denid);
            ViewBag.patid = new SelectList(db.Patients, "id", "meta", prescription.patid);
            return View(prescription);
        }

        // GET: admin/Prescriptions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // POST: admin/Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Prescription prescription = db.Prescriptions.Find(id);
            db.Prescriptions.Remove(prescription);
            db.SaveChanges();
            return RedirectToAction("Index");
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
