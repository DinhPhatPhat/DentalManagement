using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Dental.Help;
using Dentalcare.Models;

namespace Dentalcare.Areas.admin.Controllers
{
    public class Service_CategoryController : BaseController
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
        public ActionResult Create([Bind(Include = "id,name,descrip,note,meta,hide,new_order,datebegin")] Service_Category service_Category)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    //check name exists or not
                    bool nameExists = db.Service_Category
                                 .Any(s => s.name.Equals(service_Category.name, StringComparison.OrdinalIgnoreCase));

                    if (nameExists)
                    {
                        // Add a model error for the 'name' field
                        ModelState.AddModelError("name", "Tên loại dịch vụ đã tồn tại");
                        return View(service_Category); // Return the view with the error
                    }


                    // Generate the new ID
                    string prefix = "SC";
                    var lastId = db.Service_Category
                            .Where(n => n.id.StartsWith(prefix)) // Ensure the id starts with the prefix
                            .Select(n => n.id)
                            .OrderByDescending(id => id) // Order by the alphanumeric id
                            .FirstOrDefault();

                    if (lastId != null)
                    {
                        service_Category.id = Functions.GenerateNewId(prefix, lastId);
                    }
                    else
                    {
                        service_Category.id = prefix + "00000001";
                    }

                    service_Category.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    service_Category.meta = Functions.ConvertToUnSign(service_Category.meta);
                    service_Category.new_order = getMaxOrder();
                    db.Service_Category.Add(service_Category);
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
        public ActionResult Edit([Bind(Include = "id,name,descrip,note,meta,hide,new_order,datebegin")] Service_Category service_Category)
        {
            try
            {
                Service_Category temp = db.Service_Category.Find(service_Category.id);

                // Check if the name already exists in the database, excluding the current record
                bool nameExists = db.Service_Category
                                     .Any(s => s.name.Equals(service_Category.name, StringComparison.OrdinalIgnoreCase) && s.id != service_Category.id);

                if (nameExists)
                {
                    // Add a model error for the 'name' field if the name already exists
                    ModelState.AddModelError("name", "Tên loại dịch vụ đã tồn tại.");
                    return View(service_Category); // Return to the view with the error
                }

                if (ModelState.IsValid)
                {
                    temp.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    temp.name = service_Category.name;
                    temp.descrip = service_Category.descrip;
                    temp.meta = Functions.ConvertToUnSign(service_Category.meta); //convert Tiếng Việt không dấu
                    if (service_Category.note == null || service_Category.note == "")
                    {
                        temp.note = "";
                    }
                    else
                    {
                        temp.note = service_Category.note;
                    }
                    temp.hide = service_Category.hide;

                    temp.new_order = service_Category.new_order;
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
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

        public int getMaxOrder()
        {
            int lastOrder = db.Service_Category
                               .OrderByDescending(n => n.order)
                               .Select(n => n.order)
                               .FirstOrDefault();
            return lastOrder + 1;
        }
    }
}
