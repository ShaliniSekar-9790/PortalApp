namespace NewPortalWebAPI.Model.Response
{
    public class PortalApiResponse<T>
    {
        public string status { set; get; }
        public T data { set; get; }

        public PortalApiResponse(string status, T data)
        {
            this.status = status;
            this.data = data;
        }
        public PortalApiResponse(string status)
        {
            this.status = status;
            this.data = default;
        }
    }
}
