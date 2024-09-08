using NewPortalWebAPI.Model.Entity;

namespace NewPortalWebAPI.Model.Request
{
    public class NewsRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }

        public static implicit operator NewsInfo(NewsRequestDto requestDto)
            => requestDto is null ? new NewsInfo() :
            new NewsInfo()
            {
                Title = requestDto.Title,
                Description = requestDto.Description,
                Category = requestDto.Category,
                CreateDate = DateTime.Now,
            };
    }
}