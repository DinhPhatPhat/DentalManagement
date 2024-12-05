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
using System.Web.Services.Description;
using Dentalcare.Models;
using System.Web.Management;

namespace Dentalcare.Areas.admin.Controllers
{
    public class MaterialsController : Controller
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Materials
        public ActionResult Index(string id = null)
        {
            getCategory(id);
            return View();
        }
        //public ActionResult Index()
        //{
        //    getMaterialType();
        //    return View();
        //}

        //public void getMaterialType(string materialType = "")
        //{
        //    // Define the list of available material types
        //    var materialTypes = new List<SelectListItem>
        //        {
        //            new SelectListItem { Value = "", Text = " --- Chọn loại vật liệu --- " }, // Default option
        //            new SelectListItem { Value = "Consumable_Material", Text = "Vật liệu tiêu hao" },
        //            new SelectListItem { Value = "Fixed_Material", Text = "Vật liệu cố định" }
        //        };

        //    // Populate the ViewBag.MaterialType with the material type options
        //    ViewBag.MaterialType = new SelectList(materialTypes, "Value", "Text", materialType);
        //}

        public ActionResult getMaterial(string id = null)
        {
            if (id == null || id == "")
            {
                var v = db.Materials.OrderBy(x => x.order).ToList();
                return PartialView(v);
            }
            var m = db.Materials.Where(x => x.cateId == id).OrderBy(x => x.order).ToList();
            return PartialView(m);
        }

        public void getCategory(string selectedId = null)
        {
            ViewBag.Category = new SelectList(db.Material_Category.Where(x => x.hide == false).OrderBy(x => x.order), "id", "name", selectedId);
        }


        //public ActionResult getMaterial(string materialType = "")
        //{
        //    var materials = new List<Material>();

        //    // Handle the different material types
        //    if (materialType == "Consumable_Material")
        //    {
        //        // Fetch consumable materials
        //        materials = db.ConsumableMaterials
        //                       .Where(x => x.hide == false)  // Add additional conditions if necessary
        //                       .OrderBy(x => x.order)
        //                       .Select(x => x.Material)  // Assuming you need the Material data related to ConsumableMaterial
        //                       .ToList();
        //    }
        //    else if (materialType == "Fixed_Material")
        //    {
        //        // Fetch fixed materials
        //        materials = db.FixedMaterials
        //                       .Where(x => x.hide == false)  // Add additional conditions if necessary
        //                       .OrderBy(x => x.order)
        //                       .Select(x => x.Material)  // Assuming you need the Material data related to FixedMaterial
        //                       .ToList();
        //    }
        //    else
        //    {
        //        // If no type is selected, fetch all materials
        //        materials = db.Materials
        //                       .Where(x => x.hide == false)
        //                       .OrderBy(x => x.order)
        //                       .ToList();
        //    }


