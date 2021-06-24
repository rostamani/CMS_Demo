using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace MyCMS.ViewModels.Users
{
    public class EditViewModel
    {
        public EditViewModel()
        {
            Roles = new List<RoleViewModel>();
        }
        public string UserId { get; set; }

        [Display(Name ="نام کاربری")]
        [Required(ErrorMessage ="{0} نمیتواند خالی باشد.")]
        [MaxLength(10,ErrorMessage ="{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        public string Username { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد.")]
        [EmailAddress(ErrorMessage ="ایمیل وارد شده معتبر نیست.")]
        public string Email { get; set; }

        public List<RoleViewModel> Roles { get; set; }        
    }

    public class RoleViewModel
    {
        public string Name { get; set; }

        public bool IsSelected { get; set; }
    }
}
