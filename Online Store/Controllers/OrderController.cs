using Microsoft.AspNetCore.Mvc;
using Online_Store.Data;
using Online_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Online_Store.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BuyOrder(decimal totalPrice)
        {
            // You can retrieve necessary data from the database or other sources here if needed
            var order = _context.Orders;
            var payment = _context.Payments;
            var productCart = _context.ProductCarts;

            ViewBag.Order = order;
            ViewBag.Payment = payment;
            ViewBag.ProductCart = productCart;
            ViewBag.TotalPrice = totalPrice; // Pass totalPrice to the view
            return View();
        }


        [HttpPost]
        public IActionResult BuyOrder(IFormCollection form, Order order, decimal totalPrice)
        {
            // Retrieve user data from Form Collection
            var user = new User
            {
                Phone = form["user.Phone"],
                Email = form["email"],
                Address = form["user.Address"],
                CreditCardNumber = form["user.CreditCardNumber"],
                NationalID = form["user.NationalID"]
            };

            string email = form["email"];
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);

            if (existingUser != null)
            {
                // Update existing user information
                existingUser.Phone = user.Phone;
                existingUser.Address = user.Address;
                existingUser.CreditCardNumber = user.CreditCardNumber;
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            order.OrderDate = DateTime.Now;
            order.UserID = existingUser.UserID;
            order.TotalPrice = totalPrice; // Assuming you have a TotalPrice field in the Order model

            _context.Add(order);
            _context.SaveChanges();

            List<int> lsUserId = new List<int> { Convert.ToInt32(existingUser.UserID) };
            List<int> lsProCart = new List<int>();

            var itemsToRemoveFromCart = _context.Carts.Where(item => lsUserId.Contains(item.UserId)).ToList();
            var cartIdsToRemove = itemsToRemoveFromCart.Select(cart => cart.CartId).ToList();

            // Step 2: Find the items in the ProductCarts table with the specified Cart IDs
            var itemsToRemoveFromProductCart = _context.ProductCarts
                                                        .Where(productCart => cartIdsToRemove.Contains(productCart.CartId))
                                                        .ToList();

            if (itemsToRemoveFromCart.Any())
            {
                _context.Carts.RemoveRange(itemsToRemoveFromCart);
            }

            // Remove items from ProductCarts table
            if (itemsToRemoveFromProductCart.Any())
            {
                _context.ProductCarts.RemoveRange(itemsToRemoveFromProductCart);
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }



        public IActionResult OrderDetails()
        {
            var orders = _context.Orders.ToList();  // Retrieve all orders
            var user = _context.Users.ToList();
            var payment = _context.Payments.ToList();

            ViewBag.Orders = orders;  // Pass orders to ViewBag
            ViewBag.Users = user;
            ViewBag.Payment = payment;


            return View();
        }


    }
}
