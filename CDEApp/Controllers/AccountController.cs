using CDEApp.Models;
using CDEApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDEApp.Controllers
{
    public class AccountController : Controller
    {
        #region Class fields

        private readonly UserManager<User> _userManager; //Users control service
        private readonly SignInManager<User> _signInManager; // Authentication user service
        #endregion
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Methods
        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        //Register new User
        public async Task<IActionResult> Register(RegisterViewModel model) //Get view model with information about user from register view form
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email }; //Creating user
                var result = await _userManager.CreateAsync(user, model.Password); //Add user
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false); //Setting authentication cookie files
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        #endregion

        #endregion
    }
}
