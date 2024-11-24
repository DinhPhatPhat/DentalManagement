using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dentalcare.Services
{
    public class FacultyManager
    {
        private readonly clinicEntities db;

        public FacultyManager()
        {
            db = new clinicEntities();
        }
        //Lấy dữ liệu bảng khoa
        public List<Faculty> GetAllFaculties()
        {
            return db.Faculties
                     .Where(f => f.hide == false)
                     .OrderBy(f => f.order)
                     .ToList();
        }
    }
}