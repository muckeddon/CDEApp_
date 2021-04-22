using CDEApp.Models;
using CDEApp.Models.DataAccessLayer;
using CDEApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDEApp.Controllers
{
    public class ProjectController : Controller
    {
        #region Class fields

        ApplicationContext _context;
        #endregion
        public ProjectController(ApplicationContext context)
        {
            _context = context;
        }

        #region Methods
        #region Create project

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //Create new Project method
        public IActionResult Create(AddProjectViewModel model)
        {
            User user = _context.Users.Include(u => u.Projects).ToList().
                                       Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault(); //Get User from DB
            Project project = new Project { Name = model.Name };
            Document document = new Document();
            Comment comment = new Comment();
            if (model.Document != null)
            {
                byte[] data = null; //create byte array for file bytes
                using (var reader = new BinaryReader(model.Document.OpenReadStream())) //open stream
                {
                    data = reader.ReadBytes((int)model.Document.Length);
                }

                project = new Project { Name = model.Name, Admin = HttpContext.User.Identity.Name, IsClose = false }; //add properties to project
                document = new Document //add properties to document
                {
                    Project = project,
                    Name = model.DocumentName,
                    ProjectId = project.Id,
                    Subject = data,
                    Format = new FileFormatGetter().GetFileFormat(model)
                };
                comment = new Comment { Text = model.Comment, Document = document, DocumentId = document.Id, User = user, Date = DateTime.Now }; //add properties to comment
            }
            project.Users.Add(user);
            user.Projects.Add(project);
            _context.Comments.Add(comment); //add entities in DB and save db context
            _context.Documents.Add(document);
            _context.Projects.Add(project);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #endregion
    }
}
