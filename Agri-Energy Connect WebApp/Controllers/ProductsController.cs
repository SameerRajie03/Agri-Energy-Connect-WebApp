using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect_WebApp.Data;
using Agri_Energy_Connect_WebApp.Models;
using Agri_Energy_Connect_WebApp.Workers;

namespace Agri_Energy_Connect_WebApp.Controllers
{
    public class ProductViewModel
    {
        public List<Product> Products { get; set; }
        public int? SelectedCategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<Category> Categories { get; set; }
    }

    public class ProductsController : Controller
    {
        private readonly Agri_Energy_Connect_WebAppContext _context;

        public ProductsController(Agri_Energy_Connect_WebAppContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int id)
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if (Validation.FarmerExistsId(GetSet.UserFarmer.ToString(), _context))
                {
                    var agri_Energy_Connect_WebAppContext = _context.Product.Include(p => p.Category);
                    var specificFarmer = agri_Energy_Connect_WebAppContext.Where(p => p.FarmerId == id);
                    return View(await specificFarmer.ToListAsync());
                }
                else
                {
                    TempData["ErrorMessage"] = "You do not have access to this page";
                    return RedirectToAction("IndexEmployee", "Home");
                }
            }
            
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if (Validation.FarmerExistsId(GetSet.UserFarmer.ToString(), _context))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var product = await _context.Product
                        .Include(p => p.Category)
                        .Include(p => p.Farmer)
                        .FirstOrDefaultAsync(m => m.ProductId == id);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    return View(product);
                }
                else
                {
                    TempData["ErrorMessage"] = "You do not have access to this page";
                    return RedirectToAction("IndexEmployee", "Home");
                }
            }
            
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if (Validation.FarmerExistsId(GetSet.UserFarmer.ToString(), _context))
                {
                    ViewData["CategoryId"] = new SelectList(_context.Category, "CategporyId", "Description");
                    ViewData["FarmerId"] = new SelectList(_context.Farmer, "FarmerId", "Name");
                    return View();
                }
                else
                {
                    TempData["ErrorMessage"] = "You do not have access to this page";
                    return RedirectToAction("IndexEmployee", "Home");
                }
            }
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,ProductionDate,CategoryId,FarmerId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.FarmerId = GetSet.UserFarmer;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategporyId", "Description", product.CategoryId);
            ViewData["FarmerId"] = new SelectList(_context.Farmer, "FarmerId", "Name", product.FarmerId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if (Validation.FarmerExistsId(GetSet.UserFarmer.ToString(), _context))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var product = await _context.Product.FindAsync(id);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    ViewData["CategoryId"] = new SelectList(_context.Category, "CategporyId", "Description", product.CategoryId);
                    ViewData["FarmerId"] = new SelectList(_context.Farmer, "FarmerId", "Name", product.FarmerId);
                    return View(product);
                }
                else
                {
                    TempData["ErrorMessage"] = "You do not have access to this page";
                    return RedirectToAction("IndexEmployee", "Home");
                }
            }
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,ProductionDate,CategoryId,FarmerId")] Product product)
        {
            if (id != product.ProductId)
            {
                TempData["Login"] = "You need to Login First";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategporyId", "Description", product.CategoryId);
            ViewData["FarmerId"] = new SelectList(_context.Farmer, "FarmerId", "Name", product.FarmerId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if (Validation.FarmerExistsId(GetSet.UserFarmer.ToString(), _context))
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var product = await _context.Product
                        .Include(p => p.Category)
                        .Include(p => p.Farmer)
                        .FirstOrDefaultAsync(m => m.ProductId == id);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    return View(product);
                }
                else
                {
                    TempData["ErrorMessage"] = "You do not have access to this page";
                    return RedirectToAction("IndexEmployee", "Home");
                }
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }

        public async Task<IActionResult> FilteredList(int id)
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if (Validation.EmployeeExistsId(GetSet.UserEmployee, _context))
                {
                    if (id != 0)
                    {
                        var products = from p in _context.Product.Include(p => p.Category).Include(p => p.Farmer)
                                       select p;
                        var farmerProduct = products.ToList();

                        farmerProduct = farmerProduct.Where(f => f.FarmerId == id).ToList();


                        var categories = await _context.Category.ToListAsync();

                        var viewModel = new ProductViewModel
                        {
                            Products = farmerProduct,
                            SelectedCategoryId = 0,
                            StartDate = null,
                            EndDate = null,
                            Categories = categories
                        };

                        return View(viewModel);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "You must first select a Farmer";
                        return RedirectToAction("Index", "Farmers");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "You do not have access to this page";
                    return RedirectToAction("IndexFarmer", "Home");
                }
            }
        }

        [HttpPost, ActionName("FilteredList")]
        public async Task<IActionResult> FilteredList(int? selectedCategoryId, DateTime? startDate, DateTime? endDate, int id)
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if (Validation.EmployeeExistsId(GetSet.UserEmployee, _context))
                {
                    if (id != 0)
                    {
                        var products = from p in _context.Product.Include(p => p.Category).Include(p => p.Farmer)
                                       select p;
                        var farmerProduct = products.ToList().Where(f => f.FarmerId == id).ToList();

                        if (selectedCategoryId.HasValue)
                        {
                            farmerProduct = farmerProduct.Where(p => p.CategoryId == selectedCategoryId.Value).ToList();
                        }

                        if (startDate.HasValue)
                        {
                            farmerProduct = farmerProduct.Where(p => p.ProductionDate >= startDate.Value).ToList();
                        }

                        if (endDate.HasValue)
                        {
                            farmerProduct = farmerProduct.Where(p => p.ProductionDate <= endDate.Value).ToList();
                        }

                        var categories = await _context.Category.ToListAsync();

                        var viewModel = new ProductViewModel
                        {
                            Products = farmerProduct,
                            SelectedCategoryId = selectedCategoryId,
                            StartDate = startDate,
                            EndDate = endDate,
                            Categories = categories
                        };

                        return View(viewModel);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "You must first select a Farmer";
                        return RedirectToAction("Index", "Farmers");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "You do not have access to this page";
                    return RedirectToAction("IndexFarmer", "Home");
                }
            }
        }
    }
}

