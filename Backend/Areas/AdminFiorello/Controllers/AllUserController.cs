using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Areas.AdminFiorello.Controllers
{
    [Area("AdminFiorello")]
    //[Authorize(Roles = "Admin")]
    public class AllUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public AllUserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = _userManager.Users.ToList();
            List<UserVM> usersVM = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Fullname = user.Fullname,
                    Role = await _userManager.GetRolesAsync(user)
                };

                usersVM.Add(userVM);
            }
            
            return Json(usersVM);
        }
    }
}