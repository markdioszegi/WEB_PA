using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using PA.Services;
using PA.Models;
using System.Linq;

namespace PA.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUsersService _usersService;

        public AccountController(ILogger<AccountController> logger, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            System.Console.WriteLine("Account page visited");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(AccountModel account)
        {
            System.Console.WriteLine(account.RegisterModel.Username);
            try
            {
                _usersService.Register(account.RegisterModel.Username, account.RegisterModel.Password, account.RegisterModel.Email);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Username", "Already taken.");
                //return View("Account");
            }
            return View("Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(AccountModel account, string returnUrl)
        {
            User user = _usersService.Login(account.LoginModel.Username, account.LoginModel.Password);
            if (user == null)
            {
                //If the user does not exist in the DB
                return RedirectToAction("Index");
            }
            else
            {
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Username", user.Username),
                        new Claim(ClaimTypes.Role, user.Role),
                    }, CookieAuthenticationDefaults.AuthenticationScheme)),
                    new AuthenticationProperties());
                System.Console.WriteLine("Logged in as: " + user.ToString());
                if (returnUrl == null)
                {
                    return LocalRedirect("/");
                }
                return LocalRedirect(returnUrl);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        public async Task<IActionResult> LogoutAsync(string returnUrl)
        {
            /* var claims = HttpContext.User.Claims;
            _logger.LogInformation(String.Join(", ", claims)); */
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (returnUrl == null)
            {
                return LocalRedirect("/");
            }
            return LocalRedirect("/");
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}