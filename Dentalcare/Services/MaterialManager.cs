using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dentalcare.Services
{
    public class MaterialManager
    {
        private readonly clinicEntities db;
        public MaterialManager()
        {
            db = new clinicEntities();
        }
        public List<Material> getMaterials()
        {
            return db.Materials
                .Where(f => f.hide == false)
                     .OrderBy(f => f.new_order)
                     .ToList();
        }
    }
}