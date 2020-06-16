using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PA.Models;
using PA.Services;

namespace PA.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        readonly ILogger<ProductController> _logger;
        public readonly IProductsService _productsService;
        public AdminController(ILogger<ProductController> logger, IProductsService productsService)
        {
            _logger = logger;
            _productsService = productsService;
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(ProductModel product)
        {
            try
            {
                _productsService.Add(product);
            }
            catch (ProductAlreadyExists)
            {
                ModelState.AddModelError("AlreadyExists", "Product already exists!");
                return View();
            }
            return View();
        }

        public IActionResult UpdateProduct()
        {
            ProductModel productModel = new ProductModel();
            productModel.Products = _productsService.GetAll();
            return View(productModel);
        }

        [HttpPost]
        public IActionResult UpdateProduct(ProductModel productModel)
        {
            string productName = Request.Form["ddlUpdateProduct"].ToString();
            _productsService.Update(_productsService.GetOne(productName).Id, productModel);
            return RedirectToAction();
        }

        public IActionResult RemoveProduct()
        {
            ProductModel productModel = new ProductModel();
            productModel.Products = _productsService.GetAll();
            return View(productModel);
        }

        [HttpPost]
        public IActionResult RemoveProduct(ProductModel productModel)
        {
            string productName = Request.Form["ddlUpdateProduct"].ToString();
            _productsService.Remove(_productsService.GetOne(productName).Id);
            return RedirectToAction();
        }
    }
}