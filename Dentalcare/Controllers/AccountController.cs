using Dentalcare.Models;
using System.Linq;
using System.Web.Mvc;

namespace Dentalcare.Controllers
{
    public class AccountController : BaseController
    {
        private clinicEntities db = new clinicEntities();

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            var account = db.Accounts.FirstOrDefault(a => a.username == userName && a.password == password);
            if (account != null)
            {
                var person = db.People.FirstOrDefault(p => p.id == account.id);
                if (person != null)
                {
                    var role = person.role;

                    switch (role)
                    {
                        case 1: // Admin
                            var admin = db.Admins.FirstOrDefault(a => a.id == person.id);
                            Session["Account"] = account;
                            Session["Person"] = person;
                            Session["Admin"] = admin;
                            Session["Role"] = "Admin";
                            return RedirectToAction("Index", "Admin", new { area = "admin" });

                        case 2: // Receptionist
                            var receptionist = db.Receptionists.FirstOrDefault(r => r.id == person.id);
                            Session["Account"] = account;
                            Session["Person"] = person;
                            Session["Receptionist"] = receptionist;
                            Session["Role"] = "Receptionist";
                            return RedirectToAction("Index", "Default", new { area = "receptionist" });

                        case 3: // Dentist
                            var dentist = db.Dentists.FirstOrDefault(d => d.id == person.id);
                            Session["Account"] = account;
                            Session["Person"] = person;
                            Session["Dentist"] = dentist;
                            Session["Role"] = "Dentist";
                            return RedirectToAction("Index", "Default", new { area = "dentist" });

                        default:
                            ViewBag.ErrorMessage = "Role không xác định.";
                            return View("Login", "Default");
                    }
                }
            }

            // Nếu đăng nhập thất bại
            TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng.";

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["Role"] != null)
            {
                // Điều hướng dựa trên Role
                switch (Session["Role"].ToString())
                {
                    case "Admin":
                        return RedirectToAction("Index", "Admin", new { area = "admin" });
                    case "Receptionist":
                        return RedirectToAction("Index", "Default", new { area = "receptionist" });
                    case "Dentist":
                        return RedirectToAction("Index", "Default", new { area = "dentist" });
                    default:
                        // Nếu Role không xác định, trả về trang đăng nhập
                        return View("~/Views/Default/Login.cshtml");
                }
            }

            // Nếu chưa đăng nhập, hiển thị trang đăng nhập
            return View("~/Views/Default/Login.cshtml");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Clear();
            TempData["logout"] = "Đăng xuất thành công";
            return RedirectToAction("Login", "Account");
        }
    }
}
