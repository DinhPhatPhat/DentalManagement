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
    public class Service_CategoryController : Controller
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Service_Category
        public ActionResult Index()
        {
            return View(db.Service_Category.ToList());
        }

        // GET: admin/Service_Category/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service_Category service_Category = db.Service_Category.Find(id);
            if (service_Category == null)
            {
                return HttpNotFound();
            }
            return View(service_Category);
        }

        // GET: admin/Service_Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/Service_Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,descrip,note,meta,hide,order,datebegin")] Service_Category service_Category)
        {
            if (ModelState.IsValid)
            {
                db.Service_Category.Add(service_Category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(service_Category);
        }

        // GET: admin/Service_Category/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service_Category service_Category = db.Service_Category.Find(id);
            if (service_Category == null)
            {
                return HttpNotFound();
            }
            return View(service_Category);
        }

        // POST: admin/Service_Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,descrip,note,meta,hide,order,datebegin")] Service_Category service_Category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(service_Category);
        }

        // GET: admin/Service_Category/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service_Category service_Category = db.Service_Category.Find(id);
            if (service_Category == null)
            {
                return HttpNotFound();
            }
            return View(service_Category);
        }

        // POST: admin/Service_Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Service_Category service_Category = db.Service_Category.Find(id);
            db.Service_Category.Remove(service_Category);
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
