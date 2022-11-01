using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Upgrade_az.Areas.Admin.Models.ViewModels;
using Upgrade_az.Data;
using Upgrade_az.Models.CourseDetail;
using VisioForge.Shared.MediaFoundation.OPM;

namespace Upgrade_az.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/course")]
    public class CourseController : Controller
    {
        private readonly UpGradeDbContext _context;
        public CourseController(UpGradeDbContext context)
        {
            _context = context;
        }

        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var upGradeDbContext = _context.Courses.Include(c => c.Category).Include(c => c.Instructor);
            return View(await upGradeDbContext.ToListAsync());
        }


        [Route("create")]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            ViewBag.Instructors = _context.Instructors.Select(c => new SelectListItem
            {
                Text = c.FullName,
                Value = c.Id.ToString()
            }).ToList();
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(Course request, CourseViewModel courseViewModel)
        {
            Course course = new Course();
            List<Lecture> questions = new List<Lecture>();
            if (request.Upload.ContentType != "image/jpeg" && request.Upload.ContentType != "image/png" && request.Upload.ContentType != "image/jpg")
            {
                ModelState.AddModelError("Upload", "Image only can be jepg, jpg or png");
            }

            if (!ModelState.IsValid)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(request.Upload.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    request.Upload.CopyTo(stream);
                }

                request.Photo = fileName;

                _context.Courses.Add(request);

                
                course.Title = courseViewModel.Title;
                course.Text = courseViewModel.Text;
                course.LectureCount = courseViewModel.LectureCount;
                course.LecturePeriod = courseViewModel.LecturePeriod;
                course.LectureHour = courseViewModel.LectureHour;

                if (courseViewModel.Questions is null)
                {
                    ViewBag.ErrorMessage = "There is a problem, Try Again";
                    return RedirectToAction("index", "course", new { area = "admin" });
                }
                if (courseViewModel.Questions.Count > 0)
                {
                    int cache = 0;
                    for (int i = 0; i < courseViewModel.Questions.Count; i++)
                    {
                        var lectures = new List<LectureParticle>();
                        var stepquestionCount = courseViewModel.SteppOrders.Where(a => Convert.ToInt32(a) == i).ToList().Count;
                        if (courseViewModel.QuestionStepps.Count > 0)
                        {
                            for (int s = 0; s < stepquestionCount; s++)
                            {
                                if (courseViewModel.QuestionStepps[cache] is null)
                                {
                                    TempData["ServiceErrorAdd"] = "Əlavə etmə prosesi uğursuzdur yenidən təkrar edin";
                                    return RedirectToAction("Index", "Services");
                                }
                                lectures.Add(new LectureParticle { Name = courseViewModel.QuestionStepps[cache] });
                                cache += 1;
                            }
                        }
                        questions.Add(new Lecture { Name = courseViewModel.Questions[i], LectureParticles = lectures });
                    }
                }
                request.Lectures = questions;
                
                _context.SaveChanges();
                return RedirectToAction("index", "course", new { area = "admin" });
            }

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            ViewBag.Instructors = _context.Instructors.Select(c => new SelectListItem
            {
                Text = c.FullName,
                Value = c.Id.ToString()
            }).ToList();

            return RedirectToAction("index", "course", new { area = "admin" });
        }


        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            Course course = _context.Courses.Find(id);
            if (course is null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            ViewBag.Instructors = _context.Instructors.Select(c => new SelectListItem
            {
                Text = c.FullName,
                Value = c.Id.ToString()
            }).ToList();

            return View(course);
        }


        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(Course course)
        {
            if (course.Upload != null)
            {
                if (course.Upload.ContentType != "image/jpeg" && course.Upload.ContentType != "image/png" && course.Upload.ContentType != "image/jpg")
                {
                    ModelState.AddModelError("Upload", "Image only can be jepg, jpg or png");
                }
            }

            if (ModelState.IsValid)
            {
                if (course.Upload != null)
                {
                    var oldfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", course.Photo);
                    System.IO.File.Delete(oldfile);
                    var fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(course.Upload.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        course.Upload.CopyTo(stream);
                    }

                    course.Photo = fileName;
                }

                _context.Entry(course).State = EntityState.Modified;

                _context.SaveChanges();

                return RedirectToAction("index", "course", new { area = "admin" });
            }

            ViewBag.Categories = _context.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            ViewBag.Instructors = _context.Instructors.Select(c => new SelectListItem
            {
                Text = c.FullName,
                Value = c.Id.ToString()
            }).ToList();
            return View(course);
        }

        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            Course course = _context.Courses.Find(id);
            if (course is null)
            {
                return NotFound();
            }

            var photo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", course.Photo);
            System.IO.File.Delete(photo);
            _context.Courses.Remove(course);
            _context.SaveChanges();
            return RedirectToAction("index", "course", new { area = "admin" });
        }

    }
}
