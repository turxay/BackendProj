using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Controllers
{
    //[Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        public ProductController(AppDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index(int? page)
        {
            ViewBag.Page = page;
            ViewBag.PageCount =Math.Ceiling((decimal)_db.Products.Count() / 12);
            ViewBag.ProductCount = _db.Products.Count();
            if (page == null)
            {
                return View(_db.Products.Select(p => new ProductVM
                {
                    Id = p.Id,
                    Price = p.Price,
                    Title = p.Title,
                    Category = p.Category,
                    Image = p.Image
                }).Take(12));
            }
            else
            {
                return View(_db.Products.Select(p => new ProductVM
                {
                    Id = p.Id,
                    Price = p.Price,
                    Title = p.Title,
                    Category = p.Category,
                    Image = p.Image
                }).Skip(((int)page-1)*12).Take(12));
            }
            
        }

        public IActionResult Load(int skip)
        {
            var model = _db.Products.Select(p => new ProductVM
            {
                Id = p.Id,
                Price = p.Price,
                Title = p.Title,
                Category = p.Category,
                Image = p.Image
            }).Skip(skip).Take(8);
            return PartialView("_ProductPartial", model);
            #region OldVersion
            //return Json(_db.Products.Select(p => new ProductVM
            //{
            //    Id = p.Id,
            //    Price = p.Price,
            //    Title = p.Title,
            //    Category = p.Category,
            //    Image = p.Image
            //}).Skip(8).Take(8));
            #endregion

        }

        public IActionResult Search(string search)
        {
            var model = _db.Products.Where(p => p.Title.Contains(search)).OrderByDescending(p=>p.Id).Take(10).ToList();
            //return Json(model);
            return PartialView("_SearchPartial", model);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Product product =await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            return Json(product);
        }

        //[AllowAnonymous]
        public IActionResult Test()
        {
            return Content("Test");
        }
    }
}