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
        public ActionResult Index()
        {
            var blogs = blogManager.GetAllBlogs();
            ViewBag.Blogs = blogs;
            return View();
        }

        public ActionResult Details(string meta)
        {
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