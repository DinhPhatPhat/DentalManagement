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

        public DefaultController()
        {
            this.facultyManager = new FacultyManager();
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
                    return View("Doctors");
                case "tin-tuc":
                    return View("Blog"); 
                case "dich-vu":
                    return View("Service"); 
                case "binh-luan":
                    return View("Comments"); 
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
            ViewBag.faculties = faculties;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About.";
            return View();

        }

        public ActionResult Service()
        {
            ViewBag.Message = "Sevice.";
            return View();
        }

        public ActionResult Doctors()
        {
            ViewBag.Message = "Doctors.";
            return View();
        }

        public ActionResult Blog()
        {
            ViewBag.Message = "Blog.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact.";
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