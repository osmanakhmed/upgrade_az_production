using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Upgrade_az.Models.CourseDetail
{
    public class Instructor 
    {
        public Instructor()
        {
            Courses = new HashSet<Course>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; }
        public string Photo { get; set; }
        [NotMapped]
        public IFormFile Upload { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

    }
}
