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
    public class DocumentController : Controller
    {
        #region Class fields

        ApplicationContext _context;
        #endregion
        public DocumentController(ApplicationContext context)
        {
            _context = context;
        }

        #region Methods

        #region Create
        [HttpGet]
        public IActionResult Create(int projectId)
        {
            return View(new AddDocumentViewModel { projectId = projectId });
        }

        [HttpPost]
        //Create new document
        public IActionResult Create(AddDocumentViewModel model)
        {
            User user = _context.Users.Include(u => u.Projects).ToList().                       //Get User from DB 
                Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            Project project = _context.Projects.Include(u => u.Documents).ToList().             //Get Project from DB
                Where(i => i.Id == model.projectId).FirstOrDefault();
            Document document = new Document();
            Comment comment = new Comment();

            if (model.Document != null)
            {
                byte[] data = null;
                using (var reader = new BinaryReader(model.Document.OpenReadStream())) //Open stream
                {
                    data = reader.ReadBytes((int)model.Document.Length);
                }

                document = new Document { Project = project, Name = model.Name, ProjectId = project.Id, Subject = data, Format = new FileFormatGetter().GetFileFormat(model) };
                comment = new Comment { Text = model.Comment, Document = document, DocumentId = document.Id, User = user, Date = DateTime.Now };
            }

            _context.Comments.Add(comment);   //add new records to tables
            _context.Documents.Add(document);
            _context.SaveChanges();           //save db
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region Download
        [HttpGet]
        public IActionResult Download(int documentId, string documentFormat, string documentName)
        {
            byte[] subject = new byte[1];
            foreach (var a in _context.Set<Document>()) //Get byte array from db
            {
                if (a.Id == documentId)
                    subject = a.Subject;
            }

            return new FileContentResult(subject, "image/png") //Return file to user
            {
                FileDownloadName = $"{documentName}{documentFormat}"
            };
        }
        #endregion
        #endregion
    }
}
