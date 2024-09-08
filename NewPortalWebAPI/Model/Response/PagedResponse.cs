using NewPortalWebAPI.Model.Entity;

namespace NewPortalWebAPI.Model.Response { 
public class PagedResponse
{
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public List<NewsInfo> NewsInfos { get; set; }


        public PagedResponse(List<NewsInfo> NewsInfos, int pageNumber, int totalRecords, int totalPages)
        {
            this.PageNumber = pageNumber;
            this.TotalRecords = totalRecords;
            this.NewsInfos = NewsInfos;
            this.TotalPages = totalPages;

        }

        public PagedResponse()
        {
            this.PageNumber = 0;
            this.TotalRecords = 0;
            this.NewsInfos = new List<NewsInfo>();
            this.TotalPages = 0;

        }
    }
}