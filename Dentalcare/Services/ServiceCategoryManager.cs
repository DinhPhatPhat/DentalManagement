using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dentalcare.Services
{
    public class ServiceCategoryManager
    {
        private readonly clinicEntities db;

        public ServiceCategoryManager()
        {
            db = new clinicEntities();
        }
        
        //Lấy tất cả các service
        public List<Service_Category> GetAllServiceCategory()
        {
            return db.Service_Category
                     .Where(f => f.hide == false)
                     .ToList();
        }

        //Lấy các service theo meta
        public List<Service> GetServicesByCategoryMeta(string categoryMeta)
        {
            var category = db.Service_Category.FirstOrDefault(sc => sc.meta == categoryMeta && sc.hide == false);
            if (category == null)
            {
                return new List<Service>();
            }
            return db.Services
                     .Where(s => s.cateId == category.id && s.hide == false)
                     .ToList();
        }


    }
}