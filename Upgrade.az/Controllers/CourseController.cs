using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upgrade_az.Data;
using Upgrade_az.Models.CourseDetail;

namespace Upgrade_az.Controllers
{
    public class CourseController : Controller
    {
        private readonly UpGradeDbContext _context;
        public CourseController(UpGradeDbContext context)
        {
            _context = context;
        }
        public IActionResult Course()
        {
            ViewBag.CountCourses = _context.Courses.Count();
            return View();
        }

        public IActionResult Instructor()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            Course details = new Course();
            Course course = _context.Courses.Where(a => a.Id == id).FirstOrDefault();
            if (course is object)
            {
                details = course;
            }

            return View(details);
        }

        public IActionResult Category(int id)
        {
            var category = _context.Categories.Find(id);
            ViewBag.Category = category;
            ViewBag.CoursesCount = category.Courses.Count(p=>p.CategoryId == category.Id);
            ViewBag.Courses = category.Courses.Where(p => p.CategoryId == category.Id).ToList();
            return View("Category");
        }

    }
}
