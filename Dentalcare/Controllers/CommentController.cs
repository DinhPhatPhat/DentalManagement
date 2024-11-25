using Dentalcare.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dentalcare.Controllers
{
    public class CommentController : BaseController
    {
        // GET: Comment
        private readonly CommentManager commentManager;

        public CommentController()
        {
            commentManager = new CommentManager();
        }
        //Lấy dữ liệu của comment cho Vỉew Comment
        public ActionResult Index()
        {
            var comments = commentManager.GetAllCommentInfo();
            ViewBag.Comments = comments;
            return View();
        }

    }
}