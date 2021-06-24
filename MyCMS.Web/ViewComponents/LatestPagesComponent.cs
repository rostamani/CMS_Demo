using Microsoft.AspNetCore.Mvc;
using MyCMS.Services.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCMS.Web.ViewComponents
{
    public class LatestPagesComponent : ViewComponent
    {
        private readonly IPageRepository _pageRepository;
        public LatestPagesComponent(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("LatestPages",await _pageRepository.GetLatestPages()));
        }
    }
}
