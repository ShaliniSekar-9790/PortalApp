using NewPortalWebAPI.Model.Entity;
using NewPortalWebAPI.Model.Response;

namespace NewsPortal.API.Repositories
{
    public interface INewsRepository
    {
        IEnumerable<Category>  GetCategories();
        NewsInfo GetNewsById(int id);
        PagedResponse GetNewsInfoByPagination(int pageNumber, int pageSize);
        PagedResponse GetNewsInfoBySearchString(int pageSize,int pageNumber, string searchTerm);
        void CreateNews(NewsInfo newsInfo);
        void UpdateNews(NewsInfo newsInfo);
        void DeleteNewsById(int id);
    }
}