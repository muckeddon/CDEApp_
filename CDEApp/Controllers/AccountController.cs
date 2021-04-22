using CDEApp.Models;
using CDEApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        #region Login

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl, ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        //Login User
        public async Task<IActionResult> Login(LoginViewModel model) //Get view model with login information
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false); //sign in by information from model
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home"); //redirection to home controller
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин/пароль");
                }
            }
            return View(model);
        }
        #endregion
        #region ExternalLogin

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null) // if remoteError is not null it means that we have error from external provider
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync(); //get all login information from external provider, like name of provider and claims
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, $"Error loading external login information: {remoteError}");

                return View("Login", loginViewModel);
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded) //checking of sign in
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email); // get email 

                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email); //try to get user by email

                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _userManager.CreateAsync(user); //if user is null, creating new user with information from provider(info)
                    }

                    await _userManager.AddLoginAsync(user, info); //user login
                    await _signInManager.SignInAsync(user, isPersistent: false); //user sign in

                    return LocalRedirect(returnUrl);
                }

                ViewBag.ErrorTitle = $"EMail claim not reveived from {info.LoginProvider}";

                return View("Error");
            }
        }
        #endregion
        #region Logout

        [HttpPost]
        [ValidateAntiForgeryToken]

        //Logout object
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion
        #region Redirect to create method of project controller

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProject()
        {
            return RedirectToAction("Create", "Project");
        }
        #endregion
        #endregion
    }
}
