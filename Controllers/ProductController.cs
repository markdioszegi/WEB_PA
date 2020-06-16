using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PA.Models;
using PA.Services;

namespace PA.Controllers
{
    public class ProductController : Controller
    {
        readonly ILogger<ProductController> _logger;
        readonly IProductsService _productsService;
        public ProductController(ILogger<ProductController> logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public IActionResult Index()
        {
            System.Console.WriteLine("Invoked products!");
            return View(_productsService.GetAll());
        }

        public IActionResult ShowProduct(string name)
        {
            return View(_productsService.GetOne(name));
        }
    }
}