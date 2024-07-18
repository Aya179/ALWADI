using ALWADI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using ALWADI.Data;
using System.Linq;
namespace ALWADI.Controllers
{
    public class AccountController : Controller
    {
        private readonly AL_WADIContext _context;
        private readonly ApplicationDbContext _context1;

        private UserManager<IdentityUser> userManager;


        public readonly IPasswordHasher<IdentityUser> _passwordHasher;

        private RoleManager<IdentityRole> _roleManager { get; }

        public AccountController(AL_WADIContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> usrMgr, IPasswordHasher<IdentityUser> passwordHasher)
        {
            _context = context;
            _roleManager = roleManager;
            userManager = usrMgr;
            _passwordHasher = passwordHasher;


        }



        public async Task<JsonResult> addRole1Async(string role)

        {
            bool x = await _roleManager.RoleExistsAsync(role);
            if (!x)
            {
                // first we create Admin rool    
                var rolen = new IdentityRole();
                rolen.Name = role;
                await _roleManager.CreateAsync(rolen);
                return Json(rolen);
            }

            else
            return Json("no");



        }

        public IActionResult deleteUser(string userName)
        {
            ViewBag.u=userName;
            return View();
        }
        public async Task<IActionResult> deleteUserActionAsync(string userName)

        {
            IdentityUser user = await userManager.FindByEmailAsync(userName);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("allUsers");
                //else
                //    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("allUsers", userManager.Users);

        }

        public async Task<IActionResult> listofroleAsync()
        {

            var roles = _roleManager.Roles;

            return Json(roles);



        }

        public IActionResult addRole()
        {

            return View();
        }
        public IActionResult allUsersapi()
        {
            var users = userManager.Users;
            return Json(users);
        }
        public IActionResult allUsers()
        {
            return View();
        }
        //public IActionResult adduser()
        //{

        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> creat(string name,string email,string role,string pass)
        //{



        //    if (ModelState.IsValid)
        //    {
        //        var newuser = new IdentityUser { UserName = name, Email = email };
        //        var result = await userManager.CreateAsync(newuser, pass);


        //        //IdentityUser applicationUser = new IdentityUser();
        //        //Guid guid = Guid.NewGuid();
        //        //applicationUser.Id = guid.ToString();
        //        //applicationUser.UserName = name;
        //        //applicationUser.NormalizedUserName = name.ToUpper();
        //        //applicationUser.Email = email;
        //        //applicationUser.NormalizedEmail = email.ToUpper();
        //        //applicationUser.SecurityStamp= Guid.NewGuid().ToString();



        //        //var hashedPassword = _passwordHasher.HashPassword(applicationUser, pass);
        //        //applicationUser.SecurityStamp = Guid.NewGuid().ToString();
        //        //applicationUser.PasswordHash = hashedPassword;
        //        //_context.Users.Add(applicationUser);

        //        //_context.SaveChanges();

        //        ////IdentityResult result = await userManager.CreateAsync(employee, employee.Password);
        //        ////employee.RegisterDate = DateTime.Now;
        //        ////employee.EmailConfirmed = true;
        //        ////employee.NormalizedEmail = employee.Email.ToUpper();
        //        ////employee.NormalizedUserName = employee.UserName.ToUpper();
        //        ////employee.PasswordHash = _passwordHasher.HashPassword(employee, employee.Password);
        //        ////employee.SecurityStamp = Guid.NewGuid().ToString();
        //        ////employee.Phone = "+05" + employee.Phone;
        //        ////_context.Add(employee);
        //        ////await _context.SaveChangesAsync();
        //        ////var user = _context.Employees.FirstOrDefault(x => x.EmployeeNumber == employee.EmployeeNumber);
        //        ////var dbrole = _context.EmployeeRoles.Where(rr => rr.RoleId == user.RoleId).FirstOrDefault();

        //        var user = await userManager.FindByNameAsync(newuser.UserName);
        //        //var rolen = _roleManager.Roles.FirstOrDefault(r => r.Name == role);



        //        //IdentityUserRole<int> userRole = new IdentityUserRole<int>();

        //        //userRole.RoleId = rolen.Id;
        //        //userRole.UserId = user.Id;


        //        var roleresult = await userManager.AddToRoleAsync(user, role);

        //      //  _context1.UserRoles.Add(userRole);
        //        //_context1.SaveChanges();

        //        return Json(newuser);
        //    }


        //    return Json("no");
        //}



       

        public IActionResult Index()
        {
            return View();
        }
    }
}
