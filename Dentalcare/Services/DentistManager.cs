using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Xml.Linq;

namespace Dentalcare.Services
{
    public class DentistManager
    {
        private readonly clinicEntities db;

        public DentistManager()
        {
            db = new clinicEntities();
        }

        public class DentistInfo
        {
            public string Name { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Email { get; set; }

            public int Order { get; set; }

            public string img { get; set; }
        }

        //Lấy thông tin tất cả nha sĩ
        public List<Dentist> GetAllDentists()
        {
            return db.Dentists
                     .Where(f => f.hide == false)
                     .OrderBy(f => f.order)
                     .ToList();
        }


        // Lấy thông tin nha sĩ không bị ẩn, bao gồm ảnh từ bảng People
        public List<DentistInfo> GetAllDentistsInfo()
        {
            var dentists = db.Dentists
                             .Where(d => !d.hide)
                             .Join(db.People.Where(p => !p.hide),
                                   d => d.id,
                                   p => p.id,
                                   (d, p) => new DentistInfo
                                   {
                                       Name = p.name,
                                       Title = d.title,
                                       Description = d.descrip,
                                       Email = p.email,
                                       Order = d.order,
                                       img = p.img
                                   })
                             .OrderBy(di => di.Order)
                             .ToList();

            return dentists;
        }


        //Lấy 4 nha sĩ đầu tiên
        public List<DentistInfo> GetTopFourDentistsInfo()
        {
            var dentists = db.Dentists
                        .Where(d => !d.hide)
                        .Join(db.People.Where(p => !p.hide),
                              d => d.id,
                              p => p.id,
                              (d, p) => new DentistInfo
                              {
                                  Name = p.name,
                                  Title = d.title,
                                  Description = d.descrip,
                                  Email = p.email,
                                  Order = d.order,
                                  img = p.img
                              })                            
                        .OrderBy(di => di.Order)
                        .Take(4)
                        .ToList();

            return dentists;
        }

    }
}