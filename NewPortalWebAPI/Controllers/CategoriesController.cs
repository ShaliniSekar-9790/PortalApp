using Microsoft.AspNetCore.Mvc;
using NewPortalWebAPI.Model.Response;
using NewPortalWebAPI.Model;
using NewPortalWebAPI.Model.Entity;
using NewsPortal.API.Repositories;

namespace NewPortalWebAPI.Controllers
{
    [ApiController]
    [Route("[api/Category")]
    public class CategoriesController : ControllerBase
    {
        INewsRepository newsRepository;

        public CategoriesController(INewsRepository newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        [HttpGet("/getAllCategories")]
        public IActionResult GetAll()
        {
            var response = newsRepository.GetCategories();
            return Ok(new PortalApiResponse<IEnumerable<Category>>(MyConsts.SUCCESS,response));
        }
    }
}