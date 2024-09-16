using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using NewPortalWebAPI.Data;
using NewPortalWebAPI.Model.Entity;
using NewPortalWebAPI.Model.Response;
using NewsPortal.API.Repositories;
using System;

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
                categories =  newsContext.Category.AsNoTracking().ToList();
                logger.LogInformation("NewsRepository:GetCategories:: Data retrieved from DB");

            }
            catch (Exception ex)
            {
                new ApplicationException("Exception while getting categories", ex);
            }
            return categories;
        }

        public Category GetCategoryById(int id)
        {
            Category category = new Category();
            try
            {
                category = newsContext.Category
                  .AsNoTracking()
                  .SingleOrDefault(p => p.Category_Id == id);
            }
            catch (Exception ex)
            {
                new ApplicationException("Exception while getting the news by id", ex);
            }
            return category;
        }

        public NewsInfo GetNewsById(int id)
        {
            NewsInfo newsInfo = new NewsInfo();
            try
            {
                  newsInfo = newsContext.NewsInfos.
                    SingleOrDefault(p => p.News_Id == id);
                   newsInfo.Category = newsContext.Category.
                    SingleOrDefault(c => c.Category_Id == newsInfo.Category_Id);

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
                List<NewsInfo> newsInfo = newsContext.NewsInfos
                                           .OrderByDescending(n => n.Updated_Date)
                                           .Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToList();
                newsInfo.ForEach(n => {
                    n.Category = newsContext.Category.SingleOrDefault(c => c.Category_Id == n.Category_Id);
                });


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
                List<NewsInfo> newsInfo = new List<NewsInfo> ();
                var totalRecords = 0;
                if (searchTerm != "" && searchTerm is not null)
                {
                    newsInfo = newsContext.NewsInfos
                                             .OrderByDescending(n => n.Updated_Date)
                                             .Where(n => n.Title.ToLower().Contains(searchTerm.ToLower()) ||
                                             n.News_Description.ToLower().Contains(searchTerm.ToLower())).ToList();                    
                    totalRecords = newsInfo.Count;
                    newsInfo.Skip((pageNumber - 1) * pageSize)
                                              .Take(pageSize);                                              
                                              
                } else
                {
                    newsInfo = newsContext.NewsInfos
                                             .OrderByDescending(n => n.Updated_Date)
                                             .Skip((pageNumber - 1) * pageSize)
                                             .Take(pageSize)
                                             .ToList();
                    totalRecords = newsContext.NewsInfos.Count();

                }

                newsInfo.ForEach(n => {
                    n.Category = newsContext.Category.SingleOrDefault(c => c.Category_Id == n.Category_Id);
                });

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

                if (newsInfo != null && newsInfo.Category.Category_Id == 0) {
                    logger.LogDebug("NewsRepository:CreateNews");
                    Category category = newsInfo.Category;
                    category = newsContext.Category.Add(category).Entity;
                    newsContext.SaveChanges();
                 }
                newsInfo.Category_Id = newsInfo.Category.Category_Id;
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

                updateNewsInfo = GetNewsById(newsInfo.News_Id);

                if (updateNewsInfo is null)
                    throw new InvalidOperationException($"News Info '{newsInfo.News_Id}' does not exist.");
                
                updateNewsInfo.Title = newsInfo.Title;
                updateNewsInfo.News_Description = newsInfo.News_Description;
                updateNewsInfo.Updated_Date = DateTime.Now;
                
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
