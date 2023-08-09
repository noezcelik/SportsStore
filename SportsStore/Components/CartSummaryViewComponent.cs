using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Components {
    public class CartSummaryViewComponent : ViewComponent {
        public IViewComponentResult Invoke() {
            Cart cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            return View(cart);
        }
    }
}
