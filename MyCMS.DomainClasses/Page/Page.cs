using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using MyCMS.DomainClasses.PageGroup;

namespace MyCMS.DomainClasses.Page
{
    public class Page
    {
        public Page()
        {

        }
        [Key]
        public int PageId { get; set; }

        [Display(Name = "گروه خبر")]
        
        public int PageGroupId { get; set; }


        [Display(Name = "عنوان خبر")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد.")]
        [MaxLength(400, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string PageTitle { get; set; }


        [Display(Name = "توضیج کوتاه")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد.")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }


        [Display(Name = "متن خبر")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد.")]
        [DataType(DataType.MultilineText)]
        public string PageText { get; set; }

        [Display(Name = "تعداد بازدید خبر")]

        public int PageVisit { get; set; }

        [Display(Name = "عکس خبر")]

        public string ImageName { get; set; }

        [Display(Name = "کلمات کلیدی")]

        public string PageTags { get; set; }

        [Display(Name = "نمایش در اسلایدر")]

        public bool ShowInSlider { get; set; }

        [Display(Name = "تاریخ ارسال خبر")]

        public DateTime CreatedDate { get; set; }


        public PageGroup.PageGroup PageGroup { get; set; }
    }
}
