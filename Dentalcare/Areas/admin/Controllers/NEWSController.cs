using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dentalcare.Models;
using System.IO;
using Dental.Help;
using System.Data.Entity.Validation;
using System.Runtime.InteropServices.WindowsRuntime;


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
            NEWS news = db.NEWS.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
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
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,img,title,descrip,msg,meta,hide,order,datebegin")] NEWS news, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";

                if (ModelState.IsValid)
                {
                    // Generate the new ID
                    string prefix = "NE";
                    var lastId = db.NEWS
                            .Where(n => n.id.StartsWith(prefix)) // Ensure the id starts with the prefix
                            .Select(n => n.id)
                            .OrderByDescending(id => id) // Order by the alphanumeric id
                            .FirstOrDefault();

                    if (lastId != null)
                    {
                        news.id = Functions.GenerateNewId(prefix, lastId);
                    }
                    else
                    {
                        news.id = prefix + "00000001";
                    }



                    if (img != null)
                    {
                        filename = img.FileName;
                        path = Path.Combine(Server.MapPath("~/Content/images/Blog"), filename);
                        img.SaveAs(path);
                        news.img = "Content/images/Blog/" + filename;
                    }
                    else
                    {
                        news.img = "Content/images/Blog/NE00000002.jpg";
                    }

                    news.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    news.meta = Functions.ConvertToUnSign(news.meta);
                    news.msg = news.msg;
                    db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT NEWS ON");
                    news.order = getMaxOrder();
                    // Save the news item to the database
                    db.NEWS.Add(news);
                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT NEWS OFF");
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

            return View(news);
        }


        // GET: admin/NEWS/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NEWS news = db.NEWS.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: admin/NEWS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "id,title,img,descrip,msg,meta,hide,order,datebegin")] NEWS news, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";
                NEWS temp = getById(news.id);
                if (ModelState.IsValid)
                {

                    if (img != null)
                    {
                        filename = img.FileName;
                        path = Path.Combine(Server.MapPath("~/Content/images/Blog"), filename);
                        img.SaveAs(path);
                        temp.img = "Content/images/Blog/" + filename;
                    }
                    temp.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());                   
                    temp.title = news.title;
                    temp.descrip = news.descrip;
                    temp.msg = news.msg;
                    temp.meta = Functions.ConvertToUnSign(news.meta); //convert Tiếng Việt không dấu
                    temp.hide = news.hide;
                    db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT NEWS ON");
                    temp.order = news.order;
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand("SET IDENTITY_INSERT NEWS OFF");
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

            return View(news);
        }


        // GET: admin/NEWS/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NEWS news = db.NEWS.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: admin/NEWS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NEWS news = db.NEWS.Find(id);
            db.NEWS.Remove(news);
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

        public NEWS getById(string id)
        {
            return db.NEWS.Where(x => x.id == id).FirstOrDefault();
        }

        public int getMaxOrder()
        {
            if (db.NEWS.Count() == 0)
            {
                return 1;
            }
            return db.NEWS.Count() + 1;
        }
    }
}
