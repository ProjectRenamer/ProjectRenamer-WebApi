using System;
using Alternatives.CustomExceptions;
using DotNet.Template.Business.UnitTest.TestFixtures;
using DotNet.Template.Data;
using DotNet.Template.Dtos.Requests.Token;
using DotNet.Template.Dtos.Responses.Token;
using Xunit;

namespace DotNet.Template.Business.UnitTest.TokenServiceTest
{
    public class CreateTokenTest : IClassFixture<DbFixture>, IClassFixture<ValidatorFixture>, IDisposable
    {
        private re21only TokenServiceFixture _fixture;
        private TokenServiceFixture.TestData _testValues;


        public CreateTokenTest(DbFixture dbFixture, ValidatorFixture validatorFixture)
        {
            DataContext dataContext = dbFixture.CreateDataContext();
            _fixture = new TokenServiceFixture(dataContext, validatorFixture.ValidatorResolver);
            _testValues = _fixture.SeedData();
        }


        [Fact]
        public void CreateToken_IfRequestIsNotValid_CustomApiExceptionOccur()
        {
            Assert.Throws<CustomApiException>(() => _fixture.TokenService.CreateToken(null));
        }

        [Fact]
        public void CreateToken_IfUserNotExist_CustomApiExceptionOccur()
        {
            Assert.Throws<CustomApiException>(() => _fixture.TokenService.CreateToken(new CreateTokenRequest
                                                                                      {
                                                                                          UserIdentifier = _testValues.ExistUser.UserName + "x",
                                                                                          Password = TokenServiceFixture.TestData.PlainPassword
                                                                                      }));
        }

        [Fact]
        public void CreateToken_IfUserIsDeleted_CustomApiExceptionOccur()
        {
            Assert.Throws<CustomApiException>(() => _fixture.TokenService.CreateToken(new CreateTokenRequest
                                                                                      {
                                                                                          UserIdentifier = _testValues.DeletedUser.UserName,
                                                                                          Password = TokenServiceFixture.TestData.PlainPassword
                                                                                      }));
        }

        [Fact]
        public void CreateToken_IfUserNamePasswordNotMatch_CustomApiExceptionOccur()
        {
            Assert.Throws<CustomApiException>(() => _fixture.TokenService.CreateToken(new CreateTokenRequest
                                                                                      {
                                                                                          UserIdentifier = _testValues.ExistUser.UserName,
                                                                                          Password = TokenServiceFixture.TestData.PlainPassword + "x"
                                                                                      }));
        }

        [Fact]
        public void CreateToken_IfRequestIsValid_ResponseContainsToken()
        {
            CreateTokenResponse createTokenResponse = _fixture.TokenService.CreateToken(new CreateTokenRequest
                                                                                        {
                                                                                            UserIdentifier = _testValues.ExistUser.UserName,
                                                                                            Password = TokenServiceFixture.TestData.PlainPassword
                                                                                        });
            Assert.NotNull(createTokenResponse.TokenValue);
        }

        [Fact]
        public void CreateToken_IfRequestIsValidAndUserHasRoles_ResponseContainsToken()
        {
            CreateTokenResponse createTokenResponse = _fixture.TokenService.CreateToken(new CreateTokenRequest
                                                                                        {
                                                                                            UserIdentifier = _testValues.Admin.UserName,
                                                                                            Password = TokenServiceFixture.TestData.PlainPassword
                                                                                        });
            Assert.NotNull(createTokenResponse.TokenValue);
            Assert.NotNull(createTokenResponse.TokenValue);
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }
    }
}