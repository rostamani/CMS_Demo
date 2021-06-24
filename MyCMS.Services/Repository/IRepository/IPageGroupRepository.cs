using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyCMS.DomainClasses.Page;
using MyCMS.DomainClasses.PageGroup;
using MyCMS.ViewModels.PageGroups;

namespace MyCMS.Services.Repository.IRepository
{
    public interface IPageGroupRepository
    {
        Task<IEnumerable<PageGroup>> GetAllPageGroups();
        Task<IEnumerable<Page>> GetAllPagesInGroup(int pageGroupId);

        Task<PageGroup> GetPageGroup(int pageGroupId);

        IEnumerable<PageGroupsMenuVM> GetPageGroupsMenuViewModel();

        Task<bool> PageGroupExists(int pageGroupId);

        Task<bool> PageGroupExists(string pageGroupTitle);

        Task<bool> CreatePageGroup(PageGroup pageGroup);

        Task<bool> UpdatePageGroup(PageGroup pageGroup);

        Task<bool> DeletePageGroup(PageGroup pageGroup);

        Task<bool> DeletePageGroup(int pageGroupId);

        Task<bool> Save();

    }
}
