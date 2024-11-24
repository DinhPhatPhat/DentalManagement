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
    public class Material_CategoryController : Controller
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Material_Category
        public ActionResult Index()
        {
            return View(db.Material_Category.ToList());
        }

        // GET: admin/Material_Category/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material_Category material_Category = db.Material_Category.Find(id);
            if (material_Category == null)
            {
                return HttpNotFound();
            }
            return View(material_Category);
        }

        // GET: admin/Material_Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/Material_Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,descrip,note,meta,hide,able,order,datebegin")] Material_Category material_Category)
        {
            if (ModelState.IsValid)
            {
                db.Material_Category.Add(material_Category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(material_Category);
        }

        // GET: admin/Material_Category/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material_Category material_Category = db.Material_Category.Find(id);
            if (material_Category == null)
            {
                return HttpNotFound();
            }
            return View(material_Category);
        }

        // POST: admin/Material_Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,descrip,note,meta,hide,able,order,datebegin")] Material_Category material_Category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(material_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(material_Category);
        }

        // GET: admin/Material_Category/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material_Category material_Category = db.Material_Category.Find(id);
            if (material_Category == null)
            {
                return HttpNotFound();
            }
            return View(material_Category);
        }

        // POST: admin/Material_Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Material_Category material_Category = db.Material_Category.Find(id);
            db.Material_Category.Remove(material_Category);
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
