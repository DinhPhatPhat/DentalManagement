using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dentalcare.Models;
using Dentalcare.Areas.admin.Models;
using System.IO;
using System.Data.Entity.Validation;

namespace Dentalcare.Areas.receptionist.Controllers
{
    public class DentistsController : BaseController
    {
        private clinicEntities db = new clinicEntities();
        // GET: admin/Dentists
        public ActionResult Index()
        {
            var dentists = db.Dentists
                             .Include(d => d.Faculty)
                             .Include(d => d.Person)
                             .ToList();
            // Chuyển đổi thành danh sách ViewModel
            var dentistViewModels = dentists.Select(d => new CreateDentistViewModel
            {
                dentist = d,
                person = d.Person,
                account = db.Accounts.FirstOrDefault(a => a.id == d.Person.id)
            }).ToList();
            return View(dentistViewModels);
        }

        // GET: admin/Dentists/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dentist dentist = db.Dentists.Find(id);
            if (dentist == null)
            {
                return HttpNotFound();
            }
            return View(dentist);
        }

        // GET: admin/Dentists/Create
        
        public ActionResult Create()
        {
            ViewBag.falid = new SelectList(db.Faculties, "id", "name");
            return View();
        }

        private string GenerateNewAccountId()
        {
            // Lấy accountId lớn nhất hiện có trong bảng Account
            var lastAccountId = db.Accounts
                                 .OrderByDescending(a => a.id) // Sắp xếp giảm dần theo id
                                 .Select(a => a.id) // Lấy giá trị id
                                 .FirstOrDefault(); // Lấy giá trị đầu tiên

            // Nếu chưa có accountId nào, bắt đầu từ AC00000001
            if (string.IsNullOrEmpty(lastAccountId))
                return "AC00000001";

            // Lấy phần số từ accountId, bỏ phần "AC"
            int numberPart = int.Parse(lastAccountId.Substring(2));

            // Tăng phần số lên 1
            numberPart++;

            // Trả về accountId mới với định dạng "ACXXXXXXXX"
            return $"AC{numberPart:D8}";
        }
        // POST: Dentists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateDentistViewModel model, HttpPostedFileBase avatar)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newAccountId = GenerateNewAccountId();

                    // Kiểm tra trùng tên người dùng
                    if (db.Accounts.Any(a => a.username == model.account.username))
                    {
                        ModelState.AddModelError("account.username", "Tên người dùng đã tồn tại.");
                        ViewBag.falid = new SelectList(db.Faculties, "id", "name");
                        return View(model);
                    }

                    // Kiểm tra trùng số điện thoại
                    if (db.People.Any(p => p.phoneNumber == model.person.phoneNumber))
                    {
                        ModelState.AddModelError("person.phoneNumber", "Số điện thoại đã tồn tại.");
                        ViewBag.falid = new SelectList(db.Faculties, "id", "name");
                        return View(model);
                    }

                    // Kiểm tra trùng email
                    if (db.People.Any(p => p.email == model.person.email))
                    {
                        ModelState.AddModelError("person.email", "Email đã tồn tại.");
                        ViewBag.falid = new SelectList(db.Faculties, "id", "name");
                        return View(model);
                    }

                    // Tạo mới Account với userName và password
                    var account = new Account
                    {
                        id = newAccountId,
                        username = model.account.username,
                        password = BCrypt.Net.BCrypt.HashPassword("123"), // Dùng BCrypt Hash mật khẩu
                        meta = newAccountId,
                        hide = false,
                        datebegin = DateTime.UtcNow,
                        able = true,
                    };

                    // Xử lý upload file avatar
                    string avatarPath = "123";
                    if (avatar != null && avatar.ContentLength > 0)
                    {
                        // Lấy đuôi file (ví dụ .jpg, .png)
                        string fileExtension = Path.GetExtension(avatar.FileName);

                        // Đường dẫn thư mục lưu trữ
                        string folderPath = Server.MapPath($"~/Content/images/{newAccountId}/");

                        // Tạo thư mục nếu chưa tồn tại
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        // Đường dẫn đầy đủ của file
                        avatarPath = $"/Content/images/{newAccountId}/avatar{fileExtension}";

                        // Lưu file lên server
                        avatar.SaveAs(Path.Combine(folderPath, $"avatar{fileExtension}"));
                    }

                    // Tạo mới Person và lưu đường dẫn avatar vào trường img
                    var person = new Person
                    {
                        id = newAccountId,
                        name = model.person.name,
                        phoneNumber = model.person.phoneNumber,
                        email = model.person.email,
                        salary = model.person.salary,
                        address = model.person.address,
                        gender = model.person.gender,
                        birthday = model.person.birthday,
                        nation = model.person.nation,
                        img = avatarPath,  // Lưu đường dẫn avatar vào trường img
                        meta = model.dentist.meta,
                        role = 3,
                        hide = false,
                        datebegin = DateTime.UtcNow
                    };

