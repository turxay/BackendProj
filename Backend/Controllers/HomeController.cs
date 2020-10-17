using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public HomeController(AppDbContext db,
            UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            //HttpContext.Session.SetString("group", "p507");
            //Response.Cookies.Append("name", "Mubush", new CookieOptions { MaxAge = TimeSpan.FromMinutes(20) });
            //string cookie = Request.Cookies["name"];
            //string session = HttpContext.Session.GetString("group");
            HomeVM model = new HomeVM()
            {
                Sliders = _db.Sliders,
                Content = _db.Contents.FirstOrDefault(),
                Categories = _db.Categories
            };
            return View(model);
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null) return NotFound();
            Product product =await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            List<ProductBasketVM> products;
            string existBasket = Request.Cookies["basket"];
            if (existBasket == null)
            {
                products = new List<ProductBasketVM>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<ProductBasketVM>>(existBasket);
            }

            ProductBasketVM checkPro = products.FirstOrDefault(p=>p.Id==id);

            if (checkPro == null)
            {
                ProductBasketVM newPro = new ProductBasketVM
                {
                    Id = product.Id,
                    Image = product.Image,
                    Title = product.Title,
                    Price = product.Price,
                    Count = 1
                };
                products.Add(newPro);
            }
            else
            {
                checkPro.Count++;
            }
            
            string basket = JsonConvert.SerializeObject(products);
            Response.Cookies.Append("basket", basket, new CookieOptions { MaxAge = TimeSpan.FromDays(14) });
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Basket()
        {
            List<ProductBasketVM> products=JsonConvert.DeserializeObject<List<ProductBasketVM>>(Request.Cookies["basket"]);
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Basket")]
        public async Task<IActionResult> BasketPost()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                List<ProductBasketVM> products = JsonConvert.DeserializeObject<List<ProductBasketVM>>(Request.Cookies["basket"]);
                Sale sale = new Sale
                {
                    Date = DateTime.Now,
                    AppUserId= user.Id
                };
                List<SaleProduct> saleProducts = new List<SaleProduct>();
                double total = 0;
                foreach (ProductBasketVM product in products)
                {
                    saleProducts.Add(new SaleProduct
                    {
                        SaleId= sale.Id,
                        Count=product.Count,
                        Price=product.Price,
                        ProductId=product.Id
                    });
                    total += product.Price * product.Count;
                }
                sale.Total = total;
                sale.SaleProducts = saleProducts;
                await _db.Sales.AddAsync(sale);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        public IActionResult Error()
        {
            return Content("Error page");
        }
    }
}