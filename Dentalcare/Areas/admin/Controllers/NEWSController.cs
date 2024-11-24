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
    public class NEWSController : Controller
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/NEWS
        public ActionResult Index()
        {
            return View(db.NEWS.ToList());
        }

        // GET: admin/NEWS/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NEWS nEWS = db.NEWS.Find(id);
            if (nEWS == null)
            {
                return HttpNotFound();
            }
            return View(nEWS);
        }

        // GET: admin/NEWS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: admin/NEWS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,img,title,msg,meta,hide,order,datebegin")] NEWS nEWS)
        {
            if (ModelState.IsValid)
            {
                db.NEWS.Add(nEWS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nEWS);
        }

        // GET: admin/NEWS/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NEWS nEWS = db.NEWS.Find(id);
            if (nEWS == null)
            {
                return HttpNotFound();
            }
            return View(nEWS);
        }

        // POST: admin/NEWS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,img,title,msg,meta,hide,order,datebegin")] NEWS nEWS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nEWS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nEWS);
        }

        // GET: admin/NEWS/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NEWS nEWS = db.NEWS.Find(id);
            if (nEWS == null)
            {
                return HttpNotFound();
            }
            return View(nEWS);
        }

        // POST: admin/NEWS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NEWS nEWS = db.NEWS.Find(id);
            db.NEWS.Remove(nEWS);
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
