using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Store.Data;
using Online_Store.Models;
using System.Security.Cryptography;
using System.Text;
namespace Online_Store.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Users'  is null.");
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,userName,Email,Password,ConfirmPassword,Gender")] User user)
        {
            user.NationalID = "0";
            user.Address = "NULL";
            user.CreditCardNumber = "0";
            user.Phone = "0";
            if (user.Password == null || user.ConfirmPassword == null || !user.Password.SequenceEqual(user.ConfirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password do not match.");
                return View(user);
            }

            // Hash the password
            user.Password = HashPassword(user.Password);

            // Clear ConfirmPassword for security reasons
            user.ConfirmPassword = null;
            var email = from us in _context.Users where us.Email == user.Email select user.Email;
            if ((email.Any()))
            {
                ViewBag.Title = "invalid";
                return View();
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(LogIn));
            //if (user.Password != user.ConfirmPassword)
            //{
            //    ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password do not match.");
            //    return View(user);
            //}
            //user.Password = HashPassword(user.Password);
            //user.ConfirmPassword = HashPassword(user.ConfirmPassword);
            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(LogIn));

        }

        private byte[] HashPassword(byte[] password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(password);
            }
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LogIn(User user)
        {
            // Authenticate user against your data store
            var re = AuthenticateUser(user.Email, user.Password);
            if (re != null)
            {
         
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid email or password");
            }

            return View(user);

        }
        public User AuthenticateUser(string email, byte[] password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null && VerifyPassword(password, user.Password))
            {
                // Authentication successful
                HttpContext.Session.SetInt32("UserID", user.UserID);
                return user;
            }
            return null;
        }
        private bool VerifyPassword(byte[] enteredPassword, byte[] hashedPassword)
        {
            if (enteredPassword == null || hashedPassword == null)
            {
                return false; // Password or hashed password is null, cannot verify
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] enteredHashedBytes = sha256.ComputeHash(enteredPassword);
                return enteredHashedBytes.SequenceEqual(hashedPassword);
            }
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,NationalID,FirstName,LastName,Phone,userName,Email,Password,ConfirmPassword,Address,CreditCardNumber,Age")] User user)
        {

            if (id != user.UserID)
            {
                return NotFound();
            }

            try
            {
                // Retrieve the existing user from the database
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Update properties of the existing user
                existingUser.NationalID = user.NationalID;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Phone = user.Phone;
                existingUser.userName = user.userName;
                existingUser.Email = user.Email;
                existingUser.Address = user.Address;
                existingUser.CreditCardNumber = user.CreditCardNumber;

                // Check if a new password is provided
                if (user.Password != null && user.Password.Length > 0)
                {
                    existingUser.Password = user.Password; // Update password
                }

                _context.Update(existingUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.UserID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index", "Home");
            //return View();
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserID == id)).GetValueOrDefault();
        }
    }
}
