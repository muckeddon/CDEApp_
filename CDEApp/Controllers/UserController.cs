using CDEApp.Models;
using CDEApp.Models.DataAccessLayer;
using CDEApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CDEApp.Controllers
{
    public class UserController : Controller
    {
        #region Class fields

        ApplicationContext _context;
        #endregion
        public UserController(ApplicationContext context)
        {
            _context = context;
        }

        #region Methods
        #region AddUser
        [HttpGet]
        public IActionResult AddUser(int projectId)
        {
            return View(new AddUserViewModel { ProjectId = projectId });
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            var user = _context.Users.Where(u => u.Email == model.Email).FirstOrDefault(); //get user that will adding to project by Email from view model
            if (user is null)
            {
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync($"{model.Email}", "Вас приглашают присоедениться к проекту Collaborative Document Editing!", "[Ссылка на проект]");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var project = _context.Projects.Include(u => u.Users).Where(p => p.Id == model.ProjectId).FirstOrDefault(); // get current project
                project.Users.Add(user); //add user
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #endregion
    }
}
