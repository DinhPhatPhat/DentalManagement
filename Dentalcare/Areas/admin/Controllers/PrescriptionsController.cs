using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dental.Help;
using Dentalcare.Models;
using Newtonsoft.Json;
using static Dentalcare.Areas.admin.Controllers.PrescriptionsController;

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
            ViewBag.billid = new SelectList(db.Bills, "id", "id");
            ViewBag.denid = new SelectList(
                db.Dentists
                    .Join(db.People,
                          dentist => dentist.id,  // Join Dentists' PersonId with Persons' Id
                          person => person.id,           // Join condition
                          (dentist, person) => new       // Create an anonymous object with Dentist id and Person name
                          {
                              id = dentist.id,
                              name = person.name          // Assuming `name` is the field in the `Person` table
                          })
                    .ToList(),
                "id", "name");


            ViewBag.patid = new SelectList(
                db.Patients
                    .Join(db.People,
                          patient => patient.id,  // Join Patients' PersonId with People' Id
                          person => person.id,          // Join condition (assuming 'id' is the key in People)
                          (patient, person) => new      // Create an anonymous object with Patient id and Person name
                          {
                              id = patient.id,           // Use patient id
                              name = person.name         // Use name from the Person table
                          })
                    .ToList(),
                "id", "name");  // Set the value field to 'id' and the display field to 'name'

            var medicines = db.Medicines
                .Include(m => m.ConsumableMaterial) // Include ConsumableMaterial
                .Include(m => m.ConsumableMaterial.Material) // Include Material via ConsumableMaterial
                .ToList();

            // Serialize with settings to handle circular references
            var serializedMedicines = JsonConvert.SerializeObject(
                medicines,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            );

            ViewBag.MedicinesJson = serializedMedicines;
            return View();
        }

        // POST: admin/Prescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "note,price,hide,meta,order,datebegin,denid,patid,new_order,able")] Prescription prescription, List<PrescriptionMedicine> prescription_Medicines)
            {
            try
            {
                //Declaration
                ViewBag.billid = new SelectList(db.Bills, "id", "id");
                ViewBag.denid = new SelectList(
                    db.Dentists
                        .Join(db.People,
                              dentist => dentist.id,  // Join Dentists' PersonId with Persons' Id
                              person => person.id,           // Join condition
                              (dentist, person) => new       // Create an anonymous object with Dentist id and Person name
                              {
                                  id = dentist.id,
                                  name = person.name          // Assuming `name` is the field in the `Person` table
                              })
                        .ToList(),
                    "id", "name");


                ViewBag.patid = new SelectList(
                    db.Patients
                        .Join(db.People,
                              patient => patient.id,  // Join Patients' PersonId with People' Id
                              person => person.id,          // Join condition (assuming 'id' is the key in People)
                              (patient, person) => new      // Create an anonymous object with Patient id and Person name
                              {
                                  id = patient.id,           // Use patient id
                                  name = person.name         // Use name from the Person table
                              })
                        .ToList(),
                    "id", "name");  // Set the value field to 'id' and the display field to 'name'

                var medicines = db.Medicines
                   .Include(m => m.ConsumableMaterial) // Include ConsumableMaterial
                   .Include(m => m.ConsumableMaterial.Material) // Include Material via ConsumableMaterial
                   .ToList();

                // Serialize with settings to handle circular references
                var serializedMedicines = JsonConvert.SerializeObject(
                    medicines,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );

                int currentPrescriptionCount = db.Prescription_Medicine
                    .Where(pm => pm.preId == prescription.id)  // Filter by the current prescription's ID
                    .Count();

                ViewBag.MedicinesJson = serializedMedicines;
                if (ModelState.IsValid)
                {
                    // Generate the new ID
                    string prefix = "PR";
                    var lastId = db.Prescriptions
                            .Where(n => n.id.StartsWith(prefix)) // Ensure the id starts with the prefix
                            .Select(n => n.id)
                            .OrderByDescending(id => id) // Order by the alphanumeric id
                            .FirstOrDefault();

                    prescription.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    prescription.meta = Functions.ConvertToUnSign(prescription.meta);
                    prescription.new_order = getMaxOrder();

                    if (lastId != null)
                    {
                        prescription.id = Functions.GenerateNewId(prefix, lastId);
                    }
                    else
                    {
                        prescription.id = prefix + "00000001";
                    }

                    if (string.IsNullOrEmpty(prescription.billid))
                    {
                        prescription.billid = "BI00000001";
                    }
                    db.Prescriptions.Add(prescription);
                    db.SaveChanges();

                    // Group by medId and sum the quantities for the same medId
                    var groupedMedicines = prescription_Medicines
                        .GroupBy(pm => pm.medId)
                        .Select(g => new
                        {
                            medId = g.Key,
                            totalQuantity = g.Sum(pm => pm.quantityMedicine)
                        })
                        .ToList();


                    // Process the selected medicines
                    foreach (var item in groupedMedicines)
                    {
                        if (item.medId != null  && item.totalQuantity > 0)
                        {
                            // Create the Prescription_Medicine record
                            var prescriptionMedicine = new Prescription_Medicine
                            {
                                preId = prescription.id,
                                medId = item.medId,
                                quantityMedicine = item.totalQuantity,
                                able = true,
                                hide = false,
                                datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                                meta = $"chi-tiet-toa-thuoc-{item.medId}"
                            };

                            // Subtract the prescribed quantity from the Material's quantity
                            var medicine = db.Medicines
                                .Include(m => m.ConsumableMaterial)
                                .FirstOrDefault(m => m.id == item.medId);
                            if (medicine != null && medicine.ConsumableMaterial != null)
                            {
                                var material = medicine.ConsumableMaterial.Material;
                                if (material.quantity >= item.totalQuantity)
                                {
                                    material.quantity -= item.totalQuantity; // Update material quantity
                                    db.Entry(material).State = EntityState.Modified;
                                }
                                else
                                {
                                    throw new Exception("Không đủ thuốc.");
                                }
                            }

                            // Add Prescription_Medicine to the database
                            db.Prescription_Medicine.Add(prescriptionMedicine);
                        }
                    }
                    // Save all changes (both Prescription_Medicine and Material)
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


            return View(prescription);
        }

        // GET: admin/Prescriptions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var prescription = db.Prescriptions.Include(p => p.Prescription_Medicine)
                                       .FirstOrDefault(p => p.id == id);
            if (prescription == null)
            {
                return HttpNotFound();
            }

            ViewBag.billid = new SelectList(db.Bills, "id", "id");
            ViewBag.denid = new SelectList(
                db.Dentists
                    .Join(db.People,
                          dentist => dentist.id,  // Join Dentists' PersonId with Persons' Id
                          person => person.id,           // Join condition
                          (dentist, person) => new       // Create an anonymous object with Dentist id and Person name
                          {
                              id = dentist.id,
                              name = person.name          // Assuming `name` is the field in the `Person` table
                          })
                    .ToList(),
                "id", "name");


            ViewBag.patid = new SelectList(
                db.Patients
                    .Join(db.People,
                          patient => patient.id,  // Join Patients' PersonId with People' Id
                          person => person.id,          // Join condition (assuming 'id' is the key in People)
                          (patient, person) => new      // Create an anonymous object with Patient id and Person name
                          {
                              id = patient.id,           // Use patient id
                              name = person.name         // Use name from the Person table
                          })
                    .ToList(),
                "id", "name");  // Set the value field to 'id' and the display field to 'name'

            var medicines = db.Medicines
                .Include(m => m.ConsumableMaterial) // Include ConsumableMaterial
                .Include(m => m.ConsumableMaterial.Material) // Include Material via ConsumableMaterial
                .ToList();

            // Serialize with settings to handle circular references
            var serializedMedicines = JsonConvert.SerializeObject(
                medicines,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            );
            ViewBag.PrescriptionMedicines = prescription.Prescription_Medicine;
            ViewBag.Medicines = db.Medicines
                .Include(m => m.ConsumableMaterial) // Include ConsumableMaterial
                .Include(m => m.ConsumableMaterial.Material) // Include Material via ConsumableMaterial
                .ToList();
            ViewBag.MedicinesJson = serializedMedicines;
            ViewBag.MedicineCount = prescription.Prescription_Medicine.Count;
            return View(prescription);
        }

        // POST: admin/Prescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,note,price,hide,meta,order,datebegin,denid,patid,billid,new_order,able")] Prescription prescription, List<PrescriptionMedicine> prescription_Medicines)
        {
            try
            {
                // Fetch the existing Prescription from the database
                var existingPrescription = db.Prescriptions.FirstOrDefault(p => p.id == prescription.id);
                // Update the basic prescription details
                if (ModelState.IsValid)
                {
                    existingPrescription.note = prescription.note;
                    existingPrescription.price = prescription.price;
                    existingPrescription.hide = prescription.hide;
                    prescription.meta = Functions.ConvertToUnSign(prescription.meta);
                    prescription.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    existingPrescription.denid = prescription.denid;
                    existingPrescription.patid = prescription.patid;
                    existingPrescription.new_order = prescription.new_order;


                    // Now handle the Prescription_Medicine logic
                    var existingMedicineIds = existingPrescription.Prescription_Medicine.Select(pm => pm.medId).ToList();
                    var groupedMedicines = prescription_Medicines
                        .GroupBy(pm => pm.medId)
                        .Select(g => new
                        {
                            medId = g.Key,
                            totalQuantity = g.Sum(pm => pm.quantityMedicine)
                        })
                        .ToList();

                    // Step 1: Remove medicines that are no longer in the new list
                    foreach (var oldItem in existingPrescription.Prescription_Medicine)
                    {
                        if (!groupedMedicines.Any(pm => pm.medId == oldItem.medId))
                        {
                            // Remove the old medicine
                            db.Prescription_Medicine.Remove(oldItem);

                            // Update the material's quantity as it's being removed
                            var material = db.Medicines
                                .Include(m => m.ConsumableMaterial)
                                .Where(m => m.id == oldItem.medId)
                                .Select(m => m.ConsumableMaterial.Material)
                                .FirstOrDefault();

                            if (material != null)
                            {
                                material.quantity += oldItem.quantityMedicine; // Revert the quantity back to the material
                                db.Entry(material).State = EntityState.Modified;
                            }
                        }
                    }

                    // Step 2: Update existing medicines or add new ones
                    foreach (var item in groupedMedicines)
                    {
                        if (item.medId != null && item.totalQuantity > 0)
                        {
                            // Check if the medicine already exists in the prescription
                            var existingItem = existingPrescription.Prescription_Medicine
                                .FirstOrDefault(pm => pm.medId == item.medId);

                            if (existingItem != null)
                            {
                                // Update the existing medicine's quantity
                                existingItem.quantityMedicine = item.totalQuantity;
                            }
                            else
                            {
                                // Create a new Prescription_Medicine entry
                                var prescriptionMedicine = new Prescription_Medicine
                                {
                                    preId = prescription.id,
                                    medId = item.medId,
                                    quantityMedicine = item.totalQuantity,
                                    able = true,
                                    hide = false,
                                    datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                                    meta = $"chi-tiet-toa-thuoc-{item.medId}"
                                };

                                // Subtract the prescribed quantity from the Material's quantity
                                var medicine = db.Medicines
                                    .Include(m => m.ConsumableMaterial)
                                    .FirstOrDefault(m => m.id == item.medId);

                                if (medicine != null && medicine.ConsumableMaterial != null)
                                {
                                    var material = medicine.ConsumableMaterial.Material;
                                    if (material.quantity >= item.totalQuantity)
                                    {
                                        material.quantity -= item.totalQuantity; // Update material quantity
                                        db.Entry(material).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        throw new Exception("Không đủ thuốc.");
                                    }
                                }

                                // Add the new Prescription_Medicine to the database
                                db.Prescription_Medicine.Add(prescriptionMedicine);
                            }
                        }
                    }

                    // Save all changes to the database
                    db.Entry(existingPrescription).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                ViewBag.billid = new SelectList(db.Bills, "id", "id");
                ViewBag.denid = new SelectList(
                    db.Dentists
                        .Join(db.People,
                              dentist => dentist.id,  // Join Dentists' PersonId with Persons' Id
                              person => person.id,           // Join condition
                              (dentist, person) => new       // Create an anonymous object with Dentist id and Person name
                              {
                                  id = dentist.id,
                                  name = person.name          // Assuming `name` is the field in the `Person` table
                              })
                        .ToList(),
                    "id", "name");


                ViewBag.patid = new SelectList(
                    db.Patients
                        .Join(db.People,
                              patient => patient.id,  // Join Patients' PersonId with People' Id
                              person => person.id,          // Join condition (assuming 'id' is the key in People)
                              (patient, person) => new      // Create an anonymous object with Patient id and Person name
                              {
                                  id = patient.id,           // Use patient id
                                  name = person.name         // Use name from the Person table
                              })
                        .ToList(),
                    "id", "name");  // Set the value field to 'id' and the display field to 'name'

                var medicines = db.Medicines
                    .Include(m => m.ConsumableMaterial) // Include ConsumableMaterial
                    .Include(m => m.ConsumableMaterial.Material) // Include Material via ConsumableMaterial
                    .ToList();

                // Serialize with settings to handle circular references
                var serializedMedicines = JsonConvert.SerializeObject(
                    medicines,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );
                ViewBag.PrescriptionMedicines = prescription.Prescription_Medicine;
                ViewBag.Medicines = db.Medicines
                    .Include(m => m.ConsumableMaterial) // Include ConsumableMaterial
                    .Include(m => m.ConsumableMaterial.Material) // Include Material via ConsumableMaterial
                    .ToList();
                ViewBag.MedicinesJson = serializedMedicines;
                ViewBag.MedicineCount = prescription.Prescription_Medicine.Count;
                return View(prescription);
            }
            catch (Exception ex)
            {
                // Handle the exception (you could log the error here)
                throw ex;
            }
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
            try
            {
                // Find the Prescription being deleted
                Prescription prescription = db.Prescriptions.Find(id);

                if (prescription != null)
                {
                    // Step 1: Find all associated Prescription_Medicine records
                    var prescriptionMedicines = db.Prescription_Medicine.Where(pm => pm.preId == id).ToList();

                    foreach (var pm in prescriptionMedicines)
                    {
                        // Step 2: Restore the quantity of the associated Medicine's Material
                        var medicine = db.Medicines
                            .Include(m => m.ConsumableMaterial)
                            .FirstOrDefault(m => m.id == pm.medId);

                        if (medicine != null && medicine.ConsumableMaterial != null)
                        {
                            var material = medicine.ConsumableMaterial.Material;

                            // Add back the quantity
                            material.quantity += pm.quantityMedicine; // Restore the quantity
                            db.Entry(material).State = EntityState.Modified; // Mark the Material as modified
                        }

                        // Step 3: Delete the Prescription_Medicine record
                        db.Prescription_Medicine.Remove(pm);
                    }

                    // Step 4: Remove the Prescription
                    db.Prescriptions.Remove(prescription);

                    // Save all changes to the database
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle errors appropriately (e.g., log them or show a message to the user)
                Console.WriteLine(ex);
                throw;
            }
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
            int lastOrder = db.Prescriptions
                               .OrderByDescending(n => n.order)
                               .Select(n => n.order)
                               .FirstOrDefault();
            return lastOrder + 1;
        }

        public class PrescriptionMedicine
        {
            public string medId { get; set; }
            public int quantityMedicine { get; set; }
            public int totalPrice { get; set; }
        }
    }
}
