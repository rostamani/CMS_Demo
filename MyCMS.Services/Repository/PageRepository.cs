using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyCMS.DataLayer.Context;
using MyCMS.DomainClasses.Page;
using MyCMS.Services.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCMS.Services.Repository
{
    public class PageRepository : IPageRepository
    {
        private readonly MyCMSDbContext _db;
        public PageRepository(MyCMSDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreatePage(Page page)
        {
            await _db.Pages.AddAsync(page);
            return await Save();
        }

        public async Task<bool> DeletePage(Page page)
        {
            var pageToDelete = new Page { PageId = page.PageId };
            _db.Entry(pageToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            return await Save();
        }

        public async Task<bool> DeletePage(int pageId)
        {
            var pageToDelete = new Page { PageId=pageId};
            _db.Entry(pageToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            return await Save();
        }

        public async Task<IEnumerable<Page>> GetAllPages()
        {
            return await _db.Pages.Include(p=>p.PageGroup).ToListAsync();
        }

        public IEnumerable<Page> GetMostViewedPages(int take=5)
        {
            return _db.Pages.OrderByDescending(p => p.PageVisit).Take(take).ToList();
        }

        public async Task<Page> GetPage(int pageId)
        {
            return await _db.Pages.FindAsync(pageId);
        }

        public async Task<IEnumerable<Page>> GetPagesInSlider()
        {
            return await _db.Pages.Where(p => p.ShowInSlider == true).ToListAsync();
        }
        public async Task<IEnumerable<Page>> GetLatestPages(int take=3)
        {
            return await _db.Pages.OrderByDescending(p => p.CreatedDate).Take(take).ToListAsync();
        }
        public async Task<bool> PageExists(int pageId)
        {
            var page = await GetPage(pageId);
            if (page == null)
                return false;
            return true;
        }

        public async Task<bool> PageExists(string pageTitle)
        {
            return await _db.Pages.AnyAsync(p => p.PageTitle == pageTitle);
        }

        public async Task<bool> Save()
        {
            if(await _db.SaveChangesAsync()>0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdatePage(Page page)
        {
            _db.Entry(page).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return await Save();
        }

        public async Task<IEnumerable<Page>> Search(string q)
        {
            var pages=await _db.Pages.Where(p => p.PageText.Contains(q) || p.PageTitle.Contains(q) || p.ShortDescription.Contains(q) || p.PageTags.Contains(q))
                .ToListAsync();
            return pages.Distinct().ToList();
        }
    }
}
