using NewPortalWebAPI.Model.Entity;
using NewPortalWebAPI.Model.Request;
using NewPortalWebAPI.Model.Response;
using NewPortalWebAPI.Service;
using Microsoft.AspNetCore.Mvc;
using NewPortalWebAPI.Model;
using NewsPortal.API.Repositories;

namespace NewPortalWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class NewsController : ControllerBase
    {
        INewsRepository newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        [HttpGet]
        public IActionResult GetAllByPagination([FromQuery] PaginationFilter filter)
        {
             var response =  newsRepository.GetNewsInfoByPagination(filter.PageNumber, filter.PageSize);
             return Ok(new PortalApiResponse<PagedResponse>(MyConsts.SUCCESS, response));

        }

        [HttpGet]
        public IActionResult GetAllBySearchTerm([FromQuery] PaginationFilter filter)
        {
            PagedResponse response = newsRepository.GetNewsInfoBySearchString(filter.PageSize, filter.PageNumber, filter.SearchTerm);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public  IActionResult GetNewsById(int id)
        {
            var newsInfo =  newsRepository.GetNewsById(id);

            if (newsInfo is not null)
            {
                return Ok(new PortalApiResponse<NewsInfo>(MyConsts.SUCCESS,newsInfo));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult createNews([FromBody] NewsRequestDto requestData)
        {
            NewsInfo newsInfo = requestData;
            newsRepository.CreateNews(newsInfo);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNewsById(int id)
        {
            newsRepository.DeleteNewsById(id);
            return Ok(new PortalApiResponse<string>(MyConsts.SUCCESS,""));
        }

        
        [HttpPut]
        public IActionResult UpdateNews([FromBody] NewsInfo requestData)
        {
            newsRepository.UpdateNews(requestData);
            return Ok(new PortalApiResponse<string>(MyConsts.SUCCESS,""));
        }
    }
}