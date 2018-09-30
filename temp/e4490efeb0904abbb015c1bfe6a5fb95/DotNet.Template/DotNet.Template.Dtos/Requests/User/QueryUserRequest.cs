using DotNet.Template.Common.ValidatorHelper;
using FluentValidation;

namespace DotNet.Template.Dtos.Requests.User
{
    public class QueryUserRequest : PagedRequest
    {
        private string _userName;
        private string _userEmail;

        public string UserName
        {
            get => _userName;
            set => _userName = value?.ToLowerInvariant();
        }
        public string UserEmail
        {
            get => _userEmail;
            set => _userEmail = value?.ToLowerInvariant();
        }
    }

    public class QueryUserRequestValidator : NotNullAbstractValidator<QueryUserRequest>
    {
        public QueryUserRequestValidator()
        {
            RuleFor(request => request.PageNumber).GreaterThan(0);
            RuleFor(request => request.PageSize).InclusiveBetween(1, 100);
        }
    }
}