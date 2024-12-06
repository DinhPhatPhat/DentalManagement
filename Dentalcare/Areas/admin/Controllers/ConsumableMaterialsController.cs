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
using static Dentalcare.Areas.admin.Controllers.PrescriptionsController;

namespace Dentalcare.Areas.admin.Controllers
{
    public class ConsumableMaterialsController : Controller
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/ConsumableMaterials
        public ActionResult Index()
        {
            var consumableMaterials = db.ConsumableMaterials.Include(c => c.Material).Include(c => c.Medicine);
            return View(consumableMaterials.ToList());
        }

        // GET: admin/ConsumableMaterials/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumableMaterial consumableMaterial = db.ConsumableMaterials.Find(id);
            if (consumableMaterial == null)
            {
                return HttpNotFound();
            }
            var ingredientConsumableMaterials = db.Ingredient_ConsumableMaterial
                .Where(m => m.consumId == id)
                .Include(m => m.Ingredient)  // Assuming Ingredient is a navigation property
                .ToList();
            // Pass the ingredients to ViewBag
            ViewBag.ingredientConsumableMaterial = ingredientConsumableMaterials;
            return View(consumableMaterial);
        }

        // GET: admin/ConsumableMaterials/Create
        public ActionResult Create()
        {
            // Get materials that don't have related FixedMaterial or ConsumableMaterial
            var materialsWithoutRelationships = db.Materials
                .Where(m => !db.FixedMaterials.Any(fm => fm.id == m.id) &&
                            !db.ConsumableMaterials.Any(cm => cm.id == m.id))
                .ToList();
            // Retrieve the list of ingredients (assuming you have a method to get them)
            var ingredients = db.Ingredients.ToList();

            // If no materials are found, handle the situation gracefully
            if (materialsWithoutRelationships == null || !materialsWithoutRelationships.Any())
            {
                // Optional: Add a default option or handle the case where no materials exist
                materialsWithoutRelationships.Add(new Material { id = "0", name = "No materials available" });
            }

            // Pass the ingredients to the view using ViewBag
            ViewBag.Ingredients = new SelectList(ingredients, "id", "name");

            // Pass the filtered materials to the ViewBag
            ViewBag.Material = new SelectList(materialsWithoutRelationships, "id", "name");
            return View();
        }

        // POST: admin/ConsumableMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,expDate,hide,meta,order,datebegin,able,new_order,selectedIngredients")] ConsumableMaterial consumableMaterial)
        {
            // Get materials that don't have related FixedMaterial or ConsumableMaterial
            var materialsWithoutRelationships = db.Materials
                .Where(m => !db.FixedMaterials.Any(fm => fm.id == m.id) &&
                            !db.ConsumableMaterials.Any(cm => cm.id == m.id))
                .ToList();
            // Retrieve the list of ingredients (assuming you have a method to get them)
            var ingredients = db.Ingredients.ToList();

            // If no materials are found, handle the situation gracefully
            if (materialsWithoutRelationships == null || !materialsWithoutRelationships.Any())
            {
                // Optional: Add a default option or handle the case where no materials exist
                materialsWithoutRelationships.Add(new Material { id = "0", name = "No materials available" });
            }

            // Pass the ingredients to the view using ViewBag
            ViewBag.Ingredients = new SelectList(ingredients, "id", "name");

            // Pass the filtered materials to the ViewBag
            ViewBag.Material = new SelectList(materialsWithoutRelationships, "id", "name");
            // Check if selectedIngredients is null or empty
            if (consumableMaterial.selectedIngredients == null || consumableMaterial.selectedIngredients.Count == 0)
            {
                ModelState.AddModelError("selectedIngredients", "Chọn ít nhất 1 thành phần.");
                return View(consumableMaterial);
            }
            if (ModelState.IsValid)
            {
                consumableMaterial.datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                consumableMaterial.meta = Functions.ConvertToUnSign("vat-lieu-tieu-hao"+ getMaxOrder());
                consumableMaterial.new_order = getMaxOrder();
                consumableMaterial.expDate = Convert.ToDateTime((consumableMaterial.expDate).ToShortDateString());
                db.ConsumableMaterials.Add(consumableMaterial);
                db.SaveChanges();

                foreach (var ingredientId in consumableMaterial.selectedIngredients)
                {
                    var consumableMaterialIngredient = new Ingredient_ConsumableMaterial
                    {
                        ingreId = ingredientId,
                        consumId = consumableMaterial.id,
                        able = true,
                        hide = false,
                        datebegin = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                        meta = $"vat-lieu-tieu-hao-va-thanh-phan-{getMaxOrder()}"
                    };
                    consumableMaterialIngredient.new_order = consumableMaterialIngredient.order;
                    db.Ingredient_ConsumableMaterial.Add(consumableMaterialIngredient);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consumableMaterial);
        }

        // GET: admin/ConsumableMaterials/Edit/5
        public ActionResult Edit(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumableMaterial consumableMaterial = db.ConsumableMaterials.Find(id);
            if (consumableMaterial == null)
            {
                return HttpNotFound();
            }
            // Get materials that don't have related FixedMaterial or ConsumableMaterial
            var materialsWithoutRelationships = db.Materials
                .Where(m => !db.FixedMaterials.Any(fm => fm.id == m.id) &&
                            !db.ConsumableMaterials.Any(cm => cm.id == m.id && cm.id != id))
                .ToList();
            // Retrieve the list of ingredients (assuming you have a method to get them)
            var ingredients = db.Ingredients.ToList();

            // If no materials are found, handle the situation gracefully
            if (materialsWithoutRelationships == null || !materialsWithoutRelationships.Any())
            {
                // Optional: Add a default option or handle the case where no materials exist
                materialsWithoutRelationships.Add(new Material { id = "0", name = "No materials available" });
            }

            // Pass the ingredients to the view using ViewBag
            ViewBag.Ingredients = new SelectList(ingredients, "id", "name");

            // Pass the filtered materials to the ViewBag
            ViewBag.Material = new SelectList(materialsWithoutRelationships, "id", "name");
            return View(consumableMaterial);
        }

        // POST: admin/ConsumableMaterials/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, [Bind(Include = "id,expDate,hide,meta,order,datebegin,able,new_order,selectedIngredients")] ConsumableMaterial consumableMaterial)
        {
            if (id != consumableMaterial.id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Get materials that don't have related FixedMaterial or ConsumableMaterial
            var materialsWithoutRelationships = db.Materials
                .Where(m => !db.FixedMaterials.Any(fm => fm.id == m.id) &&
                            !db.ConsumableMaterials.Any(cm => cm.id == m.id))
                .ToList();
            // Retrieve the list of ingredients (assuming you have a method to get them)
            var ingredients = db.Ingredients.ToList();

            // If no materials are found, handle the situation gracefully
            if (materialsWithoutRelationships == null || !materialsWithoutRelationships.Any())
            {
                // Optional: Add a default option or handle the case where no materials exist
                materialsWithoutRelationships.Add(new Material { id = "0", name = "No materials available" });
            }

            // Pass the ingredients to the view using ViewBag
            ViewBag.Ingredients = new SelectList(ingredients, "id", "name");

            // Pass the filtered materials to the ViewBag
            ViewBag.Material = new SelectList(materialsWithoutRelationships, "id", "name");

            // Check if selectedIngredients is null or empty
            if (consumableMaterial.selectedIngredients == null || consumableMaterial.selectedIngredients.Count == 0)
            {
                ModelState.AddModelError("selectedIngredients", "Chọn ít nhất 1 thành phần.");
                return View(consumableMaterial);
            }

            var existingConsumableMaterial = db.ConsumableMaterials
                .FirstOrDefault(c => c.id == id);

            // Check if the model is valid
            if (ModelState.IsValid)
            {
                // Update the ConsumableMaterial properties
                existingConsumableMaterial.hide = consumableMaterial.hide;
                existingConsumableMaterial.meta = Functions.ConvertToUnSign(existingConsumableMaterial.meta);  // Update meta properly
                existingConsumableMaterial.datebegin = DateTime.Now;  // Set current date for datebegin
                existingConsumableMaterial.new_order = consumableMaterial.new_order;
                existingConsumableMaterial.expDate = consumableMaterial.expDate.Date;  // Ensure date format

                // Update the Ingredient_ConsumableMaterial relationships
                var existingIngredients = db.Ingredient_ConsumableMaterial
                    .Where(i => i.consumId == consumableMaterial.id)
                    .ToList();

                // Remove any ingredients that are no longer selected
                foreach (var existingIngredient in existingIngredients)
                {
                    if (!consumableMaterial.selectedIngredients.Contains(existingIngredient.ingreId))
                    {
                        db.Ingredient_ConsumableMaterial.Remove(existingIngredient);
                    }
                }

                // Add new ingredients that are selected but not already associated
                foreach (var ingredientId in consumableMaterial.selectedIngredients)
                {
                    if (!existingIngredients.Any(ei => ei.ingreId == ingredientId))
                    {
                        var consumableMaterialIngredient = new Ingredient_ConsumableMaterial
                        {
                            ingreId = ingredientId,
                            consumId = consumableMaterial.id,
                            able = true,
                            hide = false,
                            datebegin = DateTime.Now,
                            meta = $"vat-lieu-tieu-hao-va-thanh-phan-{getMaxOrder()}",
                            new_order = getMaxOrderIngredientConsumableMaterial()
                        };

                        db.Ingredient_ConsumableMaterial.Add(consumableMaterialIngredient);
                    }
                }

                // Save changes to the database
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(consumableMaterial);
        }



        // GET: admin/ConsumableMaterials/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsumableMaterial consumableMaterial = db.ConsumableMaterials.Find(id);
            if (consumableMaterial == null)
            {
                return HttpNotFound();
            }
            var ingredientConsumableMaterials = db.Ingredient_ConsumableMaterial
                .Where(m => m.consumId == id)
                .Include(m => m.Ingredient)  // Assuming Ingredient is a navigation property
                .ToList();
            // Pass the ingredients to ViewBag
            ViewBag.ingredientConsumableMaterial = ingredientConsumableMaterials;
            return View(consumableMaterial);
        }

        // POST: admin/ConsumableMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            // Find the ConsumableMaterial by its ID
            ConsumableMaterial consumableMaterial = db.ConsumableMaterials.Find(id);
            if (consumableMaterial == null)
            {
                return HttpNotFound();
            }

            // Check if the ConsumableMaterial is associated with any Medicine
            var associatedMedicine = db.Medicines.FirstOrDefault(m => m.id == consumableMaterial.id);

            if (associatedMedicine != null)
            {
                // If there is an associated Medicine, show an error message
                ModelState.AddModelError("", "Không thể xóa vì vật liệu hao này là thuốc.");
                return View(consumableMaterial); // Return to the view with an error message
            }

            // Remove the associated Ingredient_ConsumableMaterial relationships
            var ingredientConsumableMaterials = db.Ingredient_ConsumableMaterial
                .Where(icm => icm.consumId == consumableMaterial.id)
                .ToList();

            foreach (var ingredientConsumableMaterial in ingredientConsumableMaterials)
            {
                db.Ingredient_ConsumableMaterial.Remove(ingredientConsumableMaterial);
            }

            // Remove the ConsumableMaterial
            db.ConsumableMaterials.Remove(consumableMaterial);

            // Save changes to the database
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
            int lastOrder = db.ConsumableMaterials
                               .OrderByDescending(n => n.order)
                               .Select(n => n.order)
                               .FirstOrDefault();
            return lastOrder + 1;
        }

        public int getMaxOrderIngredientConsumableMaterial()
        {
            int lastOrder = db.Ingredient_ConsumableMaterial
                               .OrderByDescending(n => n.order)
                               .Select(n => n.order)
                               .FirstOrDefault();
            return lastOrder + 1;
        }
    }
}