        //    // Return the partial view with the materials
        //    return PartialView(materials);
        //}



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
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,cateId,name,quantity,calUnit,func,mfgDate,able,hide,meta,order,datebegin,img")] Material material, HttpPostedFileBase img, bool isFixedMaterial)
        {
            getCategory();
            try
            {
                // Check if the material name already exists
                var existingMaterial = db.Materials
                    .FirstOrDefault(m => m.name.ToLower() == material.name.ToLower()); // Case-insensitive check for the same name

                if (existingMaterial != null)
                {
                    // If the name exists, add a model error
                    ModelState.AddModelError("name", "Tên vật liệu đã tồn tại.");
                    return View(material); // Return to the form with the error message
                }

                var path = "";
                var filename = "";
                if (ModelState.IsValid)
                {
                    // Generate the new ID
                    string prefix = "MA";
                    var lastId = db.Materials
                            .Where(n => n.id.StartsWith(prefix)) // Ensure the id starts with the prefix
                            .Select(n => n.id)
                            .OrderByDescending(id => id) // Order by the alphanumeric id
                    .FirstOrDefault();

                    if (lastId != null)
                    {
                        material.id = Functions.GenerateNewId(prefix, lastId);
                    }
                    else
                    {
                        material.id = prefix + "00000001";
                    }


                    if (img != null)
                    {
                        filename = img.FileName;
                        path = Path.Combine(Server.MapPath("~/Content/images/Material"), filename);
                        img.SaveAs(path);
                        material.img = "Content/images/Material/" + filename;
                    }
                    else
                    {
                        material.img = "Content/images/Material/MA00000001.jpg";
                    }
                    material.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    material.meta = Functions.ConvertToUnSign(material.meta); //convert Tiếng Việt không dấu
                    material.new_order = getMaxOrder();
                    db.Materials.Add(material);
                    db.SaveChanges();

                    if (isFixedMaterial)
                    {
                        var fixedMaterial = new FixedMaterial
                        {
                            id = material.id,
                            hide = material.hide,
                            meta = material.meta,
                            new_order = material.new_order,
                            order = material.order,
                            datebegin = material.datebegin,
                            able = material.able,
                            Material = material // Associate the material with the FixedMaterial entry
                        };
                        db.FixedMaterials.Add(fixedMaterial);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", "Materials", new { id = material.cateId });
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
            getCategory(id);
            return View(material);
        }

        // POST: admin/Materials/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, [Bind(Include = "id,cateId,name,quantity,order,calUnit,func,mfgDate,able,hide,meta,new_order,datebegin,img")] Material material, HttpPostedFileBase img, bool isFixedMaterial)
        {
            getCategory();
            try
            {
                if (id != material.id) // Ensure the IDs match
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                // Check if the material name already exists (except for the current material being edited)
                var existingMaterial = db.Materials
                    .FirstOrDefault(m => m.name.ToLower() == material.name.ToLower() && m.id != material.id); // Exclude the current material by checking different ID

                if (existingMaterial != null)
                {
                    // If the name exists, add a model error
                    ModelState.AddModelError("name", "Tên vật liệu đã tồn tại.");
                    return View(material); // Return to the form with the error message
                }

                var path = "";
                var filename = "";
                var temp = db.Materials.Find(material.id);
                if (ModelState.IsValid)
                {
                    // If an image is uploaded, handle the file
                    if (img != null)
                    {
                        filename = img.FileName;
                        path = Path.Combine(Server.MapPath("~/Content/images/Material"), filename);
                        img.SaveAs(path);
                        temp.img = "Content/images/Material/" + filename;
                    }

                    temp.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    temp.meta = Functions.ConvertToUnSign(material.meta); // Convert Tiếng Việt không dấu
                    temp.func = material.func;
                    temp.quantity = material.quantity;
                    temp.able = true;
                    temp.new_order = material.new_order;
                    temp.cateId = material.cateId;
                    temp.calUnit = material.calUnit;
                    temp.mfgDate =  Convert.ToDateTime((material.mfgDate).ToShortDateString());
                    temp.hide = material.hide;
                    temp.meta = Functions.ConvertToUnSign(material.meta); //convert Tiếng Việt không dấu


                    // Update the material in the database
                    db.Entry(temp).State = EntityState.Modified;
                    db.SaveChanges();

                    if (isFixedMaterial)
                    {
                        // Check if a fixed material entry exists for this material
                        var fixedMaterial = db.FixedMaterials.FirstOrDefault(fm => fm.id == material.id);
                        if (fixedMaterial == null)
                        {
                            // Create a new fixed material if it does not exist
                            fixedMaterial = new FixedMaterial
                            {
                                id = material.id,
                                hide = material.hide,
                                meta = Functions.ConvertToUnSign(material.meta),
                                new_order = material.new_order,
                                order = material.order,
                                datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                                able = material.able,
                            };
                            db.FixedMaterials.Add(fixedMaterial);
                        }
                        else
                        {
                            // Update the existing fixed material if it already exists
                            fixedMaterial.hide = material.hide;
                            fixedMaterial.meta = Functions.ConvertToUnSign(material.meta);
                            fixedMaterial.new_order = material.new_order;
                            fixedMaterial.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                            fixedMaterial.able = material.able;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        // If isFixedMaterial is false, remove the FixedMaterial entry if it exists
                        var fixedMaterial = db.FixedMaterials.FirstOrDefault(fm => fm.id == material.id);
                        if (fixedMaterial != null)
                        {
                            db.FixedMaterials.Remove(fixedMaterial);
                        }
                        db.SaveChanges();
                    }


                    return RedirectToAction("Index", "Materials", new { id = material.cateId });
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

        public int getMaxOrder()
        {
            int lastOrder = db.Materials
                               .OrderByDescending(n => n.order)
                               .Select(n => n.order)
                               .FirstOrDefault();
            return lastOrder + 1;
        }
    }
}
