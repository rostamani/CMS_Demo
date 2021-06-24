using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyCMS.DomainClasses.PageGroup
{
    public class PageGroup
    {
        public PageGroup()
        {

        }
        [Key]
        public int PageGroupId { get; set; }

        [Display(Name ="عنوان گروه")]
        [Required(ErrorMessage ="{0} نمیتواند خالی باشد")]
        [MaxLength(200,ErrorMessage ="{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string PageGroupTitle { get; set; }


        public virtual List<Page.Page> Pages { get; set; }


    }
}
