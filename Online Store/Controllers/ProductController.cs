using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Online_Store.Data;
using Online_Store.Models;
 
namespace Online_Store.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _context = context;
            _environment = (IWebHostEnvironment?)environment;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return _context.products != null ?
                        View(await _context.products.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.products'  is null.");
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,Description,Price,Category,clientFile,Stock,Weight,Brand")] Product product)
        {


            string fileName = string.Empty;
            if (product.clientFile != null)
            {
                string myUpload = Path.Combine(_environment.WebRootPath, "Image");
                fileName = product.clientFile.FileName;
                string extension = Path.GetExtension(fileName).ToLower();
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                {
                    string fullPath = Path.Combine(myUpload, fileName);
                    product.clientFile.CopyTo(new FileStream(fullPath, FileMode.Create));
                    product.Image = fileName;
                }
                else
                {
                    return View();
                }
            }

            _context.products.Add(product);
            _context.SaveChanges();
            TempData["successData"] = "Item has been added successfully";
            if (product.Category == Category.Clothes)
            {
                return RedirectToAction("ClothesIndex"); // توجيه المستخدم إلى صفحة الملابس
            }
            else if (product.Category == Category.Electronics)
            {
                return RedirectToAction("ElectronicsIndex");
            }
            else if (product.Category == Category.Books)
            {
                return RedirectToAction("BooksIndex");
            }
            else if (product.Category == Category.Furnitures)
            {
                return RedirectToAction("FurnituresIndex");
            }
            else if (product.Category == Category.Foods)
            {
                return RedirectToAction("FoodIndex");
            }
            return RedirectToAction("Index");
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Description,Price,Category,clientFile,Image,Stock,Weight,Brand")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }


            try
            {
                if (product.clientFile != null)
                {
                    string myUpload = Path.Combine(_environment.WebRootPath, "Image");
                    string fileName = product.clientFile.FileName;
                    string extension = Path.GetExtension(fileName).ToLower();

                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {
                        string fullPath = Path.Combine(myUpload, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await product.clientFile.CopyToAsync(stream);
                        }
                        product.Image = fileName;
                    }
                    else
                    {
                        ModelState.AddModelError("clientFile", "The file should be an image with one of the following extensions: .jpg, .jpeg, .png, .gif");
                        return View(product);
                    }
                }
                else
                {
                    // Keep the existing image if no new file is uploaded
                    var existingProduct = await _context.products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
                    if (existingProduct != null)
                    {
                        product.Image = existingProduct.Image;
                    }
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));


            return View(product);
        }



        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.products == null)
            {
                return NotFound();
            }

            var product = await _context.products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.products'  is null.");
            }
            var product = await _context.products.FindAsync(id);
            if (product != null)
            {
                _context.products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
        public IActionResult ClothesIndex()
        {
            ViewBag.Model =_context.products.Where(e=>e.Category==Category.Clothes).ToList();
            return View();
        }
        public IActionResult FurnituresIndex()
        {
            ViewBag.Model = _context.products.Where(W => W.Category== Category.Furnitures).ToList();
            return View();
        }
        public IActionResult FoodIndex()
        {
            ViewBag.Model = _context.products.Where(W => W.Category == Category.Foods).ToList();
            return View();
        }
        public IActionResult ElectronicsIndex()
        {
            ViewBag.Model = _context.products.Where(W => W.Category == Category.Electronics).ToList();
            return View();
        }
        public IActionResult BooksIndex()
        {
            ViewBag.Model = _context.products.Where(W => W.Category == Category.Books).ToList();
            return View();
        }

       


    }
}
