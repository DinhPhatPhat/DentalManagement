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
    public class DentistsController : Controller
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Dentists
        public ActionResult Index()
        {
            var dentists = db.Dentists.Include(d => d.Faculty).Include(d => d.Person);
            return View(dentists.ToList());
        }

        // GET: admin/Dentists/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dentist dentist = db.Dentists.Find(id);
            if (dentist == null)
            {
                return HttpNotFound();
            }
            return View(dentist);
        }

        // GET: admin/Dentists/Create
        public ActionResult Create()
        {
            ViewBag.falid = new SelectList(db.Faculties, "id", "name");
            ViewBag.id = new SelectList(db.People, "id", "meta");
            return View();
        }

        // POST: admin/Dentists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "title,hide,meta,order,datebegin,id,falid,descrip")] Dentist dentist)
        {
            if (ModelState.IsValid)
            {
                db.Dentists.Add(dentist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.falid = new SelectList(db.Faculties, "id", "name", dentist.falid);
            ViewBag.id = new SelectList(db.People, "id", "meta", dentist.id);
            return View(dentist);
        }

        // GET: admin/Dentists/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dentist dentist = db.Dentists.Find(id);
            if (dentist == null)
            {
                return HttpNotFound();
            }
            ViewBag.falid = new SelectList(db.Faculties, "id", "name", dentist.falid);
            ViewBag.id = new SelectList(db.People, "id", "meta", dentist.id);
            return View(dentist);
        }

        // POST: admin/Dentists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "title,hide,meta,order,datebegin,id,falid,descrip")] Dentist dentist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dentist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.falid = new SelectList(db.Faculties, "id", "name", dentist.falid);
            ViewBag.id = new SelectList(db.People, "id", "meta", dentist.id);
            return View(dentist);
        }

        // GET: admin/Dentists/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dentist dentist = db.Dentists.Find(id);
            if (dentist == null)
            {
                return HttpNotFound();
            }
            return View(dentist);
        }

        // POST: admin/Dentists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Dentist dentist = db.Dentists.Find(id);
            db.Dentists.Remove(dentist);
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
