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
    public class AssisstantsController : Controller
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Assisstants
        public ActionResult Index()
        {
            var assisstants = db.Assisstants.Include(a => a.Person);
            return View(assisstants.ToList());
        }

        // GET: admin/Assisstants/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assisstant assisstant = db.Assisstants.Find(id);
            if (assisstant == null)
            {
                return HttpNotFound();
            }
            return View(assisstant);
        }

        // GET: admin/Assisstants/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.People, "id", "meta");
            return View();
        }

        // POST: admin/Assisstants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "hide,meta,order,datebegin,id")] Assisstant assisstant)
        {
            if (ModelState.IsValid)
            {
                db.Assisstants.Add(assisstant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.People, "id", "meta", assisstant.id);
            return View(assisstant);
        }

        // GET: admin/Assisstants/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assisstant assisstant = db.Assisstants.Find(id);
            if (assisstant == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.People, "id", "meta", assisstant.id);
            return View(assisstant);
        }

        // POST: admin/Assisstants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "hide,meta,order,datebegin,id")] Assisstant assisstant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assisstant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.People, "id", "meta", assisstant.id);
            return View(assisstant);
        }

        // GET: admin/Assisstants/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assisstant assisstant = db.Assisstants.Find(id);
            if (assisstant == null)
            {
                return HttpNotFound();
            }
            return View(assisstant);
        }

        // POST: admin/Assisstants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Assisstant assisstant = db.Assisstants.Find(id);
            db.Assisstants.Remove(assisstant);
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
