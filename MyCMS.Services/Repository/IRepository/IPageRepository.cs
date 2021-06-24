using MyCMS.DomainClasses.Page;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyCMS.Services.Repository.IRepository
{
    public interface IPageRepository
    {
        Task<IEnumerable<Page>> GetAllPages();
        IEnumerable<Page> GetMostViewedPages(int take = 3);

        Task<IEnumerable<Page>> GetPagesInSlider();
        Task<IEnumerable<Page>> GetLatestPages(int take = 3);
        Task<IEnumerable<Page>> Search(string q);

        Task<Page> GetPage(int pageId);

        Task<bool> PageExists(int pageId);

        Task<bool> PageExists(string pageTitle);

        Task<bool> CreatePage(Page page);

        Task<bool> UpdatePage(Page page);

        Task<bool> DeletePage(Page page);

        Task<bool> DeletePage(int pageId);

        Task<bool> Save();
    }
}
