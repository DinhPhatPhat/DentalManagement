using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            public string Avatar { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Email { get; set; }

            public int Order { get; set; }
        }

        public List<Dentist> GetAllDentists()
        {
            return db.Dentists
                     .Where(f => f.hide == false)
                     .OrderBy(f => f.order)
                     .ToList();
        }

        public List<DentistInfo> GetAllDentistsInfo()
        {
            var dentists = db.Dentists
                        .Where(d => !d.hide)
                        .Join(db.People.Where(p => !p.hide),
                              d => d.id,
                              p => p.id,
                              (d, p) => new { Dentist = d, Person = p })
                        .Join(db.Avatars,
                              dp => dp.Person.id,
                              a => a.personId,
                              (dp, a) => new DentistInfo
                              {
                                  Name = dp.Person.name,
                                  Title = dp.Dentist.title,
                                  Description = dp.Dentist.descrip,
                                  Email = dp.Person.email,
                                  Order = dp.Person.order,
                                  Avatar = a.imgPath
                              })
                        .OrderBy(p => p.Order)
                        .ToList();

            return dentists;
        }

        public List<DentistInfo> GetTopFourDentistsInfo()
        {
            var dentists = db.Dentists
                        .Where(d => !d.hide)
                        .Join(db.People.Where(p => !p.hide),
                              d => d.id,
                              p => p.id,
                              (d, p) => new { Dentist = d, Person = p })
                        .Join(db.Avatars,
                              dp => dp.Person.id,
                              a => a.personId,
                              (dp, a) => new DentistInfo
                              {
                                  Name = dp.Person.name,
                                  Title = dp.Dentist.title,
                                  Description = dp.Dentist.descrip,
                                  Email = dp.Person.email,
                                  Order = dp.Person.order,
                                  Avatar = a.imgPath
                              })
                        .OrderBy(p => p.Order)
                        .Take(4)
                        .ToList();

            return dentists;
        }

    }
}