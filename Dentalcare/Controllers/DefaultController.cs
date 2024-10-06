using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dentalcare.Controllers
{
    public class DefaultController : Controller
    {

        public ActionResult Home()
        {
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

        public ActionResult Login() {
            ViewBag.Message = "Login.";
            return View();
        }

        public ActionResult Register()
        {
            ViewBag.Message = "Register.";
            return View();
        }

    }
}