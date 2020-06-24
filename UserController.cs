using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using TullymurrySystem.Data.Services;
using TullymurrySystem.Data.Models;
using TullymurrySystem.Web.ViewModels;

namespace TullymurrySystem.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly TullymurryDataService _svc;

        public UserController()
        {
            _svc = new TullymurryDataService();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")]UserViewModel m)
        {        
            // call service to locate user
            var user = _svc.GetUserByCredentials(m.Username, m.Password);
            if (user == null)
            {
                ModelState.AddModelError("Username", "Invalid Login Credentials");
                ModelState.AddModelError("Password", "Invalid Login Credentials");
                return View(m);
            }
           
            // sign user in using cookie authentication to store principal
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                BuildClaimsPrincipal(user)
            );

            return RedirectToAction("Index","Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Password,PasswordConfirm,Role")]UserViewModel m)
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            var user = _svc.RegisterUser(m.Username, m.Password, m.Role);               

            // check if username is unique
            if (user == null )
            {
                ModelState.AddModelError("Username", "Username has already been used. Choose another");
                return View(m);
            }

            // sign user in using cookie authentication to store principal
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                BuildClaimsPrincipal(user)
            );

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult ErrorNotAuthorised()
        {   
            Alert("Not Authorized", AlertType.warning);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorNotAuthenticated()
        {
            Alert("Not Authenticated", AlertType.warning);
            return RedirectToAction("Login", "User"); 
        }

        // Build a claims principal from authenticated user
        private  ClaimsPrincipal BuildClaimsPrincipal(User user)
        { 
            // define user claims
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())                              
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            // build principal using claims
            return  new ClaimsPrincipal(claims);
        }


        // this action can be used in a remote validation to verify that
        // the username on registration is unique
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyUsername(string username)
        {
            if (_svc.GetUserByName(username) != null)
            {
                return Json($"Username {username} is already in use. Please choose another");
            }
            return Json(true);
        }

    }
}
