using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Upgrade_az.Models.User
{
    [Table("Account")]
    public class Account
    {
        public Account()
        {
            RoleAccounts = new HashSet<SlideShow>();
        }

        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<SlideShow> RoleAccounts { get; set; }
    }
}