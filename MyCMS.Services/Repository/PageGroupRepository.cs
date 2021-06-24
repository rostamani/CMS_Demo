using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MyCMS.DataLayer.Context;
using MyCMS.DomainClasses.Page;
using MyCMS.DomainClasses.PageGroup;
using MyCMS.Services.Repository.IRepository;
using MyCMS.ViewModels.PageGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCMS.Services.Repository
{
    public class PageGroupRepository : IPageGroupRepository
    {
        private readonly MyCMSDbContext _db;
        public PageGroupRepository(MyCMSDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreatePageGroup(PageGroup pageGroup)
        {
            await _db.PageGroups.AddAsync(pageGroup);
            return await Save();
        }

        public async Task<bool> DeletePageGroup(PageGroup pageGroup)
        {
            var pageGroupToDelete = new PageGroup { PageGroupId = pageGroup.PageGroupId };
            _db.PageGroups.Remove(pageGroupToDelete);
            return await Save();
        }

        public async Task<bool> DeletePageGroup(int pageGroupId)
        {
            var pageGroup = new PageGroup { PageGroupId = pageGroupId };
            _db.Entry(pageGroup).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            return await Save();

            //instead of writing _db.PageGroups.Remove(p=>p.PageGroupId == pageGroupId);
        }

        public async Task<IEnumerable<PageGroup>> GetAllPageGroups()
        {
            return await _db.PageGroups.ToListAsync();
        }

        public async Task<IEnumerable<Page>> GetAllPagesInGroup(int pageGroupId)
        {
            PageGroup group = await _db.PageGroups.Include(g => g.Pages).FirstOrDefaultAsync(g => g.PageGroupId == pageGroupId);
            
            return group.Pages.OrderByDescending(p => p.CreatedDate).ToList();
        }

        public async Task<PageGroup> GetPageGroup(int pageGroupId)
        {
            return await _db.PageGroups.FindAsync(pageGroupId);
        }

        public IEnumerable<PageGroupsMenuVM> GetPageGroupsMenuViewModel()
        {
            return _db.PageGroups.Select(pg => new PageGroupsMenuVM
            {
                PageGroupId = pg.PageGroupId,
                PageGroupTitle = pg.PageGroupTitle,
                PagesCount = pg.Pages.Count
            }).ToList();
        }

        public async Task<bool> PageGroupExists(int pageGroupId)
        {
            var pageGroup = await GetPageGroup(pageGroupId);
            if (pageGroup == null)
                return false;
            return true;
        }

        public async Task<bool> PageGroupExists(string pageGroupTitle)
        {
            return await _db.PageGroups.AnyAsync(p => p.PageGroupTitle == pageGroupTitle);
        }

        public async Task<bool> Save()
        {
            if (await _db.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<bool> UpdatePageGroup(PageGroup pageGroup)
        {
            _db.Entry(pageGroup).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return await Save();
        }
    }
}
