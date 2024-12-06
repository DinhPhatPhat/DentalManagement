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
using Dental.Help;
using Dentalcare.Models;

namespace Dentalcare.Areas.admin.Controllers
{
    public class ServicesController : Controller
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Services
        public ActionResult Index(string id =null)
        {
            getCategory(id);
            return View();
        }

        public void getCategory(string selectedId = null)
        {
            ViewBag.Category = new SelectList(db.Service_Category.Where(x => x.hide == false).OrderBy(x => x.order), "id", "name", selectedId);
        }

        public ActionResult getService(string id = null)
        {
            if(id== null || id == "")
            {
                var v = db.Services.OrderBy(x => x.order).ToList();
                return PartialView(v);
            }
            var m = db.Services.Where(x => x.cateId == id).OrderBy(x => x.order).ToList();
            return PartialView(m);
        }

        // GET: admin/Services/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: admin/Services/Create
        public ActionResult Create()
        {
            ViewBag.cateId = new SelectList(db.Service_Category, "id", "name");
            getCategory();
            return View();
        }

        // POST: admin/Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,cateId,name,price,note,descrip,caredActor,meta,hide,new_order,datebegin,calUnit,img")] Service service,HttpPostedFileBase img)
        {
            getCategory();
            try
            {
                // Check if the service name already exists
                var existingService = db.Services
                    .FirstOrDefault(m => m.name.ToLower() == service.name.ToLower()); // Case-insensitive check for the same name

                if (existingService != null)
                {
                    // If the name exists, add a model error
                    ModelState.AddModelError("name", "Tên dịch vụ đã tồn tại.");
                    return View(service); // Return to the form with the error message
                }


                var path = "";
                var filename = "";
                if (ModelState.IsValid)
                {
                    // Generate the new ID
                    string prefix = "SE";
                    var lastId = db.Services
                            .Where(n => n.id.StartsWith(prefix)) // Ensure the id starts with the prefix
                            .Select(n => n.id)
                            .OrderByDescending(id => id) // Order by the alphanumeric id
                            .FirstOrDefault();

                    if (lastId != null)
                    {
                        service.id = Functions.GenerateNewId(prefix, lastId);
                    }
                    else
                    {
                        service.id = prefix + "00000001";
                    }


                    if (img != null)
                    {
                        filename = img.FileName;
                        path = Path.Combine(Server.MapPath("~/Content/images/Service"), filename);
                        img.SaveAs(path);
                        service.img = "Content/images/Service/" + filename;
                    }
                    else
                    {
                        service.img = "Content/images/Service/SE00000001.jpg";
                    }
                    service.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    service.meta = Functions.ConvertToUnSign(service.meta); //convert Tiếng Việt không dấu
                    service.new_order = getMaxOrder();
                    db.Services.Add(service);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Services", new { id = service.cateId });
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
            return View(service);

        }

        // GET: admin/Services/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            getCategory(id);
            return View(service);

        }

        // POST: admin/Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,cateId,name,price,note,descrip,caredActor,meta,hide,new_order,datebegin,calUnit,img")] Service service,HttpPostedFileBase img)
        {
            getCategory();
            try
            {
                // Check if the service name already exists (except for the current material being edited)
                var existingService = db.Materials
                    .FirstOrDefault(m => m.name.ToLower() == service.name.ToLower() && m.id != service.id); // Exclude the current material by checking different ID

                if (existingService != null)
                {
                    // If the name exists, add a model error
                    ModelState.AddModelError("name", "Tên dịch vụ đã tồn tại.");
                    return View(service); // Return to the form with the error message
                }

                var path = "";
                var filename = "";
                Service temp = db.Services.Find(service.id);
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        filename = img.FileName;
                        path = Path.Combine(Server.MapPath("~/Content/images/Service"), filename);
                        img.SaveAs(path);
                        temp.img = "Content/images/Service/" + filename;
                    }
                    
                    temp.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());                   
                    temp.name = service.name;
                    temp.price = service.price;
                    temp.descrip = service.descrip;
                    temp.meta = Functions.ConvertToUnSign(service.meta); //convert Tiếng Việt không dấu
                    if (service.note== null || service.note == "")
                    {
                        temp.note = "";
                    }
                    else
                    {
                        temp.note = service.note;
                    }
                    temp.caredActor = service.caredActor;
                    temp.calUnit = service.calUnit;
                    temp.hide = service.hide;

                    temp.new_order = service.new_order;
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Services", new { id = service.cateId });
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

            return View(service);
        }

        // GET: admin/Services/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = db.Services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: admin/Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Service service = db.Services.Find(id);
            db.Services.Remove(service);
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
            int lastOrder = db.Services
                               .OrderByDescending(n => n.order)
                               .Select(n => n.order)
                               .FirstOrDefault();
            return lastOrder + 1;
        }
    }
}
