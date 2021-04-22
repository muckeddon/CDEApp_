using CDEApp.Models;
using CDEApp.Models.DataAccessLayer;
using CDEApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CDEApp.Controllers
{
    public class HomeController : Controller
    {
        #region Class fields
        private readonly ILogger<HomeController> _logger;
        ApplicationContext _context;
        #endregion
        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        #region Methods
        #region Index
        public IActionResult Index()
        {
            List<Project> projects = new List<Project>();
            foreach (var u in _context.Users.Include(u => u.Projects).ToList()) //get projects from DB by current user name
            {
                if (u.UserName == HttpContext.User.Identity.Name)
                {
                    projects = u.Projects;
                }
            }
            return View(projects);
        }
        #endregion
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
