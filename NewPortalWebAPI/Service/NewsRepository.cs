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


        public async Task<IEnumerable<Category>> GetCategories()
        {
            IEnumerable<Category> categories = new List<Category>();
            try
            {
                categories = await newsContext.Categories.AsNoTracking().ToListAsync();
                logger.LogInformation("NewsRepository:GetCategories:: Data retrieved from DB");

            }
            catch (Exception ex)
            {
                new ApplicationException("Exception while getting categories", ex);
            }
            return categories;
        }

        public async Task<NewsInfo> GetNewsById(int id)
        {
            NewsInfo newsInfo = new NewsInfo();
            try
            {
                  newsInfo = await newsContext.NewsInfos.Include(n => n.Category).
                    AsNoTracking().
                    SingleOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                new ApplicationException("Exception while getting the news by id", ex);
            }
            return newsInfo;
        }

        public async Task<PagedResponse> GetNewsInfoByPagination(int pageNumber, int pageSize)
        {
            PagedResponse pagedResponse = new PagedResponse();
            try
            {
                var newsInfo = await newsContext.NewsInfos
                                           .Include(n => n.Category)
                                           .OrderByDescending(n => n.CreateDate)
                                           .Skip
                                           ((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

                var totalRecords = await newsContext.NewsInfos.CountAsync();
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

        public async Task<PagedResponse> GetNewsInfoBySearchString(int pageSize, int pageNumber, string searchTerm)
        {
             PagedResponse pagedResponse = new PagedResponse();
            try
            {
                var newsInfo = new List<NewsInfo> ();
                logger.LogDebug("String:", searchTerm);

                if (searchTerm != "" && searchTerm is not null)
                {
                    newsInfo =  await newsContext.NewsInfos
                                              .OrderByDescending(n => n.CreateDate)
                                              .Where(n => n.Title.Contains(searchTerm) || n.Description.Contains(searchTerm))
                                              .Skip((pageNumber - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToListAsync();
                } else
                {
                    logger.LogDebug("String:", searchTerm);

                    newsInfo = await newsContext.NewsInfos
                                             .OrderByDescending(n => n.CreateDate)
                                             .Skip((pageNumber - 1) * pageSize)
                                             .Take(pageSize)
                                             .ToListAsync();
                }
                


                var totalRecords = await newsContext.NewsInfos.CountAsync();
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

        public async Task UpdateNews(NewsInfo newsInfo)
        {
            NewsInfo updateNewsInfo;
            try
            {

                updateNewsInfo = await GetNewsById(newsInfo.Id);

                if (updateNewsInfo is null)
                    throw new InvalidOperationException($"News Info '{newsInfo.Id}' does not exist.");
                newsInfo.CreateDate = updateNewsInfo.CreateDate;
                newsInfo.UpdatedDate = DateTime.Now;
                newsInfo.Category = updateNewsInfo.Category;

                newsContext.Entry(updateNewsInfo).State = (Microsoft.EntityFrameworkCore.EntityState)System.Data.Entity.EntityState.Detached;
                newsContext.NewsInfos.Update(newsInfo);
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
