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
    [Route("admin/instructor")]
    public class InstructorsController : Controller
    {
        private readonly UpGradeDbContext _context;

        public InstructorsController(UpGradeDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Instructors
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Instructors.ToListAsync());
        }

        // GET: Admin/Instructors/Details/5
        [Route("details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Admin/Instructors/Create
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Instructors/Create
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Instructor instructor)
        {
            if (instructor.Upload.ContentType != "image/jpeg" && instructor.Upload.ContentType != "image/png" && instructor.Upload.ContentType != "image/jpg")
            {
                ModelState.AddModelError("Upload", "Image only can be jepg, jpg or png");
            }
            if (ModelState.IsValid)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(instructor.Upload.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "teachers", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    instructor.Upload.CopyTo(stream);
                }

                instructor.Photo = fileName;
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        // GET: Admin/Instructors/Edit/5
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Admin/Instructors/Edit/5
        [Route("edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Position,Photo")] Instructor instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (instructor.Upload != null)
                    {
                        if (instructor.Upload.ContentType != "image/jpeg" && instructor.Upload.ContentType != "image/png" && instructor.Upload.ContentType != "image/jpg")
                        {
                            ModelState.AddModelError("Upload", "Image only can be jepg, jpg or png");
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        if (instructor.Upload != null)
                        {
                            var oldfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "teachers", instructor.Photo);
                            System.IO.File.Delete(oldfile);
                            var fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(instructor.Upload.FileName);
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads","teachers", fileName);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                instructor.Upload.CopyTo(stream);
                            }

                            instructor.Photo = fileName;
                        }
                        _context.Update(instructor);
                        await _context.SaveChangesAsync(); 
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
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
            return View(instructor);
        }

        // GET: Admin/Instructors/Delete/5
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Admin/Instructors/Delete/5
        [Route("delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }
    }
}
