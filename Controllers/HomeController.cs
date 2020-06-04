using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PA.Controllers
{
    //[Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            System.Console.WriteLine("Invoked home!");
            return View();
        }

        public IActionResult Products()
        {
            System.Console.WriteLine("Invoked products!");
            return View();
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            //todo json
            return Program.database.Users;
        }

        [HttpGet]
        public string wtfman()
        {
            return "SIKE";
        }
    }
}