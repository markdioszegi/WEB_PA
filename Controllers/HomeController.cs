using PA.Services;
using PA.Models;
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

namespace PA.Controllers
{
    //[Route("[controller]")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductsService _productsService;
        private readonly IUsersService _usersService;

        public HomeController(ILogger<HomeController> logger, IProductsService productsService, IUsersService usersService)
        {
            _logger = logger;
            _productsService = productsService;
            _usersService = usersService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            System.Console.WriteLine("Invoked home!");
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ListProducts()
        {
            System.Console.WriteLine("Invoked products!");

            return View("Products", _productsService.GetAll());
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ViewProduct(int id)
        {
            return View("Product", _productsService.GetOne(id));
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            //todo json
            return null;
        }

        [Authorize(Roles = "admin")]
        public IActionResult AddProduct()
        {
            return View();
        }
    }
}