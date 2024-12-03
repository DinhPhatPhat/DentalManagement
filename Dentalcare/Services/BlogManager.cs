using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dentalcare.Services
{
    public class BlogManager
    {
        private readonly clinicEntities db;
        public BlogManager()
        {
            db = new clinicEntities();
        }

        //Lấy dữ liệu tin tức
        public List<NEWS> GetAllBlogs()
        {
            return db.NEWS
                     .Where(f => f.hide == false)
                     .OrderBy(f => f.new_order)
                     .ToList();

        }

        //Lấy tin tức bằng trường meta
        public NEWS GetBlogByMeta(string blogMeta)
        {
            var blog = db.NEWS.FirstOrDefault(b => b.meta == blogMeta && b.hide == false);
            return blog ?? new NEWS();
        }

    }
}