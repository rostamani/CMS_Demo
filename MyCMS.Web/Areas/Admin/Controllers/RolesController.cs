using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCMS.ViewModels.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Owner")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _roleManager.Roles.Select(r => new RolesViewModel {RoleId=r.Id,RoleTitle=r.Name }).ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var role = new IdentityRole(model.RoleTitle);
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Redirect("/admin/roles");
            }
            ModelState.AddModelError("", "متاسفانه مشکلی به هنگام ایجاد دسترسی مربوطه پیش آمد.");
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();
            var role = await _roleManager.FindByIdAsync(id);
            if(role==null)
            {
                return NotFound();
            }

            return View(new RolesViewModel { RoleId = role.Id, RoleTitle = role.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            IdentityRole role =await _roleManager.FindByIdAsync(model.RoleId);
            role.Name = model.RoleTitle;
            var result =await _roleManager.UpdateAsync(role);
            if(result.Succeeded)
            {
                return Redirect("/admin/roles");
            }
            ModelState.AddModelError("", "متاسفانه مشکلی به هنگام ویرایش دسترسی مربوطه پیش آمد.");
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return NotFound();
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Redirect("/admin/roles");
            }
            return Redirect("/admin/roles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoesRoleExist(string roleTitle)
        {
            var role = await _roleManager.FindByNameAsync(roleTitle);
            if(role==null)
            {
                return Json(true);
            }
            return Json("دسترسی وارد شده در سایت موجود است.");
        }
    }
}
