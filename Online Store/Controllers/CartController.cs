using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Online_Store.Data;
using Online_Store.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Humanizer;


namespace Online_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult AddToCart()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int id, string action)
        {
            try
            {
                // If the user doesn't have a cart, create a new one
                var userId = HttpContext.Session.GetInt32("UserID");
                if (userId == null)
                {
                    // Redirect to login or registration page if user is not authenticated
                    return RedirectToAction("Login", "User");
                }
                var cart = _context.Carts.Include(c => c.ProductCards)
                                                    .FirstOrDefault(c => c.UserId == Convert.ToInt32(userId));
                if (cart == null)
                {
                    // If the user doesn't have a cart, create a new one
                    cart = new Cart
                    {
                        UserId = Convert.ToInt32(userId),
                        // ProductCards = new List<ProductCart>()
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();

                }
                // Check if the product is already in the cart
                var productCart = _context.ProductCarts
                                          .FirstOrDefault(pc => pc.CartId == cart.CartId && pc.ProductId == id);

                if (productCart == null)
                {
                    // If the product is not in the cart, add it with quantity 1
                    productCart = new ProductCart
                    {
                        ProductId = id,
                        CartId = cart.CartId,
                        Quantity = 1
                    };
                    _context.ProductCarts.Add(productCart);
                }
                else
                {
                    // If the product is already in the cart, increment the quantity
                    productCart.Quantity += 1;
                    _context.ProductCarts.Update(productCart);
                }

                // Save changes to the database
                _context.SaveChanges();


                // Redirect to a specific action or view after adding to cart
                // For example, you can redirect to the cart index page
                ViewBag.Message = "Change succesfully";
                if (action != "Details") {
                    return RedirectToAction(action, "Product");
                }
                else {
                    return RedirectToAction("Details", "Product", new { id });
                }
 
            }
            catch (Exception ex)
            {
                // Handle exceptions if any
                // You may want to log the exception or show an error message to the user
                return RedirectToAction("Index", "Product");
 

            }
        }


       


    }
}
