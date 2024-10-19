using System.Web.Mvc;
using Dentalcare.Services;
using Dentalcare.Models;
using System.Linq;

namespace Dentalcare.Controllers
{
    public class ServiceCategoryController : BaseController
    {
        private readonly ServiceCategoryManager serviceCategoryManager;

        public ServiceCategoryController()
        {
            this.serviceCategoryManager = new ServiceCategoryManager();
        }

        // Hiển thị trang dịch vụ
        public ActionResult Index()
        {
            var services = serviceCategoryManager.GetAllServiceCategory();
            ViewBag.serviceCategories = services;
            return View();
        }
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