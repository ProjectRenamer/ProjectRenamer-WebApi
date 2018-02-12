using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRenamer.Api.Requests
{
    public class GenerateProjectRequest
    {
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string RepositoryLink { get; set; }
        [Required]
        public List<KeyValuePair<string, string>> RenamePairs { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}