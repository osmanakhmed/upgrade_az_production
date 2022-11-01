using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Upgrade_az.Models.CourseDetail
{
    public class Lecture
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        public int CourseId { get; set; }
        public virtual List<LectureParticle> LectureParticles { get; set; }
        public virtual Course Course { get; set; }
    }
}
