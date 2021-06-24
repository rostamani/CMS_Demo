using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCMS.Utilities.Convertors;
using MyCMS.Utilities.Senders;
using MyCMS.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCMS.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IMessageSender _sender;
        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IMessageSender sender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _viewRenderService = viewRenderService;
            _sender = sender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _mapper.Map<IdentityUser>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var emailConfiemLink = Url.Action("ConfrimEmail", "Account",
                                    new { email = user.Email, token = emailConfirmToken }, Request.Scheme);
                var activeEmailModel = new ActiveEmailViewModel { Link = emailConfiemLink };

                string message = _viewRenderService.RenderToStringAsync("_ActiveEmail", activeEmailModel);
                await _sender.SendEmailAsync(user.Email, "تایید ایمیل", message, true);

                ViewBag.SuccessfulRegister = true;
                return View(model);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfrimEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return NotFound();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                ViewBag.SuccessfulConfirm = true;
            }
            else
            {
                ViewBag.SuccessfulConfirm = false;
            }
            return View();
        }

        public async Task<IActionResult> Login(string returnUrl=null)
        {
            if(_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.returnUrl = returnUrl;
            var model = new LoginViewModel
            {
                ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync(),
                ReturnUrl=returnUrl
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
            if(_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index","Home");
            }
            model.ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
            model.ReturnUrl = returnUrl;

            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
                if(!string.IsNullOrEmpty(returnUrl)&&Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                
                return RedirectToAction("Index","Home");
            }
            else
            {
                if(result.IsLockedOut)
                {
                    ViewBag.error = "اکانت شما به دلیل تلاش های ناموفق بسته شده است.";
                    return View(model);
                }


                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه وارد شده است.");
            }

            return View(model);
        }
        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ExternalLogin(string provider,string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Account", new { returnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl=null,string remoteErrors=null)
        {
            if(string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = Url.Content("~/");
            }

            var model = new LoginViewModel
            {
                ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync(),
                ReturnUrl = returnUrl
            };

            if(remoteErrors!=null)
            {
                ModelState.AddModelError("", $"Error:{remoteErrors}");
                return View("Login", model);
            }
            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if(externalLoginInfo==null)
            {
                ModelState.AddModelError("", "مشکلی پیش آمد.");
                return View("Login", model);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, true, true);
            if(signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            var email = externalLoginInfo.Principal.FindFirst(ClaimTypes.Email).Value;
            if(email!=null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if(user==null)
                {
                    var username = email.Split('@')[0];
                    if (username.Length > 10)
                    {
                        username = username.Substring(0, 10);
                    }
                    user = new IdentityUser
                    {
                        UserName = username,
                        Email = email,
                        EmailConfirmed = true
                    };
                    await _userManager.CreateAsync(user);
                }
                await _userManager.AddLoginAsync(user, externalLoginInfo);
                await _signInManager.SignInAsync(user, true);
                return Redirect(returnUrl);
            }
            ModelState.AddModelError("", $"نمیتوان اطلاعاتی از {externalLoginInfo.LoginProvider} به دست آورد.");
            return View("Login", model);
        }

        public async Task<IActionResult> IsEmailInUse(string email) 
        {
            if(await _userManager.FindByEmailAsync(email)!=null)
            {
                return Json("کاربری با ایمیل وارد شده در سایت ثبت نام شده است.");
            }
            return Json(true);
        }
        public async Task<IActionResult> IsUserNameInUse(string username) 
        {
            if (await _userManager.FindByNameAsync(username) != null)
            {
                return Json("کاربری با نام کاربری وارد شده در سایت ثبت نام شده است.");
            }
            return Json(true);
        }

        public async Task<IActionResult> AccessDenied(string returnUrl=null)
        {
            return View();
        }
    }
}
