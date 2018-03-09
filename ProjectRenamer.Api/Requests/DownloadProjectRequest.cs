using System.ComponentModel.DataAnnotations;

namespace ProjectRenamer.Api.Requests
{
    public class DownloadProjectRequest
    {
        [Required]
        public string Token { get; set; }
    }
}