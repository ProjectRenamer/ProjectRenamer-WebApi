using FluentValidation;
using ProjectRenamer.Api.Helper;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ProjectRenamer.Api.Requests
{
    public class DownloadProjectRequest
    {
        public string Token { get; set; }
    }

    public class DownloadProjectRequestValidator : AbstractValidator<DownloadProjectRequest>
    {
        protected override bool PreValidate(ValidationContext<DownloadProjectRequest> context, ValidationResult result)
            => PreValidations.NullPreValidation(context, result);

        public DownloadProjectRequestValidator()
        {
            RuleFor(r => r.Token).NotEmpty();
        }
    }
}