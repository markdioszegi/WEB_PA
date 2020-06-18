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

        public HomeController(ILogger<HomeController> logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Visited home");
            return View();
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            //todo json
            return null;
        }
    }
}