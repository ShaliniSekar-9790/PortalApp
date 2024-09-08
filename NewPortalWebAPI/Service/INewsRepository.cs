using NewPortalWebAPI.Model.Entity;
using NewPortalWebAPI.Model.Response;

namespace NewsPortal.API.Repositories
{
    public interface INewsRepository
    {
        Task<IEnumerable<Category>>  GetCategories();
        Task<NewsInfo> GetNewsById(int id);
        Task<PagedResponse> GetNewsInfoByPagination(int pageNumber, int pageSize);
        Task<PagedResponse> GetNewsInfoBySearchString(int pageSize,int pageNumber, string searchTerm);
        void CreateNews(NewsInfo newsInfo);
        Task UpdateNews(NewsInfo newsInfo);
        void DeleteNewsById(int id);
    }
}