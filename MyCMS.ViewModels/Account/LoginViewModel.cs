using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;
namespace MyCMS.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name ="نام کاربری")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد.")]
        public string Username { get; set; }


        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public string ReturnUrl { get; set; }

        [Display(Name ="مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
        public IEnumerable<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
