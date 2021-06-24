using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCMS.Services.Repository.IRepository;
using MyCMS.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Owner")]
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.Select(u => new IndexViewModel
            {
                UserId = u.Id,
                Username = u.UserName,
                Email = u.Email
            }).ToListAsync());
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = _roleManager.Roles.Select(role => new RoleViewModel
            {
                Name = role.Name,
                IsSelected = false
            }).ToList();
            foreach (var item in roles)
            {
                if (await _userManager.IsInRoleAsync(user, item.Name))
                {
                    item.IsSelected = true;
                }
            }
            var model = new EditViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles,
            };
            //return View(_mapper.Map<IndexViewModel>(user));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.UserId);

            var requestedRoles = model.Roles.Where(role => role.IsSelected).Select(role => role.Name).ToList();
            
            var userRoles = await _userManager.GetRolesAsync(user);
            var shouldBeDeletedRoles = userRoles.Where(u => !requestedRoles.Contains(u)).ToList();
            var shouldBeAddedRoles = requestedRoles.Where(u => !userRoles.Contains(u)).ToList();

 
            user.UserName = model.Username;
            user.Email = model.Email;
            await _userManager.RemoveFromRolesAsync(user, shouldBeDeletedRoles);
            await _userManager.AddToRolesAsync(user, shouldBeAddedRoles);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Redirect("/admin/users");
            }
            ModelState.AddModelError("", "مشکلی رخ داد");
            return View(model);
        }

        //public async Task<IActionResult> Delete(string UserId)
        //{
        //    if (UserId == null)
        //    {
        //        return NotFound();
        //    }

        //    var user =await  _userManager.FindByIdAsync(UserId);
        //    if(user==null)
        //    {
        //        return NotFound();
        //    }

        //    var result = await _userManager.DeleteAsync(user);
        //    if(result.Succeeded)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    ModelState.AddModelError("", $"متاسفانه مشکلی به هنگام حذف کاربر رخ داد و حذف صورت نگرفت.");
        //    return RedirectToAction("Index");
        //}

        //[HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return Json(false);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(false);
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json(true);
            }
            ModelState.AddModelError("", $"متاسفانه مشکلی به هنگام حذف کاربر رخ داد و حذف صورت نگرفت.");
            return Json(false);
        }
    }
}
