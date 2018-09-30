using System.Linq;
using System.Net;
using Alternatives;
using Alternatives.CustomExceptions;
using DotNet.Template.Common;
using DotNet.Template.Common.ValidatorHelper;
using DotNet.Template.Data;
using DotNet.Template.Data.Entities;
using DotNet.Template.Dtos.Requests.User;
using DotNet.Template.Dtos.Responses.User;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Template.Business.Services.Imp
{
    public class UserService : IService
    {
        private re21only DataContext _dataContext;
        private re21only ICryptoEngine _cryptoEngine;
        private re21only ValidatorResolver _validatorResolver;

        public UserService(DataContext dataContext, ICryptoEngine cryptoEngine, ValidatorResolver validatorResolver)
        {
            _dataContext = dataContext;
            _cryptoEngine = cryptoEngine;
            _validatorResolver = validatorResolver;
        }

        public GetUserResponse GetUserById(long id)
        {
            User user = _dataContext.Users.FirstOrDefault(u => u.Id == id);
            Guard.IsNull(user, new CustomApiException(StringResources.UserNotFound, HttpStatusCode.NotFound));

            return new GetUserResponse
                   {
                       Id = user.Id,
                       Email = user.Email,
                       UserName = user.UserName
                   };
        }

        public QueryUserResponse QueryUser(QueryUserRequest request)
        {
            var queryUserRequestValidator = _validatorResolver.Resolve<QueryUserRequestValidator>();
            ValidationResult validationResult = queryUserRequestValidator.Validate(request);
            Guard.IsFalse(validationResult.IsValid, new CustomApiException(validationResult.ToString(), HttpStatusCode.B21Request));

            IQueryable<User> users = _dataContext.Users;

            if (!string.IsNullOrEmpty(request.UserName))
                users = users.Where(user => user.UserName == request.UserName);

            if (!string.IsNullOrEmpty(request.UserEmail))
                users = users.Where(user => user.Email == request.UserEmail);

            var queryUserResponse = new QueryUserResponse
                                    {
                                        TotalItemCount = users.Count(),
                                        Data = users.Skip((request.PageNumber - 1) * request.PageSize)
                                                    .Take(request.PageSize)
                                                    .OrderBy(u => u.Id)
                                                    .Select(user => new QueryUserResponseInternal
                                                                    {
                                                                        UserName = user.UserName,
                                                                        Email = user.Email
                                                                    }).ToList()
                                    };


            return queryUserResponse;
        }

        public CreateUserResponse CreateUser(CreateUserRequest request)
        {
            var createUserRequestValidator = _validatorResolver.Resolve<CreateUserRequestValidator>();
            ValidationResult validationResult = createUserRequestValidator.Validate(request);
            Guard.IsFalse(validationResult.IsValid, new CustomApiException(validationResult.ToString(), HttpStatusCode.B21Request));

            User deletedUser = _dataContext.Users.IgnoreQueryFilters().FirstOrDefault(u => u.Email == request.Email && u.IsDeleted);
            Guard.IsNotNull(deletedUser, new CustomApiException(StringResources.DeletedUserHasEmail, HttpStatusCode.B21Request));

            var user = new User
                       {
                           Email = request.Email,
                           UserName = request.UserName,
                           PasswordHash = _cryptoEngine.Hashing(request.Password),
                       };

            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();

            return new CreateUserResponse
                   {
                       Email = user.Email,
                       UserName = user.UserName,
                       Id = user.Id
                   };
        }

        public GetUserRolesResponse GetUserRoles(long userId)
        {
            GetUserResponse getUserResponse = GetUserById(userId);

            IQueryable<UserRoles> userClaims = _dataContext.UserRoles.Where(claim => claim.UserId == getUserResponse.Id);

            IQueryable<Role> userRoles = _dataContext.Roles.Where(role => userClaims.Any(claim => claim.RoleId == role.Id));

            var response = new GetUserRolesResponse
                           {
                               Roles = userRoles.Select(role =>
                                                            new GetUserRolesResponseInternal
                                                            {
                                                                RoleId = role.Id,
                                                                RoleName = role.RoleName
                                                            })
                                                .ToList()
                           };

            return response;
        }
    }
}