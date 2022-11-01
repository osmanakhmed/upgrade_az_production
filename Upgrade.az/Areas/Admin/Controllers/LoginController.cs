using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Upgrade_az.Data;
using Upgrade_az.Models.User;
using Upgrade_az.Security;

namespace Upgrade_az.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/login")]
    public class LoginController : Controller
    {
        private readonly UpGradeDbContext _context;
        private SecurityManager securityManager = new SecurityManager();
        public LoginController(UpGradeDbContext context)
        {
            _context = context;
        }


        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("process")]
        public IActionResult Process( string username, string password)
        {
            Account account = _context.Accounts.SingleOrDefault(a => a.Username.Equals(username) && a.Status == true);

            if (account is null)
            {
                ViewBag.error = "Invalid Account";
                return View("Index");
                
            }
            else
            {
                if (BCrypt.Net.BCrypt.Verify(password, account.Password))
                {
                    securityManager.SignIn(this.HttpContext, account);
                    return RedirectToAction("index", "dashboard", new { area = "admin" });
                }
                ViewBag.error = "Invalid Password";
                return View("Index");
            }
        }



        [Route("signout")]
        public IActionResult SignOut()
        {
            securityManager.SignOut(this.HttpContext);
            return RedirectToAction("index", "login", new { area = "admin" });

        }

        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var username = user.Value;
            var account = _context.Accounts.SingleOrDefault(a => a.Username.Equals(username));
            return View("Profile", account);

        }

        [HttpPost]
        [Route("profile")]
        public  IActionResult Profile(Account account)
        {
            var currentAccount = _context.Accounts.SingleOrDefault(a => a.Id == account.Id);
            if (!string.IsNullOrEmpty(account.Password))
            {
                currentAccount.Password = currentAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);

            }
            currentAccount.Username = account.Username;
            currentAccount.Email = account.Email;
            currentAccount.FullName = account.FullName;
            _context.SaveChanges();
            ViewBag.msg = "Your profile is succesfully updated";
            return View("Profile");

        }

        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}
