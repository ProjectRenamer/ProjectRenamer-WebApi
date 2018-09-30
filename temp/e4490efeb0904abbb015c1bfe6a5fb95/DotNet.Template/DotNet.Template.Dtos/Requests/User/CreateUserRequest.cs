using DotNet.Template.Common.ValidatorHelper;
using FluentValidation;

namespace DotNet.Template.Dtos.Requests.User
{
    public class CreateUserRequest
    {
        private string _userName;
        private string _email;

        public string UserName
        {
            get => _userName;
            set => _userName = value?.ToLowerInvariant();
        }

        public string Email
        {
            get => _email;
            set => _email = value?.ToLowerInvariant();
        }

        public string Password { get; set; }
    }

    public class CreateUserRequestValidator : NotNullAbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(r => r.UserName).NotNull()
                                    .Length(3, 50);


            RuleFor(r => r.Email).NotNull()
                                 .MaximumLength(50)
                                 .EmailAddress();

            RuleFor(r => r.Password).NotNull()
                                    .MinimumLength(3);

        }
    }
}
