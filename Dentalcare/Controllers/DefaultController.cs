using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dentalcare.Services;
namespace Dentalcare.Controllers
{
    public class DefaultController : BaseController
    {

        private readonly FacultyManager facultyManager;
        private readonly DentistManager dentistManager;
        public DefaultController()
        {
            this.facultyManager = new FacultyManager();
            this.dentistManager = new DentistManager();
        }

        public ActionResult PageByMeta(string meta)
        {
            // Tìm menu dựa vào meta, nếu có menu tương ứng với meta truy cập, thì chuyển hướng đến action của meta tương ứng, không thì 404
            var menuItem = db.Menus.FirstOrDefault(m => m.meta == meta);

            //Nếu không tìm thấy meta nào tương ứng thì 404
            if (menuItem == null)
            {
                return RedirectToAction("_404");
            }

            // Tùy vào 'meta', chuyển hướng tới các action tương ứng
            switch (meta)
            {
                case "nha-si":
                    ViewBag.dentists = dentistManager.GetAllDentistsInfo();
                    return View("Dentist");
                case "tin-tuc":
                    return View("Blog"); 
                case "dich-vu":
                    return RedirectToAction("Index", "ServiceCategory");
                case "binh-luan":
                    return RedirectToAction("Index", "Comment");
                case "lien-he":
                    return View("Contact"); 
                case "tai-khoan":
                    return View("Login"); 
                default:
                    return RedirectToAction("_404");
            }
        }

        public ActionResult Home()
        {
            var faculties = facultyManager.GetAllFaculties();
            var topFourDentists = dentistManager.GetTopFourDentistsInfo();
            ViewBag.faculties = faculties;
            ViewBag.topFourDentists = topFourDentists;
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Message = "Login.";
            return View();
        }

        public ActionResult Register()
        {
            ViewBag.Message = "Register.";
            return View();
        }


        public ActionResult _404()
        {
            ViewBag.Message = "PageNotFound.";
            return View();
        }

    }
}