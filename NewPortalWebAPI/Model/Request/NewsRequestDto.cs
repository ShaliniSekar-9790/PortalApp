using NewPortalWebAPI.Model.Entity;

namespace NewPortalWebAPI.Model.Request
{
    public class NewsRequestDto
    {
        public string Title { get; set; }
        public string News_Description { get; set; }
        public Category Category { get; set; }

        public static implicit operator NewsInfo(NewsRequestDto requestDto)
            => requestDto is null ? new NewsInfo() :
            new NewsInfo()
            {
                Title = requestDto.Title,
                News_Description = requestDto.News_Description,
                Category = requestDto.Category,
                Create_Date = DateTime.Now,
                Updated_Date = DateTime.Now
            };
    }
}