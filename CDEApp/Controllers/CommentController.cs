using CDEApp.Models;
using CDEApp.Models.DataAccessLayer;
using CDEApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CDEApp.Controllers
{
    public class CommentController : Controller
    {
        #region Class fields

        ApplicationContext _context;
        #endregion
        public CommentController(ApplicationContext context)
        {
            _context = context;
        }

        #region Methods
        #region AddComment
        [HttpPost]
        //Create new document
        public IActionResult AddComment(AddCommentViewModel model)
        {
            var b = _context.Documents.Include(d => d.Comments).Where(i => i.Id == model.DocumentId).FirstOrDefault();
            User user = _context.Users.Include(u => u.Projects).ToList().                       //Get User from DB 
                Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            Comment comment = new Comment { User = user, Date = DateTime.Now, Document = b, DocumentId = b.Id, Text = model.CommentText }; //new comment with information from model
            b.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #endregion
    }
}
