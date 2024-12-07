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
    public class MaterialsController : BaseController
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Materials
        public ActionResult Index()
        {
            var materials = db.Materials.Include(m => m.ConsumableMaterial).Include(m => m.FixedMaterial).Include(m => m.Material_Category);
            return View(materials.ToList());
        }

        // GET: admin/Materials/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // GET: admin/Materials/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.ConsumableMaterials, "id", "meta");
            ViewBag.id = new SelectList(db.FixedMaterials, "id", "meta");
            ViewBag.cateId = new SelectList(db.Material_Category, "id", "name");
            return View();
        }

        // POST: admin/Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,cateId,name,quantity,calUnit,func,mfgDate,able,hide,meta,order,datebegin,img")] Material material)
        {
            if (ModelState.IsValid)
            {
                db.Materials.Add(material);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.ConsumableMaterials, "id", "meta", material.id);
            ViewBag.id = new SelectList(db.FixedMaterials, "id", "meta", material.id);
            ViewBag.cateId = new SelectList(db.Material_Category, "id", "name", material.cateId);
            return View(material);
        }

        // GET: admin/Materials/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.ConsumableMaterials, "id", "meta", material.id);
            ViewBag.id = new SelectList(db.FixedMaterials, "id", "meta", material.id);
            ViewBag.cateId = new SelectList(db.Material_Category, "id", "name", material.cateId);
            return View(material);
        }

        // POST: admin/Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,cateId,name,quantity,calUnit,func,mfgDate,able,hide,meta,order,datebegin,img")] Material material)
        {
            if (ModelState.IsValid)
            {
                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.ConsumableMaterials, "id", "meta", material.id);
            ViewBag.id = new SelectList(db.FixedMaterials, "id", "meta", material.id);
            ViewBag.cateId = new SelectList(db.Material_Category, "id", "name", material.cateId);
            return View(material);
        }

        // GET: admin/Materials/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // POST: admin/Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Material material = db.Materials.Find(id);
            db.Materials.Remove(material);
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
