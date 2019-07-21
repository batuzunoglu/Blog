using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Blog.MVC.Model;
using Blog.MVC.Dto;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Permissions;

namespace Blog.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Model.DbContext _dbContext;
        public HomeController(UserManager<User> userManager, RoleManager<Role> roleManager, Model.DbContext dbContext, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            await Task.FromResult(0);
            return View(new RegisterDto());
        }
         

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto input)
        {
            if (!(await _roleManager.RoleExistsAsync("Admin")))
            {
                await _roleManager.CreateAsync(new Role() { Name = "Admin" });
            }

            var result = await _userManager.CreateAsync(new User() { UserName = input.UserName, Email = input.Email }, input.Password);

            if (result.Succeeded)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == input.UserName && x.Email == input.Email);
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    Console.WriteLine(item.Description);
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await Task.FromResult(0);
            return View(new LoginDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto input)
        {
            var result = await _signInManager.PasswordSignInAsync(input.UserName, input.Password, true, true);
            if (result.Succeeded)
            {
                return LocalRedirect("~/");
            }
            else
            {
                Console.WriteLine(result.ToString());
            }
            return View(new LoginDto());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Users()
        {
            var users =await _dbContext.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Roles()
        {
            var roles = await _dbContext.Roles.ToListAsync();
            return View(roles);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
