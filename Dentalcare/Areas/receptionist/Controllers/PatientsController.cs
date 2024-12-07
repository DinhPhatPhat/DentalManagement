using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dentalcare.Areas.admin.Models;
using Dentalcare.Models;

namespace Dentalcare.Areas.receptionist.Controllers
{
    public class PatientsController : BaseController
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Patients
        public ActionResult Index()
        {
            var patients = db.Patients
                             .Include(d => d.Person)
                             .ToList();
            // Chuyển đổi thành danh sách ViewModel
            var patientViewModels = patients.Select(d => new CreatePatientViewModel
            {
                patient = d,
                person = d.Person,
                account = db.Accounts.FirstOrDefault(a => a.id == d.Person.id)
            }).ToList();
            return View(patientViewModels);
        }

        // GET: admin/Patients/Details/5

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }


        // GET: admin/Patients/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.People, "id", "meta");
            return View();
        }

        private string GenerateNewAccountId()
        {
            var lastAccountId = db.Accounts
                                 .OrderByDescending(a => a.id)
                                 .Select(a => a.id)
                                 .FirstOrDefault();

            if (string.IsNullOrEmpty(lastAccountId))
                return "AC00000001";

            int numberPart = int.Parse(lastAccountId.Substring(2));

            numberPart++;

            return $"AC{numberPart:D8}";
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePatientViewModel model, HttpPostedFileBase avatar)
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
                        return View(model);
                    }

                    // Kiểm tra trùng số điện thoại
                    if (db.People.Any(p => p.phoneNumber == model.person.phoneNumber))
                    {
                        ModelState.AddModelError("person.phoneNumber", "Số điện thoại đã tồn tại.");
                        return View(model);
                    }

                    // Kiểm tra trùng email
                    if (db.People.Any(p => p.email == model.person.email))
                    {
                        ModelState.AddModelError("person.email", "Email đã tồn tại.");
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


                    var person = new Person
                    {
                        id = newAccountId,
                        name = model.person.name,
                        phoneNumber = model.person.phoneNumber,
                        email = model.person.email,
                        salary = 0,
                        address = model.person.address,
                        gender = model.person.gender,
                        birthday = model.person.birthday,
                        nation = model.person.nation,
                        img = avatarPath,
                        meta = model.patient.meta,
                        role = 5,
                        hide = false,
                        datebegin = DateTime.UtcNow
                    };

                    // Tạo mới Patient
                    var patient = new Patient {
                        id = newAccountId,
                        hide = false,
                        meta = model.patient.meta,
                        datebegin = DateTime.UtcNow,
                        isVip = model.patient.isVip,
                    };

                    // Lưu các đối tượng vào cơ sở dữ liệu
                    db.Accounts.Add(account);
                    db.People.Add(person);
                    db.Patients.Add(patient);
                    db.SaveChanges();

                    return RedirectToAction("Index"); // Chuyển hướng về danh sách Patient
                }

                // Nếu ModelState không hợp lệ, trả lại view với model để người dùng sửa lại thông tin
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin."); // Thêm lỗi tổng quát
                return View(model);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        // GET: admin/Patients/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            var viewModel = new CreatePatientViewModel
            {
                patient = patient,
                person = db.People.Find(patient.id),
                account = db.Accounts.Find(patient.id)

            };

            ViewBag.id = new SelectList(db.People, "id", "meta", patient.id);
            return View(viewModel);
        }

        // POST: admin/Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreatePatientViewModel model, HttpPostedFileBase avatar)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Kiểm tra trùng số điện thoại
                    if (db.People.Any(p => p.phoneNumber == model.person.phoneNumber && p.id != model.patient.id))
                    {
                        ModelState.AddModelError("person.phoneNumber", "Số điện thoại đã tồn tại.");
                        return View(model);
                    }

                    // Kiểm tra trùng email
                    if (db.People.Any(p => p.email == model.person.email && p.id != model.patient.id))
                    {
                        ModelState.AddModelError("person.email", "Email đã tồn tại.");
                        return View(model);
                    }

                    // Cập nhật Assisstant
                    var patient = db.Patients.Find(model.patient.id);
                    if (patient == null)
                    {
                        return HttpNotFound();
                    }

                    patient.hide = model.patient.hide;
                    patient.meta = model.patient.meta;
                    patient.datebegin = DateTime.UtcNow;

                    // Cập nhật Person
                    var person = db.People.Find(model.patient.id);
                    if (person != null)
                    {
                        person.name = model.person.name;
                        person.phoneNumber = model.person.phoneNumber;
                        person.email = model.person.email;
                        person.salary = 0;
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

                    var account = db.Accounts.Find(model.patient.id);
                    if (account != null)
                    {
                        account.able = model.account.able;
                    }

                    db.SaveChanges(); // Lưu tất cả thay đổi vào database
                    return RedirectToAction("Index");
                }

                // Nếu ModelState không hợp lệ
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


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
