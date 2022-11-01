using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Upgrade_az.Models.CourseDetail
{
    public class LectureParticle
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        public int LectureId { get; set; }
        public  virtual Lecture Lecture { get; set; }
    }
}