                    // Tạo mới Dentist
                    var dentist = new Dentist
                    {
                        id = newAccountId,
                        title = model.dentist.title,
                        hide = model.dentist.hide,
                        meta = model.dentist.meta,
                        descrip = model.dentist.descrip,
                        falid = model.dentist.falid, // Lưu faculty ID
                        datebegin = DateTime.UtcNow,
                        new_order = getMaxOrder()
                    };

                    // Lưu các đối tượng vào cơ sở dữ liệu
                    db.Accounts.Add(account);
                    db.People.Add(person);
                    db.Dentists.Add(dentist);
                    db.SaveChanges();

                    return RedirectToAction("Index"); // Chuyển hướng về danh sách Dentist
                }

                // Nếu ModelState không hợp lệ, trả lại view với model để người dùng sửa lại thông tin
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin."); // Thêm lỗi tổng quát
                ViewBag.falid = new SelectList(db.Faculties, "id", "name");
                return View(model);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        // GET: admin/Dentists/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the dentist record
            Dentist dentist = db.Dentists.Find(id);
            if (dentist == null)
            {
                return HttpNotFound();
            }

            // Create a view model that includes the dentist data
            var viewModel = new CreateDentistViewModel
            {
                dentist = dentist,
                person = db.People.Find(dentist.id),
                account = db.Accounts.Find(dentist.id) // Adjust as necessary to map account data
  
        };

            // Populate the dropdown lists for faculty and person data
            ViewBag.falid = new SelectList(db.Faculties, "id", "name", dentist.falid);
            ViewBag.id = new SelectList(db.People, "id", "meta", dentist.id);

            return View(viewModel);
        }


        // POST: admin/Dentists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateDentistViewModel model, HttpPostedFileBase avatar)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Kiểm tra trùng số điện thoại
                    if (db.People.Any(p => p.phoneNumber == model.person.phoneNumber && p.id != model.dentist.id))
                    {
                        ModelState.AddModelError("person.phoneNumber", "Số điện thoại đã tồn tại.");
                        ViewBag.falid = new SelectList(db.Faculties, "id", "name", model.dentist.falid);
                        return View(model);
                    }

                    // Kiểm tra trùng email
                    if (db.People.Any(p => p.email == model.person.email && p.id != model.dentist.id))
                    {
                        ModelState.AddModelError("person.email", "Email đã tồn tại.");
                        ViewBag.falid = new SelectList(db.Faculties, "id", "name", model.dentist.falid);
                        return View(model);
                    }

                    // Cập nhật Dentist
                    var dentist = db.Dentists.Find(model.dentist.id);
                    if (dentist == null)
                    {
                        return HttpNotFound();
                    }

                    dentist.title = model.dentist.title;
                    dentist.hide = model.dentist.hide;
                    dentist.meta = model.dentist.meta;
                    dentist.falid = model.dentist.falid;
                    dentist.descrip = model.dentist.descrip;
                    dentist.datebegin = DateTime.UtcNow;
                    dentist.new_order = model.dentist.new_order;

                    // Cập nhật Person
                    var person = db.People.Find(model.dentist.id);
                    if (person != null)
                    {
                        person.name = model.person.name;
                        person.phoneNumber = model.person.phoneNumber;
                        person.email = model.person.email;
                        person.salary = model.person.salary;
                        person.address = model.person.address;
                        person.gender = model.person.gender;
                        person.birthday = model.person.birthday;
                        person.nation = model.person.nation;

                        // Xử lý upload file avatar nếu có
                        if (avatar != null && avatar.ContentLength > 0)
                        {
                            string fileExtension = Path.GetExtension(avatar.FileName);
                            string folderPath = Server.MapPath($"~/Content/images/{person.id}/");

                            if (!Directory.Exists(folderPath))
                                Directory.CreateDirectory(folderPath);

                            string avatarPath = $"/Content/images/{person.id}/avatar{fileExtension}";
                            avatar.SaveAs(Path.Combine(folderPath, $"avatar{fileExtension}"));
                            person.img = avatarPath; // Cập nhật đường dẫn avatar mới
                        }
                    }

                    var account = db.Accounts.Find(model.dentist.id);
                    if (account != null)
                    {
                        account.able = model.account.able;
                    }

                    db.SaveChanges(); // Lưu tất cả thay đổi vào database
                    return RedirectToAction("Index");
                }

                // Nếu ModelState không hợp lệ
                ViewBag.falid = new SelectList(db.Faculties, "id", "name", model.dentist.falid);
                return View(model);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}");
                    }
                }
                System.Diagnostics.Debug.WriteLine("Full exception: " + e);

                throw;
            }
        }


        // GET: admin/Dentists/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dentist dentist = db.Dentists.Find(id);
            if (dentist == null)
            {
                return HttpNotFound();
            }
            return View(dentist);
        }

        // POST: admin/Dentists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Dentist dentist = db.Dentists.Find(id);
            Person person = db.People.Find(id);
            Account account = db.Accounts.Find(id);
            db.Dentists.Remove(dentist);
            db.People.Remove(person);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private int getMaxOrder()
        {
            int lastOrder = db.Dentists
                               .OrderByDescending(n => n.order)
                               .Select(n => n.order)
                               .FirstOrDefault();
            return lastOrder + 1;
        }
    }
}
