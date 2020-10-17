using FrontToBack.DAL;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.ViewComponents
{
    public class ProductsViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;
        public ProductsViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int count=12,int skip=0)
        {
            var model = _db.Products.Select(p => new ProductVM
            {
                Id = p.Id,
                Price = p.Price,
                Title = p.Title,
                Category = p.Category,
                Image = p.Image
            }).Take(count);

            return View(await Task.FromResult(model));
        }
    }
}
