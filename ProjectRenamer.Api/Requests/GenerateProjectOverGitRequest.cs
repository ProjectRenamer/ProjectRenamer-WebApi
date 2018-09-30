using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;

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

    public class GenerateProjectOverGitRequestValidator : AbstractValidator<GenerateProjectOverGitRequest>
    {
        public GenerateProjectOverGitRequestValidator()
        {
            RuleFor(r => r.RepositoryLink).NotEmpty();
            RuleFor(r => r.BranchName).NotEmpty();
            RuleFor(r => r.RenamePairs).NotNull();
            RuleForEach(r => r.RenamePairs).Must(pair => !string.IsNullOrWhiteSpace(pair.Key) && !string.IsNullOrWhiteSpace(pair.Value))
                                           .WithMessage("Rename Pairs should not contains empty value");
        }
    }
}