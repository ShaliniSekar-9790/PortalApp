using Moq;
using NewsPortal.API.Repositories;
using NewPortalWebAPI.Controllers;
using NewPortalWebAPI.Model.Entity;
using Microsoft.AspNetCore.Mvc;
using NewPortalWebAPI.Model.Response;
using NewPortalWebAPI.Model.Request;
using System.Collections;

namespace NewsPortal_Test
{


    [TestFixture]
    public class NewsControllerTest
    {
        private Mock<INewsRepository> _mockRepository;
        private NewsController _controller;
      
        // Arrange
        NewsInfo news = new NewsInfo
        {
            Id = 1,
            Title = "TestNews",
            Description = "Test Description",
            CreateDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        Category category = new Category
        {
            CategoryId = 1,
            CategoryName = "ENTERTAINMENT"
        };

       

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<INewsRepository>();
            _controller = new NewsController(_mockRepository.Object);
        }

        [Test]
        public async Task Get_NewsExists_ReturnsOkAsync()
        {
            _mockRepository.Setup(repo => repo.GetNewsById(1)).ReturnsAsync(news);

            // Act
           var result = await _controller.GetNewsById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            PortalApiResponse<NewsInfo> returnedItem = okResult.Value as PortalApiResponse<NewsInfo>;
            Assert.NotNull(returnedItem);
            Assert.AreEqual(news.Id, returnedItem.data.Id);
        }

        [Test]
        public async Task Get_NewsDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetNewsById(1)).ReturnsAsync((NewsInfo)null);

            // Act
            var actionResult = await _controller.GetNewsById(1);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(actionResult);
        }

        [Test]
        public async Task Get_PagedResponse()
        {
            news.Category = category;
            PagedResponse pagedResponse = new PagedResponse(NewsInfos: new List<NewsInfo> { news }, pageNumber: 1, totalRecords: 1, totalPages: 1);


            _mockRepository.Setup(repo => repo.GetNewsInfoByPagination(1,1)).ReturnsAsync(pagedResponse);

            // Act
            var result = await _controller.GetAllByPagination(new PaginationFilter(1,1));

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            PortalApiResponse<PagedResponse> returnedItem = okResult.Value as PortalApiResponse<PagedResponse>;
            Assert.NotNull(returnedItem);
            Assert.AreEqual(news.Id, returnedItem.data.NewsInfos[0].Id);
        }

        [Test]
        public async Task Get_PagedResponse_WithSearch()
        {
            news.Category = category;
            PagedResponse pagedResponse = new PagedResponse(NewsInfos: new List<NewsInfo> { news }, pageNumber: 1, totalRecords: 1, totalPages: 1);


            _mockRepository.Setup(repo => repo.GetNewsInfoBySearchString(1, 1, "testNews")).ReturnsAsync(pagedResponse);

            // Act
            var result = await _controller.GetAllBySearchTerm(new PaginationFilter(1, 1,"testNews"));

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
        }
        [Test]
        public async Task Test_Update_News()
        {
            news.Category = category;
            _mockRepository.Setup(repo => repo.UpdateNews(news));


            // Act
            var result = await _controller.UpdateNews(news);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
        }
        [Test]
        public void Test_Delete_News()
        {
            news.Category = category;
            _mockRepository.Setup(repo => repo.DeleteNewsById(1));


            // Act
            var result =  _controller.DeleteNewsById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
        }
    }
}