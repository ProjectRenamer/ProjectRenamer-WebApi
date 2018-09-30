using System.Collections.Generic;

namespace DotNet.Template.Dtos.Responses
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalItemCount { get; set; }
    }
}
