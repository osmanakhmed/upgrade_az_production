using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Upgrade_az.Models.User
{
    [Table("RoleAccount")]
    public class SlideShow
    {
        public int RoleId { get; set; }
        public int AccountId { get; set; }
        public bool Status { get; set; }
        public virtual Account Account { get; set; }
        public virtual Role Role { get; set; }
    }
}