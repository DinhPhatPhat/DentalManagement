using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dentalcare.Services
{
    public class InfoClinicManager
    {
        private readonly clinicEntities db;

        public InfoClinicManager()
        {
            this.db = new clinicEntities();
        }

        public Clinic getDataOfClinic()
        {
            return db.Clinics.
                Where(f => f.hide == false).First();
        }
    }
}