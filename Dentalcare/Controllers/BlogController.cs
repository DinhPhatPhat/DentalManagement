using Dentalcare.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dentalcare.Controllers
{
    public class BlogController : BaseController
    {
        // GET: Blog
        private readonly BlogManager blogManager;

        public BlogController()
        {
            blogManager = new BlogManager();
        }

        //Lấy dữ liệu của blog để tải lên View của Blog
        public ActionResult Index()
        {
            var blogs = blogManager.GetAllBlogs();
            ViewBag.Blogs = blogs;
            return View();
        }

        public ActionResult Details(string meta)
        {
            //Kiểm tra blog có meta không, nếu blog null thì trả về trang Page Not Found.
            //Ngược lại thì ta hiển thị View Blog
            var blog = blogManager.GetBlogByMeta(meta);
            if (blog == null)
            {
                return RedirectToAction("_404", "Default");
            }

            ViewBag.blog = blog;
            return View();
        }

    }
}