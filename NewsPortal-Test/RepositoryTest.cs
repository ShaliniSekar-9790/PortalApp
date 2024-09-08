using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewPortalWebAPI.Data;
using NewPortalWebAPI.Service;
using Microsoft.EntityFrameworkCore.InMemory;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using NewPortalWebAPI.Model.Entity;
using NewPortalWebAPI.Model.Response;

namespace NewsPortal_Test
{
    [TestFixture]
    public class RepositoryTest
    {
        private NewsRepository _repository;
        private NewsContext _context;
        private ILogger<NewsRepository> _logger;
        private Category category = new Category { CategoryId = 1, CategoryName = "ENTERTAINMENT" };

        // Arrange
        NewsInfo news = new NewsInfo
        {
            Id = 1,
            Title = "TestNews",
            Description = "Test Description",
            CreateDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NewsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new NewsContext(options);
            _repository = new NewsRepository(_context,_logger);
        }

        [TearDown]
        public void destroy()
        {
            _context.Database.EnsureDeleted();
            this._context.Dispose();
        }

        [Test]
        public async Task GetCategories_ReturnsItem_WhenItemExists()
        { 
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetCategories();

            // Assert
            Assert.NotNull(result);
            Category category1 = result.AsEnumerable().ElementAt(0);
            Assert.AreEqual(category.CategoryId, category1.CategoryId);
        }

        [Test]
        public async Task SaveNews()
        {
            news.Category = category;

            // Act
            _repository.CreateNews(news);

            // Assert
            Assert.NotNull(_repository.GetNewsById(1));
        }

        [Test]
        public async Task GetNews_ByPagination_WhenItemExists()
        {
            news.Category = category;
            _context.NewsInfos.Add(news);
            _context.SaveChanges();
            var result =  await _repository.GetNewsInfoByPagination(1,1);

            // Assert
            Assert.NotNull(result);
            PagedResponse response= result;
            Assert.AreEqual(1, response.TotalRecords);
            Assert.False(response.HasPreviousPage);
            Assert.False(response.HasPreviousPage);
        }


        [Test]
        public async Task GetNews_ReturnsItem_WhenItemExists()
        {
            NewsInfo newsbyid = new NewsInfo { Id = 2, Title = "testtile", Description = "testdesc", CreateDate = DateTime.Now, UpdatedDate = DateTime.Now };
            newsbyid.Category = category;
            _context.NewsInfos.Add(newsbyid);
            _context.SaveChanges();

            var result = await _repository.GetNewsById(2);

            // Assert
            Assert.NotNull(result);
        }

        [Test]
        public void  Delete_Items()
        {

            // Act
            _repository.DeleteNewsById(1);

            // Assert
            Assert.Null(_repository.GetNewsById(1).Result);
        }
    }
}
