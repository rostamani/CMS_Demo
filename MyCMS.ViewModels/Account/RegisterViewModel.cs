using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MyCMS.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد.")]
        //[Remote("/Account/IsEmailInUse")]
        public string Username { get; set; }

        [Display(Name ="ایمیل")]
        [Required(ErrorMessage ="{0} نمیتواند خالی باشد.")]
        [EmailAddress(ErrorMessage ="آدرس ایمیل وارد شده معتبر نمیباشد.")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد.")]
        [Compare("Password",ErrorMessage ="رمز عبور و تکرار آن مطابقت ندارند.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
