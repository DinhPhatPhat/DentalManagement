using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dental.Help;
using Dentalcare.Models;

namespace Dentalcare.Areas.admin.Controllers
{
    public class MedicinesController : Controller
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Medicines
        public ActionResult Index()
        {
            var medicines = db.Medicines.Include(m => m.ConsumableMaterial);
            return View(medicines.ToList());
        }

        // GET: admin/Medicines/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // GET: admin/Medicines/Create
        public ActionResult Create()
        {
            var materials = db.ConsumableMaterials
                .Where(cm => cm.Medicine == null)  // Assuming a relationship with Medicine
                .Select(cm => new SelectListItem
                {
                    Value = cm.id.ToString(),  // The ID of the ConsumableMaterial
                    Text = cm.Material.name    // Assuming 'Material' is a related entity of 'ConsumableMaterial'
                })
                .ToList();

            ViewBag.MaterialList = new SelectList(materials, "Value", "Text");
            return View();
        }

        // POST: admin/Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "caredACtor,price,hide,meta,instruction,order,datebegin,able,id,new_order")] Medicine medicine)
        {
            var materials = db.ConsumableMaterials
                .Where(cm => cm.Medicine == null)  // Assuming a relationship with Medicine
                .Select(cm => new SelectListItem
                {
                    Value = cm.id.ToString(),  // The ID of the ConsumableMaterial
                    Text = cm.Material.name    // Assuming 'Material' is a related entity of 'ConsumableMaterial'
                })
                .ToList();

            ViewBag.MaterialList = new SelectList(materials, "Value", "Text");
            if (ModelState.IsValid)
            {
                medicine.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                medicine.meta = Functions.ConvertToUnSign("vat-lieu-tieu-hao" + getMaxOrder());
                medicine.new_order = getMaxOrder();
                db.Medicines.Add(medicine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Check if ConsumableMaterial.id is not selected
            if (string.IsNullOrEmpty(medicine.id))
            {
                ModelState.AddModelError("id", "Vui lòng chọn vật liệu tiêu hao.");  // Add custom validation message
            }

            ViewBag.id = new SelectList(db.ConsumableMaterials, "id", "meta", medicine.id);


            return View(medicine);
        }

        // GET: admin/Medicines/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Include(m => m.ConsumableMaterial).FirstOrDefault(m => m.id == id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConsumableMaterialName = medicine.ConsumableMaterial?.Material?.name;
            return View(medicine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "caredACtor,price,hide,meta,instruction,order,datebegin,able,id,new_order")] Medicine medicine)
        {
            // Check if the model state is valid (e.g., required fields are not empty)
            if (ModelState.IsValid)
            {
                // If there's any issue with the ID, handle it
                if (string.IsNullOrEmpty(medicine.id.ToString()))
                {
                    ModelState.AddModelError("id", "Vui lòng chọn vật liệu tiêu hao.");
                }

                // If the ID is selected, update the record
                if (!ModelState.IsValid)
                {
                    // Reload the ConsumableMaterials for the dropdown
                    var materials = db.ConsumableMaterials
                        .Where(cm => cm.Medicine == null)  // Assuming a relationship with Medicine
                        .Select(cm => new SelectListItem
                        {
                            Value = cm.id.ToString(),  // The ID of the ConsumableMaterial
                            Text = cm.Material.name    // Assuming 'Material' is a related entity of 'ConsumableMaterial'
                        })
                        .ToList();
                    ViewBag.MaterialList = new SelectList(materials, "Value", "Text");
                    return View(medicine);
                }

                // If the ID is selected correctly, proceed with the update
                var existingMedicine = db.Medicines.FirstOrDefault(m => m.id == medicine.id);

                if (existingMedicine == null)
                {
                    return HttpNotFound();  // Return 404 if the medicine is not found
                }

                // Update the fields of the existing medicine object
                existingMedicine.caredACtor = medicine.caredACtor;
                existingMedicine.price = medicine.price;
                existingMedicine.hide = medicine.hide;
                existingMedicine.meta = Functions.ConvertToUnSign(medicine.meta);
                existingMedicine.instruction = medicine.instruction;
                existingMedicine.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                existingMedicine.new_order = medicine.new_order;

                // You can update any other fields as necessary, such as ConsumableMaterial ID
                existingMedicine.id = medicine.id;

                // Mark the entity as modified and save changes
                db.Entry(existingMedicine).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");  // Redirect back to the Index action after successful update
            }

            // If model state is invalid, re-populate the dropdown list for the materials
            var materialsList = db.ConsumableMaterials
                .Where(cm => cm.Medicine == null)  // Assuming a relationship with Medicine
                .Select(cm => new SelectListItem
                {
                    Value = cm.id.ToString(),
                    Text = cm.Material.name
                })
                .ToList();

            ViewBag.MaterialList = new SelectList(materialsList, "Value", "Text", medicine.id);

            return View(medicine);  // Return the view with validation errors
        }


        // GET: admin/Medicines/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicines.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // POST: admin/Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Medicine medicine = db.Medicines.Find(id);
            db.Medicines.Remove(medicine);
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

        public int getMaxOrder()
        {
            int lastOrder = db.Medicines
                               .OrderByDescending(n => n.order)
                               .Select(n => n.order)
                               .FirstOrDefault();
            return lastOrder + 1;
        }
    }
}
