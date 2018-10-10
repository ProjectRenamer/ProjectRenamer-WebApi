using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProjectRenamer.Api.Helper;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ProjectRenamer.Api.Requests
{
    public class GenerateProjectOverZipRequest
    {
        [Required]
        public IFormFile ZipFile { get; set; }

        [Required]
        public List<KeyValuePair<string, string>> RenamePairs { get; set; }
    }

    public class GenerateProjectOverZipRequestValidator : AbstractValidator<GenerateProjectOverZipRequest>
    {
        protected override bool PreValidate(ValidationContext<GenerateProjectOverZipRequest> context, ValidationResult result)
            => PreValidations.NullPreValidation(context, result);

        public GenerateProjectOverZipRequestValidator()
        {
            RuleFor(r => r.RenamePairs).NotNull();
            RuleForEach(r => r.RenamePairs).Must(pair => !string.IsNullOrWhiteSpace(pair.Key) && !string.IsNullOrWhiteSpace(pair.Value))
                                           .WithMessage("Rename Pairs should not contain empty value");

            RuleFor(r => r.ZipFile).Must(file => file != null && file.Length == 0)
                                   .WithMessage("Zip file should not be empty");

            RuleFor(r => r.ZipFile).Must(f => f.FileName.EndsWith(".zip", StringComparison.CurrentCultureIgnoreCase))
                                   .When(r => r.ZipFile != null)
                                   .WithMessage("Zip file should be provided");
        }
    }
}