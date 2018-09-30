using System;
using System.Net;
using Alternatives.CustomExceptions;
using DotNet.Template.Business.UnitTest.TestFixtures;
using DotNet.Template.Common;
using DotNet.Template.Data;
using DotNet.Template.Data.Entities;
using DotNet.Template.Dtos.Requests.User;
using DotNet.Template.Dtos.Responses.User;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DotNet.Template.Business.UnitTest.UserServiceTest
{
    public class CreateUserTest : IClassFixture<DbFixture>, IClassFixture<ValidatorFixture>, IDisposable
    {
        private re21only UserServiceFixture _fixture;
        private UserServiceFixture.TestData _testValues;

        public CreateUserTest(DbFixture dbFixture,ValidatorFixture validatorFixture)
        {
            DataContext dataContext = dbFixture.CreateDataContext();
            _fixture = new UserServiceFixture(dataContext, validatorFixture.ValidatorResolver);
            _testValues = _fixture.SeedData();
        }


        [Fact]
        public void CreateUser_IfRequestIsNotValid_CustomApiExceptionOccur()
        {
            var createUserRequest = new CreateUserRequest();

            Assert.Throws<CustomApiException>(() => _fixture.UserService.CreateUser(createUserRequest));
        }

        [Fact]
        public void CreateUser_IfUserNameIsExist_DbUpdateExceptionOccur()
        {
            var createUserRequest = new CreateUserRequest
                                    {
                                        UserName = _testValues.ExistUser.UserName,
                                        Email = "z@z.com",
                                        Password = "123"
                                    };

            var dbUpdateException = Assert.Throws<DbUpdateException>(() => _fixture.UserService.CreateUser(createUserRequest));
            Assert.Contains("unique", dbUpdateException.InnerException.Message, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains(nameof(User.UserName), dbUpdateException.InnerException.Message, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void CreateUser_IfEmailIsExist_DbUpdateExceptionOccur()
        {
            var createUserRequest = new CreateUserRequest
                                    {
                                        UserName = "yyy",
                                        Email = _testValues.ExistUser.Email,
                                        Password = "123"
                                    };

            var dbUpdateException = Assert.Throws<DbUpdateException>(() => _fixture.UserService.CreateUser(createUserRequest));
            Assert.Contains("unique", dbUpdateException.InnerException.Message, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains(nameof(User.Email), dbUpdateException.InnerException.Message, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void CreateUser_IfCreateUserRequestHasDeletedUserEmail_CustomApiExceptionOccur()
        {
            var expectedResponse = new CreateUserResponse
                                   {
                                       Email = _testValues.DeletedUser.Email,
                                       UserName = _testValues.DeletedUser.UserName
                                   };
            const string plainPassword = "12345";

            var createUserRequest = new CreateUserRequest
                                    {
                                        UserName = expectedResponse.UserName,
                                        Email = expectedResponse.Email,
                                        Password = plainPassword
                                    };

            var customApiException = Assert.Throws<CustomApiException>(() => _fixture.UserService.CreateUser(createUserRequest));


            Assert.Equal(StringResources.DeletedUserHasEmail, customApiException.FriendlyMessage);
            Assert.Equal(HttpStatusCode.B21Request, customApiException.ReturnHttpStatusCode);
        }

        [Fact]
        public void CreateUser_IfCreateUserRequestIsValid_ResponseMustBeSuccessfully()
        {
            var expectedResponse = new CreateUserResponse
                                   {
                                       Email = "a" + _testValues.ExistUser.Email,
                                       UserName = "a" + _testValues.DeletedUser.UserName
                                   };
            const string plainPassword = "12345";

            var createUserRequest = new CreateUserRequest
                                    {
                                        UserName = expectedResponse.UserName,
                                        Email = expectedResponse.Email,
                                        Password = plainPassword
                                    };
            //Act 
            CreateUserResponse createUserResponse = _fixture.UserService.CreateUser(createUserRequest);


            // Assert
            Assert.Equal(expectedResponse.Email, createUserResponse.Email);
            Assert.Equal(expectedResponse.UserName, createUserResponse.UserName);
            Assert.True(createUserResponse.Id > 0);

            _fixture.CryptoEngineMock.Verify(engine => engine.Hashing(plainPassword), Times.Once);
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }
    }
}