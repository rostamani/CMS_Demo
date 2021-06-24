using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyCMS.DomainClasses.PageGroup;
using MyCMS.Services.Repository.IRepository;

namespace MyCMS.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly IPageRepository _pageRepository;
        private readonly IPageGroupRepository _pageGroupRepository;
        public NewsController(IPageRepository pageRepository,IPageGroupRepository pageGroupRepository)
        {
            _pageRepository = pageRepository;
            _pageGroupRepository = pageGroupRepository;
        }

        [Route("News/{newsId?}")]
        public async Task<IActionResult> ShowNews(int? newsId)
        {
            if(newsId == null)
            {
                return NotFound();
            }
            DomainClasses.Page.Page news = await _pageRepository.GetPage(newsId.GetValueOrDefault());
            if(news!=null)
            {
                news.PageVisit++;
                await _pageRepository.UpdatePage(news);
                return View(news);
            }

            return NotFound();
        }

        [Route("Group/{groupId?}")]
        public async Task<IActionResult> ShowGroupNews(int? groupId)
        {
            if (groupId == null)
                return NotFound();

            PageGroup group = await _pageGroupRepository.GetPageGroup(groupId.GetValueOrDefault());
            if (group != null)
            {
                ViewBag.Title = group.PageGroupTitle;
                return View(await _pageGroupRepository.GetAllPagesInGroup(groupId.GetValueOrDefault()));
            }

            return NotFound();
        }

        
        public async Task<IActionResult> Search(string q)
        {
            if (q==null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Query = q;
            return View(await _pageRepository.Search(q));
        }
    }
}