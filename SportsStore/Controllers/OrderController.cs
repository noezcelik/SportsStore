using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers {
    public class OrderController : Controller {
        private IOrderRepository repo;
        public OrderController(IOrderRepository repo) {
            this.repo = repo;
        }
        [HttpGet]
        public IActionResult Checkout() {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(Order order) {
            Cart cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            if (cart.Lines.Count() == 0) {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid) {
                order.Lines = cart.Lines.ToArray();
                repo.SaveOrder(order);
                cart.Clear();
                HttpContext.Session.SetJson("cart", cart);
                return View("Completed", order.OrderID);
            }
            else {
                return View();
            }
        }
    }
}
