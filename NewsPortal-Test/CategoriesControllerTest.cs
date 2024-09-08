using Microsoft.AspNetCore.Mvc;
using Moq;
using NewPortalWebAPI.Controllers;
using NewPortalWebAPI.Model.Entity;
using NewPortalWebAPI.Model.Response;
using NewsPortal.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal_Test
{
    [TestFixture]
    public class CategoriesControllerTest
    {
        private Mock<INewsRepository> _mockRepository;
        private CategoriesController _controller;

        Category category = new Category
        {
            CategoryId = 1,
            CategoryName = "ENTERTAINMENT"
        };



        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<INewsRepository>();
            _controller = new CategoriesController(_mockRepository.Object);
        }

        [Test]
        public async Task Get_NewsExists_ReturnsOkAsync()
        {
            _mockRepository.Setup(repo => repo.GetCategories()).ReturnsAsync(new List<Category> { category });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            PortalApiResponse<IEnumerable<Category>> returnedItem = okResult.Value as PortalApiResponse<IEnumerable<Category>>;
            Assert.NotNull(returnedItem);
            Category category1 = returnedItem.data.AsEnumerable().ElementAt(0);
            Assert.AreEqual(category.CategoryId, category1.CategoryId);
        }

    }
}
