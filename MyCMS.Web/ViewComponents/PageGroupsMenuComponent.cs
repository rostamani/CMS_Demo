using Microsoft.AspNetCore.Mvc;
using MyCMS.Services.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCMS.Web.ViewComponents
{
    public class PageGroupsMenuComponent : ViewComponent
    {
        private readonly IPageGroupRepository _pgRepository;
        public PageGroupsMenuComponent(IPageGroupRepository pgRepository)
        {
            _pgRepository = pgRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("PageGroupsMenu", _pgRepository.GetPageGroupsMenuViewModel()));
        }
    }
}
