using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upgrade_az.Data;
using Upgrade_az.Models.CourseDetail;

namespace Upgrade_az.ViewComponents
{
    [ViewComponent(Name = "Instructor")]
    public class InstructorViewComponent : ViewComponent
    {
        private UpGradeDbContext context;

        public InstructorViewComponent(UpGradeDbContext _context)
        {
            this.context = _context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Instructor> instructors = context.Instructors.ToList();
            return View("Index", instructors);
        }
    }
}
