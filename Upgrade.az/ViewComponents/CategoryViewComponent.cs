using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Upgrade_az.Data;
using Upgrade_az.Models.CourseDetail;

namespace Upgrade_az.ViewComponents
{
    [ViewComponent(Name = "Category")]
    public class CategoryViewComponent : ViewComponent
    {
        private UpGradeDbContext context;

        public CategoryViewComponent(UpGradeDbContext _context)
        {
            this.context = _context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Category> categories = context.Categories.ToList();
            return View("Index", categories);
        }
    }
}
