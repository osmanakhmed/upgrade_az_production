using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Upgrade_az.Models.CourseDetail;

namespace Upgrade_az.Areas.Admin.Models.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string Header { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Text { get; set; }
        [Required]
        public int LectureCount { get; set; }
        [Required]
        public int LectureHour { get; set; }
        public int LecturePeriod { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
        public List<string> Questions { get; set; }
        public List<string> QuestionStepps { get; set; }
        public List<int> SteppOrders { get; set; }
    }
}
