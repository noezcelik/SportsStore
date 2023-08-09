using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers {
    public class HomeController : Controller {
        private IStoreRepository repo;
        public int PageSize = 4;
        public HomeController(IStoreRepository repo) {
            this.repo = repo;
        }

        public IActionResult Index(string category, int productPage = 1) {
            ProductListViewModel plvm = new ProductListViewModel() {
                Products = repo.Products
                            .Where(p => category == null || p.Category == category)
                            .OrderBy(p => p.ProductID)
                            .Skip((productPage - 1) * PageSize)
                            .Take(PageSize),
                PagingInfo = new PagingInfo() {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = repo.Products
                                    .Where(p => category == null || p.Category == category)
                                    .Count()
                },
                CurrentCategory = category
            };
            return View(plvm);
        }
        [HttpGet]
        public IActionResult Cart(string returnUrl) {
            CartViewModel cvm = new CartViewModel() {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart(),
                ReturnUrl = returnUrl ?? "/"
            };
            return View(cvm);
        }
        [HttpPost]
        public IActionResult Cart(long productId, string returnUrl) {
            Cart cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            Product product = repo.Products
                                .FirstOrDefault(p => p.ProductID == productId);
            cart.AddItem(product, 1);
            HttpContext.Session.SetJson("cart", cart);
            return RedirectToAction(nameof(Cart), new { returnUrl });
        }
        public IActionResult RemoveLine(long productId, string returnUrl) {
            Cart cart = HttpContext.Session.GetJson<Cart>("cart");
            Product product = cart.Lines.First(l => l.Product.ProductID == productId).Product;
            cart.RemoveLine(product);
            HttpContext.Session.SetJson("cart", cart);
            return RedirectToAction(nameof(Cart), new { returnUrl });
        }
        public IActionResult IncreaseLine(long productId, string returnUrl) {
            Cart cart = HttpContext.Session.GetJson<Cart>("cart");
            CartLine line = cart.Lines
                                .First(l => l.Product.ProductID == productId);
            line.Quantity += 1;
            HttpContext.Session.SetJson("cart", cart);
            return RedirectToAction(nameof(Cart), new { returnUrl });
        }
        public IActionResult DecreaseLine(long productId, string returnUrl) {
            Cart cart = HttpContext.Session.GetJson<Cart>("cart");
            CartLine line = cart.Lines
                                .First(l => l.Product.ProductID == productId);
            line.Quantity -= (line.Quantity > 1 ? 1 : 0);
            HttpContext.Session.SetJson("cart", cart);
            return RedirectToAction(nameof(Cart), new { returnUrl });
        }
    }
}
