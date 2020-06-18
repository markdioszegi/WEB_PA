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
        private readonly IOrdersService _ordersService;

        public AccountController(ILogger<AccountController> logger, IUsersService usersService, IOrdersService ordersService)
        {
            _logger = logger;
            _usersService = usersService;
            _ordersService = ordersService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Account visited");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(AccountModel account)
        {
            try
            {
                _usersService.Register(account.RegisterModel.Username, account.RegisterModel.Password, account.RegisterModel.Email);
                User currentUser = _usersService.GetOne(account.RegisterModel.Username);
                _ordersService.CreateOrder(currentUser.Id);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("AccountError", "This username is already taken!");
                return View("Index");
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
                //If the user does not exist in the DB or the credentials were wrong
                ModelState.AddModelError("AccountError", "Wrong username or password!");
                return View("Index");
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
                //System.Console.WriteLine("Logged in as: " + user.ToString());
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
            _logger.LogInformation(String.Join(", ", HttpContext.User.Claims));
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (returnUrl == null)
            {
                return LocalRedirect("/");
            }
            return LocalRedirect("/");
        }

        public IActionResult Profile()
        {
            return View(_usersService.GetAll());
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}