using System.Web.Mvc;
using Dentalcare.Services;
using Dentalcare.Models;
using System.Linq;

namespace Dentalcare.Controllers
{
    public class ServiceCategoryController : BaseController
    {
        //Các manager để truy xuất dữ liệu
        private readonly ServiceCategoryManager serviceCategoryManager;
        private readonly PatientManager patientManager;
        private readonly DentistManager dentistManager;

        public ServiceCategoryController()
        {
            this.serviceCategoryManager = new ServiceCategoryManager();
            this.patientManager = new PatientManager();
            this.dentistManager = new DentistManager();
        }

        // Hiển thị trang dịch vụ (dùng các manager để lấy dữ liệu)
        public ActionResult Index()
        {
            var services = serviceCategoryManager.GetAllServiceCategory();
            ViewBag.serviceCategories = services;
            ViewBag.dentists = dentistManager.GetAllDentistsInfo();
            ViewBag.patients = patientManager.GetPatients();
            return View();
        }
        //Hiển thị trang chi tiết dịch vụ (dùng các manager để lấy dữ liệu)
        public ActionResult Details(string meta)
        {
            var services = serviceCategoryManager.GetServicesByCategoryMeta(meta);
            if (services == null || !services.Any())
            {
                return RedirectToAction("_404", "Default");
            }
            ViewBag.Services = services;
            return View();
        }


    }
}