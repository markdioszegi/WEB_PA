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
using PA.Helpers;

namespace PA.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUsersService _usersService;
        private readonly IOrdersService _ordersService;
        private readonly IProductsService _productsService;

        public CartController(ILogger<AccountController> logger, IUsersService usersService, IOrdersService ordersService, IProductsService productsService)
        {
            _logger = logger;
            _usersService = usersService;
            _ordersService = ordersService;
            _productsService = productsService;
        }

        public IActionResult Index()
        {
            ActiveOrderModel activeOrder = new ActiveOrderModel();
            activeOrder.Order = _ordersService.GetActive(ContextHelper.GetCurrentUser(HttpContext).Id);
            //get products
            activeOrder.Products = _ordersService.GetProductsByOrderId(activeOrder.Order.Id);
            return View(activeOrder);
        }

        public IActionResult PreviousOrders()
        {
            return View(_ordersService.GetAll(ContextHelper.GetCurrentUser(HttpContext).Id));
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.OrderId = _ordersService.GetActive(ContextHelper.GetCurrentUser(HttpContext).Id).Id;
            orderDetail.ProductId = productId;
            orderDetail.Quantity = quantity;
            _logger.LogInformation("Quantity: " + quantity + ", Active order ID: " + orderDetail.OrderId);
            Product product = _productsService.GetOne(productId);
            _logger.LogInformation("This added: " + product.Name);
            try
            {
                _ordersService.AddProductToOrder(orderDetail);
                return Json(new { text = "Successfully added the item to the cart" });
            }
            catch (Exception)
            {
                return Json(new { text = "Item already in cart!" });
                //throw;
            }
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            int orderId = _ordersService.GetActive(ContextHelper.GetCurrentUser(HttpContext).Id).Id;
            _ordersService.RemoveProductFromOrder(orderId, productId);
            return Json(new { text = "deleted " + productId });
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            int userId = ContextHelper.GetCurrentUser(HttpContext).Id;
            int orderId = _ordersService.GetActive(userId).Id;
            List<Product> products = _ordersService.GetProductsByOrderId(orderId);
            //check if the cart is empty
            if (!products.Any())
            {
                return RedirectToAction("Index");
            }
            // lower stock number
            foreach (var product in products)
            {
                ProductModel productModel = new ProductModel();
                productModel.Name = product.Name;
                productModel.Category = product.Category;
                productModel.Description = product.Description;
                productModel.Price = product.Price;
                productModel.Stock = product.Stock - product.Quantity;
                _productsService.Update(product.Id, productModel);
                System.Console.WriteLine("updated");
            }
            //remove active order
            _ordersService.RemoveActiveOrder(userId);
            //create new active order
            _ordersService.CreateOrder(userId);
            return View();
        }
    }
}