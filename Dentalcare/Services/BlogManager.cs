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

        public List<NEWS> GetAllBlogs()
        {
            return db.NEWS
                     .Where(f => f.hide == false)
                     .OrderBy(f => f.order)
                     .ToList();

        }
        public NEWS GetBlogByMeta(string blogMeta)
        {
            var blog = db.NEWS.FirstOrDefault(b => b.meta == blogMeta && b.hide == false);
            return blog ?? new NEWS();
        }

    }
}