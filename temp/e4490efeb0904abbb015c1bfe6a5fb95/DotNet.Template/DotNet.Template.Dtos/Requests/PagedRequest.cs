namespace DotNet.Template.Dtos.Requests
{
    public class PagedRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
