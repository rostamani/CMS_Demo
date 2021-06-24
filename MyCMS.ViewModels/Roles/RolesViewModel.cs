using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MyCMS.ViewModels.Roles
{
    public class RolesViewModel
    {
        public string RoleId { get; set; }

        [Display(Name ="عنوان نقش")]
        [Required(ErrorMessage ="عنوان نقش نمیتواند خالی باشد.")]
        [MaxLength(30,ErrorMessage ="{0} نمیتواند بیشتر از {1} کاراکتر باشد.")]
        //[Remote("DoesRoleExist","Roles","Admin",HttpMethod ="POST")]
        public string RoleTitle { get; set; }
    }
}
