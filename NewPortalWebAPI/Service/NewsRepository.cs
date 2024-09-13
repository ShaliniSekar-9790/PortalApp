using Microsoft.EntityFrameworkCore;
using NewPortalWebAPI.Data;
using NewPortalWebAPI.Model.Entity;
using NewPortalWebAPI.Model.Response;
using NewsPortal.API.Repositories;

namespace NewPortalWebAPI.Service
{
    public class NewsRepository : INewsRepository
    {
        NewsContext newsContext;
        ILogger<NewsRepository> logger;

        public NewsRepository(NewsContext newsContext, ILogger<NewsRepository> logger)
        {
            this.newsContext = newsContext;
            this.logger = logger;
        }


        public IEnumerable<Category> GetCategories()
        {
            IEnumerable<Category> categories = new List<Category>();
            try
            {
                categories =  newsContext.Categories.AsNoTracking().ToList();
                logger.LogInformation("NewsRepository:GetCategories:: Data retrieved from DB");

            }
            catch (Exception ex)
            {
                new ApplicationException("Exception while getting categories", ex);
            }
            return categories;
        }

        public NewsInfo GetNewsById(int id)
        {
            NewsInfo newsInfo = new NewsInfo();
            try
            {
                  newsInfo = newsContext.NewsInfos.Include(n => n.Category).
                    AsNoTracking().
                    SingleOrDefault(p => p.Id == id);
            }
            catch (Exception ex)
            {
                new ApplicationException("Exception while getting the news by id", ex);
            }
            return newsInfo;
        }

        public PagedResponse GetNewsInfoByPagination(int pageNumber, int pageSize)
        {
            PagedResponse pagedResponse = new PagedResponse();
            try
            {
                var newsInfo = newsContext.NewsInfos
                                           .Include(n => n.Category)
                                           .OrderByDescending(n => n.UpdatedDate)
                                           .Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToList();

                var totalRecords = newsContext.NewsInfos.Count();
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                pagedResponse = new PagedResponse(newsInfo, pageNumber, totalRecords, totalPages);
                logger.LogInformation("NewsRepository:GetNewsInfoByPagination:: Data retrieved from DB");

            }
            catch (Exception ex)
            {
                new ApplicationException("Exception while getting the news by pagination", ex);
            }
            return pagedResponse;
        }

        public PagedResponse GetNewsInfoBySearchString(int pageSize, int pageNumber, string searchTerm)
        {
             PagedResponse pagedResponse = new PagedResponse();
            try
            {
                var newsInfo = new List<NewsInfo> ();
                if (searchTerm != "" && searchTerm is not null)
                {
                    newsInfo =  newsContext.NewsInfos
                                              .Include(n => n.Category)
                                              .OrderByDescending(n => n.UpdatedDate)
                                              .Where(n => n.Title.ToLower().Contains(searchTerm.ToLower()) || n.Description.ToLower().Contains(searchTerm.ToLower()))
                                              .Skip((pageNumber - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToList();
                } else
                {
                    newsInfo = newsContext.NewsInfos
                                             .Include(n => n.Category)
                                             .OrderByDescending(n => n.UpdatedDate)
                                             .Skip((pageNumber - 1) * pageSize)
                                             .Take(pageSize)
                                             .ToList();
                }
                


                var totalRecords = newsContext.NewsInfos.Count();
                var totalPages =  (int)Math.Ceiling(totalRecords / (double)pageSize);
                pagedResponse = new PagedResponse(newsInfo, pageNumber, totalRecords, totalPages);

                logger.LogInformation("NewsRepository:GetNewsInfoBySearchString:: Data retrieved from DB");

            }
            catch (Exception e)
            {
                new ApplicationException("Exception while getting the news by Search", e);
            }
            return pagedResponse;
        }

        public void CreateNews(NewsInfo newsInfo)
        {
            try
            {
                logger.LogDebug("NewsRepository:CreateNews:: Data retrieved from DB",newsInfo.ToString());

                if (newsInfo != null && newsInfo.Category.CategoryId == 0) {
                    logger.LogDebug("NewsRepository:CreateNews");
                    Category category = newsInfo.Category;
                    category = newsContext.Categories.Add(category).Entity;
                    newsContext.SaveChanges();
                    newsInfo.Category = category;
                 }
                 newsContext.NewsInfos.Add(newsInfo);
                 newsContext.SaveChanges();
                logger.LogInformation("NewsRepository:CreateNews:: Data saved to DB",newsInfo.ToString());

            }
            catch (Exception e)
            {
                new ApplicationException("Exception while saving the news", e);

            }
        }

        public void UpdateNews(NewsInfo newsInfo)
        {
            NewsInfo updateNewsInfo;
            try
            {

                updateNewsInfo = GetNewsById(newsInfo.Id);

                if (updateNewsInfo is null)
                    throw new InvalidOperationException($"News Info '{newsInfo.Id}' does not exist.");
                newsInfo.CreateDate = updateNewsInfo.CreateDate;
                newsInfo.UpdatedDate = DateTime.Now;
                updateNewsInfo = newsInfo;
                newsContext.Entry(updateNewsInfo).State = EntityState.Modified;
                newsContext.SaveChanges();

                logger.LogInformation("NewsRepository:EditNews:: Data updated in DB", newsInfo);
            }
            catch (Exception e)
            {
                new ApplicationException("Exception while updating the news", e);
            }
        }

        public void DeleteNewsById(int id)
        {
            try
            {
                var newsToDelete =  newsContext.NewsInfos.Find(id);
                if (newsToDelete is null)
                    throw new InvalidOperationException($"News Info '{id}' does not exist.");
                newsContext.NewsInfos.Remove(newsToDelete);
                newsContext.SaveChanges();

                logger.LogInformation("NewsRepository: DeleteNews:: Data deleted from DB", newsToDelete);
            }
            catch (Exception e)
            {
                new ApplicationException("Error while deleting the news", e);
            }
        }

    }
}
