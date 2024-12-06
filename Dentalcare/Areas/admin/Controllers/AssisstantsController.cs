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

namespace Dentalcare.Areas.admin.Controllers
{
    public class AssisstantsController : Controller
    {
        private clinicEntities db = new clinicEntities();

        // GET: admin/Assisstants
        public ActionResult Index()
        {
            var assisstants = db.Assisstants
                             .Include(d => d.Person)
                             .ToList();
            // Chuyển đổi thành danh sách ViewModel
            var assisstantViewModels = assisstants.Select(d => new CreateAssisstantViewModel
            {
                assisstant = d,
                person = d.Person,
                account = db.Accounts.FirstOrDefault(a => a.id == d.Person.id)
            }).ToList();
            return View(assisstantViewModels);
        }

        // GET: admin/Assisstants/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assisstant assisstant = db.Assisstants.Find(id);
            if (assisstant == null)
            {
                return HttpNotFound();
            }
            return View(assisstant);
        }

        // GET: admin/Assisstants/Create
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

        // POST: Assisstant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateAssisstantViewModel model, HttpPostedFileBase avatar)
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
                        password = "123", // Sử dụng mật khẩu mặc định là "123"
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
                        salary = model.person.salary,
                        address = model.person.address,
                        gender = model.person.gender,
                        birthday = model.person.birthday,
                        nation = model.person.nation,
                        img = avatarPath,
                        meta = model.assisstant.meta,
                        role = 4,
                        hide = false,
                        datebegin = DateTime.UtcNow
                    };

                    // Tạo mới Assisstant
                    var assisstant = new Assisstant
                    {
                        id = newAccountId,
                        hide = model.assisstant.hide,
                        meta = model.assisstant.meta,
                        datebegin = DateTime.UtcNow,
                        new_order = getMaxOrder()
                    };

                    // Lưu các đối tượng vào cơ sở dữ liệu
                    db.Accounts.Add(account);
                    db.People.Add(person);
                    db.Assisstants.Add(assisstant);
                    db.SaveChanges();

                    return RedirectToAction("Index"); // Chuyển hướng về danh sách Assisstant
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

        // GET: admin/Assisstants/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assisstant assisstant = db.Assisstants.Find(id);
            if (assisstant == null)
            {
                return HttpNotFound();
            }

            var viewModel = new CreateAssisstantViewModel
            {
                assisstant = assisstant,
                person = db.People.Find(assisstant.id),
                account = db.Accounts.Find(assisstant.id)

            };

            ViewBag.id = new SelectList(db.People, "id", "meta", assisstant.id);
            return View(viewModel);
        }

        // POST: admin/Assisstants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateAssisstantViewModel model, HttpPostedFileBase avatar)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Kiểm tra trùng số điện thoại
                    if (db.People.Any(p => p.phoneNumber == model.person.phoneNumber && p.id != model.assisstant.id))
                    {
                        ModelState.AddModelError("person.phoneNumber", "Số điện thoại đã tồn tại.");
                        return View(model);
                    }

                    // Kiểm tra trùng email
                    if (db.People.Any(p => p.email == model.person.email && p.id != model.assisstant.id))
                    {
                        ModelState.AddModelError("person.email", "Email đã tồn tại.");
                        return View(model);
                    }

                    // Cập nhật Assisstant
                    var assisstant = db.Assisstants.Find(model.assisstant.id);
                    if (assisstant == null)
                    {
                        return HttpNotFound();
                    }

                    assisstant.hide = model.assisstant.hide;
                    assisstant.meta = model.assisstant.meta;
                    assisstant.datebegin = DateTime.UtcNow;
                    assisstant.new_order = model.assisstant.new_order;

                    // Cập nhật Person
                    var person = db.People.Find(model.assisstant.id);
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

                    var account = db.Accounts.Find(model.assisstant.id);
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

        // GET: admin/Assisstants/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assisstant assisstant = db.Assisstants.Find(id);
            if (assisstant == null)
            {
                return HttpNotFound();
            }
            return View(assisstant);
        }

        // POST: admin/Assisstants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Assisstant assisstant = db.Assisstants.Find(id);
            db.Assisstants.Remove(assisstant);
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
            int lastOrder = db.Assisstants
                               .OrderByDescending(n => n.order)
                               .Select(n => n.order)
                               .FirstOrDefault();
            return lastOrder + 1;
        }

    }
}
