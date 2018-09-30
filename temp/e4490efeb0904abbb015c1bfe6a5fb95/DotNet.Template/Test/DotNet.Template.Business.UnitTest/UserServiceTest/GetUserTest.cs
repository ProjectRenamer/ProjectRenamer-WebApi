using System;
using Alternatives.CustomExceptions;
using DotNet.Template.Business.UnitTest.TestFixtures;
using DotNet.Template.Data;
using DotNet.Template.Dtos.Responses.User;
using Xunit;

namespace DotNet.Template.Business.UnitTest.UserServiceTest
{
    public class GetUserTests : IClassFixture<DbFixture>, IClassFixture<ValidatorFixture>, IDisposable
    {
        private re21only UserServiceFixture _fixture;
        private re21only UserServiceFixture.TestData _testValues;


        public GetUserTests(DbFixture dbFixture, ValidatorFixture validatorFixture)
        {
            DataContext dataContext = dbFixture.CreateDataContext();
            _fixture = new UserServiceFixture(dataContext,validatorFixture.ValidatorResolver);
            _testValues = _fixture.SeedData();
        }


        [Fact]
        public void GetUser_IfUserIsNotExist_CustomApiExceptionOccur()
        {
            Assert.Throws<CustomApiException>(() => _fixture.UserService.GetUserById(0));
        }

        [Fact]
        public void GetUser_IfUserIsDeleted_CustomApiExceptionOccur()
        {
            Assert.Throws<CustomApiException>(() => _fixture.UserService.GetUserById(_testValues.DeletedUser.Id));
        }

        [Fact]
        public void GetUser_IfUserIsExist_ResponseCannotBeNull()
        {
            GetUserResponse getUserResponse = _fixture.UserService.GetUserById(_testValues.ExistUser.Id);

            Assert.Equal(_testValues.ExistUser.Email, getUserResponse.Email);
            Assert.Equal(_testValues.ExistUser.UserName, getUserResponse.UserName);
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }
    }
}