using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Store.Data;
using Online_Store.Models;

namespace Online_Store.Controllers
{
    public class ProductCartController : Controller
    {
        private readonly ApplicationDbContext _context;


        public ProductCartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cart()
        {
            var orders = _context.Orders.ToList();  // Retrieve all orders
            var carts = _context.Carts.ToList();
            var products = _context.products.ToList();
            var productCarts = _context.ProductCarts.ToList();
            //var user = _context.Users.ToList();
            var userIde = HttpContext.Session.GetInt32("UserID");

            ViewBag.Orders = orders;  // Pass orders to ViewBag
            ViewBag.Carts = carts;
            ViewBag.Products = products;
            ViewBag.ProductCarts = productCarts;
            ViewBag.userId = userIde;
            return View();
        }
        [HttpPost]
        public JsonResult DeleteItem(int productId, int cartId)
        {
            try
            {
                var productCart = _context.ProductCarts.FirstOrDefault(pc => pc.ProductId == productId && pc.CartId == cartId);
                var cart = _context.Carts.FirstOrDefault(pc =>  pc.CartId == cartId);
                if (productCart != null)
                {
                    if (productCart.Quantity > 1)
                    {
                        productCart.Quantity -= 1;
                        _context.SaveChanges();
                    }
                    else
                    {
                        _context.ProductCarts.Remove(productCart);
                        _context.Carts.Remove(cart);
                        _context.SaveChanges();
                    }
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                // Log exception
                return Json(new { success = false });
            }
        }

    }
}
