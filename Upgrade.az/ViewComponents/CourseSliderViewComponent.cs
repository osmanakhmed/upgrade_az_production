using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upgrade_az.Data;
using Upgrade_az.Models.CourseDetail;

namespace Upgrade_az.ViewComponents
{
    [ViewComponent(Name = "CourseSlider")]
    public class CourseSliderViewComponent : ViewComponent
    {
        private UpGradeDbContext context;

        public CourseSliderViewComponent(UpGradeDbContext _context)
        {
            this.context = _context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Course> courseslider = context.Courses.ToList();
            return View("Index", courseslider);
        }
    }
}
