using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCMS.DomainClasses.Page;
using MyCMS.Services.Repository.IRepository;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PagesController : Controller
    {
        private readonly IPageRepository _pageRepository;
        private readonly IPageGroupRepository _pgRepository;

        public PagesController(IPageRepository pageRepository,IPageGroupRepository pgRepository )
        {
            _pageRepository = pageRepository;
            _pgRepository = pgRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _pageRepository.GetAllPages());
        }

        public async Task<IActionResult> Create(int id)
        {
            DomainClasses.Page.Page page = new DomainClasses.Page.Page();
            ViewBag.pageGroups = new SelectList(await _pgRepository.GetAllPageGroups(),"PageGroupId","PageGroupTitle");
            return View(page);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DomainClasses.Page.Page page,IFormFile imageUpload)
        {
            if(!ModelState.IsValid)
            {
                return View(page);
            }

            page.PageVisit = 0;
            page.CreatedDate = DateTime.Now;

            if(imageUpload!=null)
            {
                page.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imageUpload.FileName);
                string directoryToSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", page.ImageName);
                using (var stream=new FileStream(directoryToSaveImage,FileMode.Create))
                {
                    await imageUpload.CopyToAsync(stream);
                }
            }
            if (!await _pageRepository.CreatePage(page))
            {
                ModelState.AddModelError("", "اشتباهی به هنگام ذخیره فایل رخ داد.");
                return View(page);
            }
            return Redirect("/admin/pages");
            //return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return Redirect("/admin/pages");
            }

            DomainClasses.Page.Page page = await _pageRepository.GetPage(id.GetValueOrDefault());
            if(page==null)
            {
                return NotFound();
            }
            ViewBag.pageGroups = new SelectList(await _pgRepository.GetAllPageGroups(), "PageGroupId", "PageGroupTitle", new { pageGroupId = page.PageGroupId });
            return View(page);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DomainClasses.Page.Page page,IFormFile imageUpload)
        {
            if(ModelState.IsValid)
            {
                if (imageUpload != null)
                {
                   if(page.ImageName==null)
                    {
                        page.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imageUpload.FileName);
                    }
                    string directoryToSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", page.ImageName);
                    using(var stream=new FileStream(directoryToSaveImage,FileMode.Create))
                    {
                        await imageUpload.CopyToAsync(stream);
                    }
                }
                if (!await _pageRepository.UpdatePage(page))
                {
                    ModelState.AddModelError("", "اشتباهی به هنگام ذخیره فایل رخ داد.");
                    return View(page);
                }
                return Redirect("/admin/pages");
                ////return RedirectToAction("Index");
                    //return RedirectToAction("Index","PageGroups",new { area="Admin"});
                //return RedirectToAction();
            }
            else
            {
                return View(page);
            }
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //        return RedirectToAction("Index");
        //    await _pageRepository.DeletePage(id.GetValueOrDefault());
        //    return RedirectToAction("Index");
        //}

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return Json(false);
            var result=await _pageRepository.DeletePage(id.GetValueOrDefault());
            return Json(result);
        }
    }
}