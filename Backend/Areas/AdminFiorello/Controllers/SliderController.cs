using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.DAL;
using FrontToBack.Helpers;
using FrontToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FrontToBack.Areas.AdminFiorello.Controllers
{
    [Area("AdminFiorello")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IHostingEnvironment _env;
        public SliderController(AppDbContext db, IHostingEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index()
        {
            ViewBag.SliderCount = _db.Sliders.Count();
            return View(_db.Sliders);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return NotFound();
            Slider slide =await _db.Sliders.FindAsync(id);
            if(slide==null) return NotFound();
            return View(slide);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (ModelState["Photo"].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                return View();
            }
            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Shekil formati sech qaqi");
                return View();
            }
            if(slider.Photo.MaxLenght(300))
            {
                ModelState.AddModelError("Photo", "Shekilin olchusu max-mum 200kb olmalidir");
                return View();
            }
            int count = _db.Sliders.Count();
            if (count >= 5)
            {
                ModelState.AddModelError("", "Slider-de max-mum 5 shekil ola biler qaqi!!!");
                return View();
            }

            
            slider.Image =await slider.Photo.SaveImg(_env.WebRootPath,"img");
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Slider slide = await _db.Sliders.FindAsync(id);
            if (slide == null) return NotFound();
            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return NotFound();
            Slider slide = await _db.Sliders.FindAsync(id);
            if (slide == null) return NotFound();

            Helper.DeleteImg(_env.WebRootPath, "img", slide.Image);
            _db.Sliders.Remove(slide);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            Slider slide = await _db.Sliders.FindAsync(id);
            if (slide == null) return NotFound();
            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,Slider slider)
        {
            if (slider.Photo != null)
            {
                Slider slide = await _db.Sliders.FindAsync(id);
                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Shekil formati sech qaqi");
                    return View(slide);
                }
                if (slider.Photo.MaxLenght(0))
                {
                    ModelState.AddModelError("Photo", "Shekilin olchusu max-mum 300kb olmalidir");
                    return View(slide);
                }
                Slider dbSlider =await _db.Sliders.FindAsync(id);
                Helper.DeleteImg(_env.WebRootPath, "img", dbSlider.Image);
                dbSlider.Image= await slider.Photo.SaveImg(_env.WebRootPath, "img");
                
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}