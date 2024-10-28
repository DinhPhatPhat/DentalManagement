using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dentalcare.Services
{
    public class PatientManager
    {
        private readonly clinicEntities db;

        public PatientManager()
        {
            db = new clinicEntities();
        }
        //Lấy thông tin của bệnh nhân
        public List<Patient> GetPatients()
        {
            return db.Patients
                     .Where(f => f.hide == false)
                     .OrderBy(f => f.order)
                     .ToList();
        }
    }
}
