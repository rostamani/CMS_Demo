using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.Services.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCMS.Web.ViewComponents
{
    public class PagesInSliderComponent:ViewComponent
    {
        private readonly IPageRepository _pageRepository;
        public PagesInSliderComponent(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("PagesInSlider",await _pageRepository.GetPagesInSlider()));
        }
    }
}
