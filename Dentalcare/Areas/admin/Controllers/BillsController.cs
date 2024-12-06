using System;
using System.Collections;
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
using Microsoft.Ajax.Utilities;

namespace Dentalcare.Areas.admin.Controllers
{
    public class BillsController : Controller
    {
        private clinicEntities db = new clinicEntities();
        private string defaultBill = "BI00000001";

        // GET: admin/Bills
        public ActionResult Index()
        {
            var bills = db.Bills
                .Where(b => b.id != defaultBill)
                .Include(b => b.Patient);
            return View(bills.ToList());
        }

        public JsonResult GetPrescriptionPrice(string id)
        {
            var prescription = db.Prescriptions.SingleOrDefault(p => p.id == id);
            if (prescription != null)
            {
                return Json(prescription.price, JsonRequestBehavior.AllowGet);  // Assuming 'Price' is a property of the Prescription model
            }
            return Json(0, JsonRequestBehavior.AllowGet);  // Return 0 if the prescription is not found
        }


        // GET: admin/Bills/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills
                .Include(b => b.Bill_Service)
                .Include(b => b.Prescriptions)
                .FirstOrDefault(b => b.id == id && b.id != defaultBill); // Find bill with id not equal to defaultBill
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // GET: admin/Bills/Create
        public ActionResult Create()
        {
            // Assuming Prescription model has 'id' and 'name' properties
            var prescriptions = db.Prescriptions
                .Where(p => p.billid == defaultBill)
                .Select(p => new { p.id, p.meta })
                .ToList();

            ViewBag.Prescriptions = prescriptions;
            // Assuming you have a Patient model and a database context named db
            var patients = db.Patients.ToList();

            // Add patients to ViewBag for easy access in the view
            ViewBag.PatientsList = new SelectList(patients, "id", "id");
            // Retrieve the list of available services from the database
            var services = db.Services.ToList();

            // Create a dictionary for services with their IDs as keys and prices as values
            var servicePrices = services.ToDictionary(s => s.id, s => s.price);

            // Pass the dictionary to ViewBag
            ViewBag.ServicePrices = servicePrices;

            // Pass the list of services to the view
            ViewBag.ServicesList = new SelectList(services, "id", "name", null);
            return View();
        }

