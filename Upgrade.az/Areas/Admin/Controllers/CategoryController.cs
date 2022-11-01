using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Upgrade_az.Data;
using Upgrade_az.Models.CourseDetail;

namespace Upgrade_az.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private readonly UpGradeDbContext _context;

        public CategoryController(UpGradeDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var upGradeDbContext = _context.Categories.Include(c => c.Parent);
            return View(await upGradeDbContext.ToListAsync());
        }

        // GET: Admin/Categories/Details/5
        [Route("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Categories/Create
        [Route("create")]
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Admin/Categories/Create
        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Category category)
        {
            if (category.Upload.ContentType != "image/jpeg" && category.Upload.ContentType != "image/png" && category.Upload.ContentType != "image/jpg")
            {
                ModelState.AddModelError("Upload", "Image only can be jepg, jpg or png");
            }

            if (ModelState.IsValid)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(category.Upload.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "categories", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    category.Upload.CopyTo(stream);
                }

                category.Photo = fileName;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentId);
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentId);
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        [Route("edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (category.Upload != null)
                    {
                        if (category.Upload.ContentType != "image/jpeg" && category.Upload.ContentType != "image/png" && category.Upload.ContentType != "image/jpg")
                        {
                            ModelState.AddModelError("Upload", "Image only can be jepg, jpg or png");
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        if (category.Upload != null)
                        {
                            if (category.Photo is object)
                            {
                                var oldfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "categories", category.Photo);
                                System.IO.File.Delete(oldfile);
                                var fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(category.Upload.FileName);
                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "categories", fileName);
                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    category.Upload.CopyTo(stream);
                                    category.Photo = fileName;

                                }
                            }
                            else
                            {
                                var fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(category.Upload.FileName);
                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "categories", fileName);
                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    category.Upload.CopyTo(stream);
                                    category.Photo = fileName;
                                }
                            }
                        }
                        _context.Update(category);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            ViewData["ParentId"] = new SelectList(_context.Categories, "Id", "Name", category.ParentId);
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [Route("delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
