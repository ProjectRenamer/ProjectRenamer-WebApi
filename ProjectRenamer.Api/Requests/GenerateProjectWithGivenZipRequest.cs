using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProjectRenamer.Api.Requests
{
    public class GenerateProjectWithGivenZipRequest
    {
        [Required]
        public IFormFile ZipFile { get; set; }

        [Required]
        public List<KeyValuePair<string, string>> RenamePairs { get; set; }
    }
}