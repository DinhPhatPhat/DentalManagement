using Dentalcare.Models;
using System.Linq;
using System.Web.Mvc;
using BCrypt.Net;
namespace Dentalcare.Controllers
{
    public class AccountController : BaseController
    {
        private clinicEntities db = new clinicEntities();

        private Account checkValidLogin(string userName, string password)
        {
            var account = db.Accounts.FirstOrDefault(a => a.username == userName);
            if (account != null)
            {
                bool isPasswordValid;
                if (account.password.StartsWith("$2"))
                {
                    isPasswordValid = BCrypt.Net.BCrypt.Verify(password, account.password);
                }
                else
                {
                    isPasswordValid = account.password == password;
                    if (isPasswordValid)
                    {
                        account.password = BCrypt.Net.BCrypt.HashPassword(password);
                        db.SaveChanges();

                    }
                }
                if (isPasswordValid)
                {
                    return account;
                }
                return null;
            }
            return null;
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
                var account = checkValidLogin(userName, password);
                if (account != null)
                {
                var role = account.Person.role;

                    switch (role)
                    {
                        case 1: // Admin
                            var admin = db.Admins.FirstOrDefault(a => a.id == account.id);
                            Session["Account"] = account;
                            Session["Person"] = account.Person;
                            Session["Admin"] = account.Person.Admin;
                            Session["Role"] = "Admin";
                            return RedirectToAction("Index", "Default", new { area = "admin" });

                        case 2: // Receptionist
                            var receptionist = db.Receptionists.FirstOrDefault(r => r.id == account.id);
                            Session["Account"] = account;
                            Session["Person"] = account.Person;
                            Session["Receptionist"] = account.Person.Receptionist;
                            Session["Role"] = "Receptionist";
                            return RedirectToAction("Index", "Default", new { area = "receptionist" });

                        case 3: // Dentist
                            var dentist = db.Dentists.FirstOrDefault(d => d.id == account.id);
                            Session["Account"] = account;
                            Session["Person"] = account.Person;
                            Session["Dentist"] = account.Person.Dentist;
                            Session["Role"] = "Dentist";
                            return RedirectToAction("Index", "Default", new { area = "dentist" });

                        default:
                            TempData["ErrorMessage"] = "Role không xác định.";
                            return View("Login", "Account");
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
                        return View("~/Views/Account/Login.cshtml");
                }
            }

            // Nếu chưa đăng nhập, hiển thị trang đăng nhập
            return View("~/Views/Account/Login.cshtml");
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
