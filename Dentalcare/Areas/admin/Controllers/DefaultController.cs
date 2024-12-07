using Dentalcare.Areas.admin.Models;
using Dentalcare.Models;
using Dentalcare.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dentalcare.Areas.admin.Controllers
{
    public class DefaultController : Controller
    {
        private clinicEntities db = new clinicEntities();
        private readonly PatientManager patientManager;
        private readonly BillManager billManager;
        private readonly MaterialManager materialManager;

        public DefaultController()
        {
            this.patientManager = new PatientManager();
            this.billManager = new BillManager();
            this.materialManager = new MaterialManager();
        }
        // GET: admin/Default
        public ActionResult Index()
        {
            //var model = new CreateDentistViewModel
            //{
            //    account = Session["Account"] as Account,
            //    person = Session["Person"] as Person,
            //    dentist = Session["Admin"] as Dentist
            //};
            //return View(model);
            ViewBag.patients = patientManager.GetPatients();
            ViewBag.bills = billManager.getBills();
            ViewBag.materials = materialManager.getMaterials();
            ViewBag.averageRevenueMonths = billManager.getAverageRevenueMonths();
            ViewBag.averageRevenueYears = billManager.getAverageRevenueYears();
            ViewBag.yearList = billManager.GetAvailableYears();
            return View();
        }
    }
}