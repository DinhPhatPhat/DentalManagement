﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dentalcare.Models;

namespace Dentalcare.Areas.admin.Controllers
{
    public class ClinicsController : BaseController
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Clinics
        public ActionResult Index()
        {
            return View(db.Clinics.ToList());
        }

        // GET: admin/Clinics/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }
 

        // GET: admin/Clinics/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        // POST: admin/Clinics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,phoneNumber,address,email,facebook,zalo,instagram,youtube,title,msg,meta,datebegin")] Clinic clinic)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Clinic temp = db.Clinics.Find(clinic.id);
                    temp.zalo = clinic.zalo;
                    temp.img = clinic.img;
                    temp.facebook = clinic.facebook;
                    temp.email = clinic.email;
                    temp.title = clinic.title;
                    temp.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(clinic);
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
