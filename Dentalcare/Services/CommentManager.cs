using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dentalcare.Services
{
    public class CommentManager
    {
        private readonly clinicEntities db;
        public class CommentInfo
        {
            public string CommentId { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public string Image { get; set; }
            public string Meta { get; set; }
            public string PersonName { get; set; }
            public DateTime DateBegin { get; set; }
        }
        public CommentManager()
        {
            db = new clinicEntities();
        }

        public List<Comment> GetAllComments()
        {
            return db.Comments
                     .Where(f => f.hide == false)
                     .OrderBy(f => f.order)
                     .ToList();
        }
        public List<CommentInfo> GetAllCommentInfo()
        {
            var query = from comment in db.Comments
                        join patient in db.Patients on comment.patid equals patient.id
                        join person in db.People on patient.id equals person.id
                        where comment.hide == false
                        orderby comment.order
                        select new CommentInfo
                        {
                            CommentId = comment.id,
                            Title = comment.title,
                            Message = comment.msg,
                            Image = comment.img,
                            Meta = comment.meta,
                            PersonName = person.name,
                            DateBegin = comment.datebegin
                        };

            return query.ToList();
        }
    }
}