        // POST: admin/Bills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,totalPrice,ServicesPrice,PrescriptionPrice,able,hide,meta,order,datebegin,isPayed,patid,selectedPrescription,selectedBill_Service")] Bill bill)
        {
            try
            {
                // Assuming Prescription model has 'id' and 'name' properties
                var prescriptions = db.Prescriptions.Select(p => new { p.id, p.meta }).ToList();

                ViewBag.Prescriptions = prescriptions;
                // Assuming you have a Patient model and a database context named db
                var patients = db.Patients.ToList();

                // Add patients to ViewBag for easy access in the view
                ViewBag.PatientsList = new SelectList(patients, "id", "id");
                // Retrieve the list of available services from the database
                var services = db.Services.ToList();

                // Create a dictionary for services with their IDs as keys and prices as values
                var servicePrices = services.ToDictionary(s => s.id, s => s.price);

                // Pass the dictionary to ViewBag
                ViewBag.ServicePrices = servicePrices;

                // Pass the list of services to the view
                ViewBag.ServicesList = new SelectList(services, "id", "name", null);
                if (bill.selectedBill_Service == null || bill.selectedBill_Service.Count == 0)
                {
                    ModelState.AddModelError("selectedBill_Service", "Vui lòng chọn toa thuốc.");
                    return View(bill);
                }
                if (bill.selectedPrescription == null)
                {
                    ModelState.AddModelError("selectedPrescription", "Chọn ít nhất 1 dịch vụ.");
                    return View(bill);
                }

                if (ModelState.IsValid)
                {
                    // Generate the new ID
                    string prefix = "BI";
                    var lastId = db.Bills
                            .Where(n => n.id.StartsWith(prefix)) // Ensure the id starts with the prefix
                            .Select(n => n.id)
                            .OrderByDescending(id => id) // Order by the alphanumeric id
                            .FirstOrDefault();


                    if (lastId != null)
                    {
                        bill.id = Functions.GenerateNewId(prefix, lastId);
                    }
                    else
                    {
                        bill.id = prefix + "00000001";
                    }

                    // Process Bill_Service records
                    foreach (var selectedService in bill.selectedBill_Service)
                    {
                        // Create a Bill_Service entity for each selected service
                        var billService = new Bill_Service
                        {
                            billID = bill.id,  // Link to the current Bill
                            SerID = selectedService.ToString(),  // Assuming selectedService is the service ID
                            quantity = 1,  // You can adjust the quantity based on your form input
                            datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString()),  // Set current date or input date
                            able = true,  // You can set this based on your business logic
                            hide = false,  // Similarly, set this based on your requirements
                            meta = "hoa-don-dich-vu-" + getMaxOrderBillService()  // Adjust this as needed
                        };

                        // Add the Bill_Service to the bill
                        db.Bill_Service.Add(billService);
                    }

                    // Handle the Prescription and update the billid
                    var prescription = db.Prescriptions.Find(bill.selectedPrescription);
                    if (prescription != null)
                    {
                        // Update the billid of the selected prescription to match the current bill's ID
                        prescription.billid = bill.id;
                        db.Entry(prescription).State = EntityState.Modified;
                    }

                    bill.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    bill.meta = Functions.ConvertToUnSign(bill.meta);
                    bill.new_order = getMaxOrder();
                    bill.able = true;

                    

                    // Add the Bill to the database
                    db.Bills.Add(bill);
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
            return View(bill);
        }

        // GET: admin/Bills/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bill bill = db.Bills
                .Include(b => b.Bill_Service)          // Include Bill_Service relationship
                .Include(b => b.Prescriptions)         // Include Prescriptions relationship
                .FirstOrDefault(b => b.id == id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            // Assuming Prescription model has 'id' and 'name' properties
            var prescriptions = db.Prescriptions
                .Where(p => p.billid == defaultBill || db.Prescriptions
                    .Where(bp => bp.id == p.id && bp.billid == bill.id)
                    .Any())
                .Select(p => new { p.id, p.meta })
                .ToList();


            ViewBag.Prescriptions = prescriptions;
            // Assuming you have a Patient model and a database context named db
            var patients = db.Patients.ToList();

            // Add patients to ViewBag for easy access in the view
            ViewBag.PatientsList = new SelectList(patients, "id", "id");
            // Retrieve the list of available services from the database
            var services = db.Services.ToList();


            // Create a dictionary for services with their IDs as keys and prices as values
            var servicePrices = services.ToDictionary(s => s.id, s => s.price);
            var totalServicePrice = 0;
            foreach (var serviceId in bill.Bill_Service.Select(b => b.SerID))
            {
                if (servicePrices.ContainsKey(serviceId))
                {
                    totalServicePrice += servicePrices[serviceId]; // Add the price for each selected service
                }
            }

            ViewBag.ServicesPriceTotal = totalServicePrice;

            // Pass the dictionary to ViewBag
            ViewBag.ServicePrices = servicePrices;

            var selectedPrescription = bill.Prescriptions.FirstOrDefault()?.id;
            ViewBag.SelectedPrescription = selectedPrescription;


            // Pass the list of services to the view
            ViewBag.BillService = bill.Bill_Service.Select(s => s.SerID).ToList();
            ViewBag.Prescription = bill.Prescriptions;
            // Pass the dictionary to ViewBag
            ViewBag.ServicePrices = servicePrices;

            // Pass the list of services to the view
            ViewBag.ServicesList = new SelectList(services, "id", "name", null);
            return View(bill);
        }

        // POST: admin/Bills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,totalPrice,ServicesPrice,PrescriptionPrice,able,hide,meta,new_order,datebegin,isPayed,patid,selectedBill_Service,selectedPrescription")] Bill bill)
        {
            // Assuming Prescription model has 'id' and 'name' properties
            var prescriptions = db.Prescriptions
                .Where(p => p.billid == defaultBill || db.Prescriptions
                    .Where(bp => bp.id == p.id && bp.billid == bill.id)
                    .Any())
                .Select(p => new { p.id, p.meta })
                .ToList();

            ViewBag.Prescriptions = prescriptions;
            // Assuming you have a Patient model and a database context named db
            var patients = db.Patients.ToList();

            // Add patients to ViewBag for easy access in the view
            ViewBag.PatientsList = new SelectList(patients, "id", "id");
            // Retrieve the list of available services from the database
            var services = db.Services.ToList();

            // Create a dictionary for services with their IDs as keys and prices as values
            var servicePrices = services.ToDictionary(s => s.id, s => s.price);

            // Pass the dictionary to ViewBag
            ViewBag.ServicePrices = servicePrices;
            

            // Pass the list of services to the view
            ViewBag.ServicesList = new SelectList(services, "id", "name", null);
            if (bill.selectedBill_Service == null || bill.selectedBill_Service.Count == 0)
            {
                ModelState.AddModelError("selectedBill_Service", "Vui lòng chọn toa thuốc.");
                return View(bill);
            }
            if (bill.selectedPrescription == null)
            {
                ModelState.AddModelError("selectedPrescription", "Chọn ít nhất 1 dịch vụ.");
                return View(bill);
            }

            if (ModelState.IsValid)
            {
                // 1. Get the original bill from the database
                var originalBill = db.Bills.Include(b => b.Bill_Service).Include(b => b.Prescriptions).FirstOrDefault(b => b.id == bill.id);
                if (originalBill == null)
                {
                    return HttpNotFound();
                }

                // 2. Remove old services from the Bill_Service table
                var oldServices = db.Bill_Service.Where(bs => bs.billID == bill.id).ToList();
                db.Bill_Service.RemoveRange(oldServices);

                // 3. Add the new selected services to the Bill_Service table (quantity always 1)
                foreach (var selectedService in bill.selectedBill_Service)
                {
                    var billService = new Bill_Service
                    {
                        billID = bill.id,  // Link to the current Bill
                        SerID = selectedService.ToString(),  // Assuming selectedService is the service ID
                        quantity = 1,  // Set quantity to 1
                        datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString()),  // Set current date or input date
                        able = true,  // Assuming service is available
                        hide = false,  // Assuming the service is not hidden
                        meta = "hoa-don-dich-vu-" + getMaxOrderBillService()  // You can adjust this as needed
                    };

                    db.Bill_Service.Add(billService);
                }

                // 4. Handle the Prescription change
                if (bill.selectedPrescription != originalBill.Prescriptions.FirstOrDefault()?.id)
                {
                    // Update the old prescription's billid to null or a default value, if needed
                    var oldPrescription = db.Prescriptions.FirstOrDefault(p => p.billid == originalBill.id);
                    if (oldPrescription != null)
                    {
                        oldPrescription.billid = "B00000001";  // Set it to the old default bill ID (or null if applicable)
                        db.Entry(oldPrescription).State = EntityState.Modified;
                    }

                    // Update the new prescription's billid to match the current bill's ID
                    var newPrescription = db.Prescriptions.Find(bill.selectedPrescription);
                    if (newPrescription != null)
                    {
                        newPrescription.billid = bill.id;
                        db.Entry(newPrescription).State = EntityState.Modified;
                    }
                }

                // 5. Update the bill's other properties
                originalBill.totalPrice = bill.totalPrice;
                originalBill.ServicesPrice = bill.ServicesPrice;
                originalBill.PrescriptionPrice = bill.PrescriptionPrice;
                originalBill.able = bill.able;
                originalBill.hide = bill.hide;
                originalBill.meta = Functions.ConvertToUnSign(bill.meta);
                originalBill.new_order = bill.new_order;
                originalBill.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                originalBill.isPayed = bill.isPayed;
                originalBill.patid = bill.patid;
                originalBill.selectedBill_Service = bill.selectedBill_Service;
                originalBill.selectedPrescription = bill.selectedPrescription;

                db.Entry(originalBill).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.patid = new SelectList(db.Patients, "id", "meta", bill.patid);
            return View(bill);
        }


        // GET: admin/Bills/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: admin/Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Bill bill = db.Bills.Find(id);
            db.Bills.Remove(bill);
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
            int lastOrder = db.Bills
                               .OrderByDescending(n => n.order)
                               .Select(n => n.order)
                               .FirstOrDefault();
            return lastOrder + 1;
        }

        public int getMaxOrderBillService()
        {
            int lastOrder = db.Bill_Service
                               .OrderByDescending(n => n.order)
                               .Select(n => n.order)
                               .FirstOrDefault();
            return lastOrder + 1;
        }
    }
}
