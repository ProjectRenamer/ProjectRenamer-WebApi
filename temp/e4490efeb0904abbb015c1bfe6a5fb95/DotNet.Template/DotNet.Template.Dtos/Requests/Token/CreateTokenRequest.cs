using System.ComponentModel.DataAnnotations;
using DotNet.Template.Common.ValidatorHelper;
using FluentValidation;

namespace DotNet.Template.Dtos.Requests.Token
{
    public class CreateTokenRequest
    {
        private string _userIdentifier;

        [Required]
        public string UserIdentifier
        {
            get => _userIdentifier;
            set => _userIdentifier = value?.ToLowerInvariant();
        }

        [Required]
        public string Password { get; set; }
    }

    public class CreateTokenRequestValidator : NotNullAbstractValidator<CreateTokenRequest>
    {
        public CreateTokenRequestValidator()
        {
            RuleFor(r => r.UserIdentifier).NotEmpty();

            RuleFor(r => r.Password).NotEmpty();
        }
    }
}