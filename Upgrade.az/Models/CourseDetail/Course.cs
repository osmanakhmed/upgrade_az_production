using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Upgrade_az.Models.CourseDetail
{
    [Table("Course")]
    public partial class Course
    {

        public Course()
        {

        }

        public int Id { get; set; }
        [DisplayName("Title")]
        [Required(ErrorMessage ="Title is required")]
        [MaxLength(200, ErrorMessage = "Title can be max 200 words")]
        public string Title { get; set; }

        public string CourseDesc { get; set; }
        public int LectureCount { get; set; }
        public int LectureHour { get; set; }
        public int LecturePeriod { get; set; }

        [Required(ErrorMessage = "Photo is required")]
        [MaxLength(100)]
        public string Photo { get; set; }
        [NotMapped]
        public IFormFile Upload { get; set; }
        [DisplayName("Article")]
        [Required(ErrorMessage = "Article is required")]
        [MinLength(100, ErrorMessage = "Title can be min 100 words")]
        [Column(TypeName = "ntext")]
        public string Text { get; set; }
        [DisplayName("Instructor")]
        [Required(ErrorMessage = "Instructor Name is required")]
        public int InstructorId { get; set; }
        public virtual Instructor Instructor { get; set; }
        public bool Status { get; set; }
        [DisplayName("Category")]
        [Required(ErrorMessage = "Category Name is required")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Lecture> Lectures { get; set; }


    }
}
