using Dentalcare.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dentalcare.Controllers
{
    public class DentistController : BaseController
    {
        // GET: Dentist

        private readonly DentistManager dentistManager;

        private readonly PatientManager patientManager;
        public DentistController() {

            this.dentistManager = new DentistManager();

            this.patientManager = new PatientManager();
        }
        public ActionResult Index()
        {
            ViewBag.dentists = dentistManager.GetAllDentistsInfo();
            ViewBag.patients = patientManager.GetPatients();
            return View();
        }
    }
}