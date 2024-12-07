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
        //Các Manager dùng để truy xuất dữ liệu
        private readonly FacultyManager facultyManager;
        private readonly DentistManager dentistManager;
        private readonly CommentManager commentManager;
        private readonly InfoClinicManager infoClinicManager;
        private readonly PatientManager patientManager;

        public DefaultController()
        {
            this.facultyManager = new FacultyManager();
            this.dentistManager = new DentistManager();
            this.commentManager = new CommentManager();
            this.infoClinicManager = new InfoClinicManager();
            this.patientManager = new PatientManager();
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
                    return RedirectToAction("Index", "Dentist");
                case "tin-tuc":
                    return RedirectToAction("Index", "Blog");
                case "dich-vu":
                    return RedirectToAction("Index", "ServiceCategory");
                case "binh-luan":
                    return RedirectToAction("Index", "Comment");
                case "lien-he":
                    return RedirectToAction("Contact", "Default");
                case "tai-khoan":
                    return RedirectToAction("Login", "Account");
                default:
                    return RedirectToAction("_404", "Default");
            }
        }

        public ActionResult Home()
        {
            var faculties = facultyManager.GetAllFaculties();
            var topFourDentists = dentistManager.GetTopFourDentistsInfo();
            ViewBag.comments = commentManager.GetAllCommentInfo();
            ViewBag.faculties = faculties;
            ViewBag.topFourDentists = topFourDentists;
            ViewBag.clinic = infoClinicManager.getDataOfClinic();
            ViewBag.dentists = dentistManager.GetAllDentists();
            ViewBag.patients = patientManager.GetPatients();
            return View();
        }
     
        public ActionResult Contact()
        {
            return View("Contact");
        }

        public ActionResult _404()
        {
            ViewBag.Message = "PageNotFound.";
            return View();
        }

    }
}