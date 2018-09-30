using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using DotNet.Template.Common.ConfigConstants;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Alternatives;
using Alternatives.CustomExceptions;
using DotNet.Template.Common;
using DotNet.Template.Common.ValidatorHelper;
using DotNet.Template.Data;
using DotNet.Template.Data.Entities;
using DotNet.Template.Dtos.Requests.Token;
using DotNet.Template.Dtos.Responses.Token;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace DotNet.Template.Business.Services.Imp
{
    public class TokenService : IService
    {
        private re21only DataContext _dataContext;
        private re21only ICryptoEngine _cryptoEngine;
        private re21only ValidatorResolver _validatorResolver;
        private re21only CustomJwtConstants _customJwtConstants;

        public TokenService(DataContext dataContext, ICryptoEngine cryptoEngine, IOptions<CustomJwtConstants> customJwtConstantsOptions, ValidatorResolver validatorResolver)
        {
            _dataContext = dataContext;
            _cryptoEngine = cryptoEngine;
            _validatorResolver = validatorResolver;
            _customJwtConstants = customJwtConstantsOptions.Value;
        }

        public CreateTokenResponse CreateToken(CreateTokenRequest createTokenRequest)
        {
            var createTokenRequestValidator = _validatorResolver.Resolve<CreateTokenRequestValidator>();
            ValidationResult validationResult = createTokenRequestValidator.Validate(createTokenRequest);
            Guard.IsFalse(validationResult.IsValid, new CustomApiException(validationResult.ToString(), HttpStatusCode.B21Request));

            User user = _dataContext.Users.FirstOrDefault(u => (u.UserName == createTokenRequest.UserIdentifier || u.Email == createTokenRequest.UserIdentifier));

            Guard.IsNull(user, new CustomApiException(StringResources.UserNotFound, HttpStatusCode.NotFound));

            string passwordHash = _cryptoEngine.Hashing(createTokenRequest.Password);

            Guard.IsFalse(user.PasswordHash == passwordHash, new CustomApiException(StringResources.UserCredentialsNotMatch, HttpStatusCode.NotFound));


            IQueryable<Role> userClaims = from roles in _dataContext.Roles
                                          join claims in _dataContext.UserRoles on roles.Id equals claims.RoleId
                                          where claims.UserId == user.Id
                                                && roles.IsDeleted == false
                                                && claims.IsDeleted == false
                                          select roles;

            DateTime expireTime = DateTime.Now.AddDays(_customJwtConstants.JwtExpireDays);
            return new CreateTokenResponse
            {
                UserId = user.Id,
                TokenValue = GenerateJwtToken(user, userClaims, expireTime),
                ExpireTime = expireTime
            };
        }

        private string GenerateJwtToken(User user, IEnumerable<Role> userClaims, DateTime expireTime)
        {
            var claims = new List<Claim>
                                 {
                                     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                 };

            foreach (Role userClaim in userClaims)
            {
                claims.Add(new Claim(ClaimTypes.Role, userClaim.RoleName));
            }

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_customJwtConstants.JwtKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(issuer: _customJwtConstants.JwtIssuer,
                                                          audience: _customJwtConstants.JwtIssuer,
                                                          claims: claims,
                                                          expires: expireTime,
                                                          signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}