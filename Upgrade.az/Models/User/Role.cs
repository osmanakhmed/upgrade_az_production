using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Upgrade_az.Models.User
{
    [Table("Role")]
    public class Role
    {
        public Role()
        {
            RoleAccounts = new HashSet<SlideShow>();
        }


        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<SlideShow> RoleAccounts { get; set; }

    }
}
