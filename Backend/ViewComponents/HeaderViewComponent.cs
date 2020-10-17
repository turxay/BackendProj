using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public HeaderViewComponent(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string basket = Request.Cookies["basket"];
            ViewBag.BasketCount = 0;
            ViewBag.BasketPrise = 0;
            if (User.Identity.IsAuthenticated)
            {
                AppUser loginUser =await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.UserFullname = loginUser.Fullname;
            }
            if (basket != null)
            {
                List<ProductBasketVM> products = JsonConvert.DeserializeObject<List<ProductBasketVM>>(basket);
                //ViewBag.BasketCount = products.Count;
                ViewBag.BasketCount = products.Sum(p => p.Count);
                //ViewBag.BasketPrise = products.Sum(p => p.Price);
            }
            
            Bio model = _db.Bios.FirstOrDefault();
            return View(await Task.FromResult(model));
        }
    }
}
