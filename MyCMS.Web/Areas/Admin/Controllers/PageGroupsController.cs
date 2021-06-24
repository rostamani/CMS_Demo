using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.DomainClasses.PageGroup;
using MyCMS.Services.Repository.IRepository;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PageGroupsController : Controller
    {
        private readonly IPageGroupRepository _pgRepository;
        public PageGroupsController(IPageGroupRepository pgRepository)
        {
            _pgRepository = pgRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _pgRepository.GetAllPageGroups());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if(id!=null)
            {
                var pageGroup = await _pgRepository.GetPageGroup(id.GetValueOrDefault());
                if (pageGroup != null)
                    return View(pageGroup);
                return NotFound();
            }
            else
            {
                return Redirect("/admin/pagegroups");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                PageGroup pageGroup = await _pgRepository.GetPageGroup(id.GetValueOrDefault());
                if (pageGroup != null)
                    return View(pageGroup);
                return NotFound();
            }
            else
            {
                return Redirect("/admin/pagegroups");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PageGroup pageGroup)
        {
            if (!ModelState.IsValid)
            {
                return View(pageGroup);
            }

            await _pgRepository.UpdatePageGroup(pageGroup);

            return Redirect("/admin/pagegroups");
        }

        public async Task<IActionResult> Create()
        {
            PageGroup pageGroup = new PageGroup();
            return View(pageGroup);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PageGroup pageGroup)
        {
            if(!ModelState.IsValid)
            {
                return View(pageGroup);
            }
            await _pgRepository.CreatePageGroup(pageGroup);
            //return View("Details", pageGroup);
            return Redirect("/admin/pagegroups");
            //return RedirectToAction("Index", "PageGroups", new { area = "Admin" });
            //return RedirectToAction("Index");
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id != null)
        //    {
        //        await _pgRepository.DeletePageGroup(id.GetValueOrDefault());
        //        return RedirectToAction("Index");
        //        //return NotFound();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var result=await _pgRepository.DeletePageGroup(id.GetValueOrDefault());
                if(result)
                {
                    return Json(true);
                }
                //return RedirectToAction("Index");
                //return NotFound();
                return Json(false);
            }
            else
            {
                return Json(false);
            }
        }

        public async Task<IActionResult> GetPageGroups()
        {
            return Json(await _pgRepository.GetAllPageGroups());
        }
    }
}