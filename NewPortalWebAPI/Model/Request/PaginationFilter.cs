namespace NewPortalWebAPI.Model.Request
{
	public class PaginationFilter
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public string? SearchTerm { get; set; }

        public PaginationFilter()
		{
			this.PageNumber = 1;
			this.PageSize = 5;
		}
		public PaginationFilter(int pageNumber, int pageSize)
		{
			this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
			this.PageSize = pageSize > 5 ? 5 : pageSize;
		}
        public PaginationFilter(int pageNumber, int pageSize, string searchTerm)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 5 ? 5 : pageSize;
			this.SearchTerm = searchTerm;
        }
    }
}