using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upgrade_az.Data;
using Upgrade_az.Models.CourseDetail;

namespace Upgrade_az.ViewComponents
{
    [ViewComponent(Name = "Course")]
    public class CourseViewComponent : ViewComponent
    {
        private UpGradeDbContext context;

        public CourseViewComponent(UpGradeDbContext _context)
        {
            this.context = _context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Course> courses = context.Courses.ToList();
            return View("Index", courses);
        }
    }
}
