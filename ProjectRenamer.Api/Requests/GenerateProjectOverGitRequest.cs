using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectRenamer.Api.Requests
{
    public class GenerateProjectOverGitRequest
    {
        [Required]
        public string RepositoryLink { get; set; }
       
        [Required]
        public List<KeyValuePair<string, string>> RenamePairs { get; set; }
       
        [Required]
        public string BranchName { get; set; }
       
        public string UserName { get; set; }
       
        public string Password { get; set; }
    }
}