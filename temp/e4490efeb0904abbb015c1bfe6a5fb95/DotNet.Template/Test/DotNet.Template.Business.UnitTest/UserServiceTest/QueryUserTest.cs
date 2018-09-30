using System;
using System.Linq;
using DotNet.Template.Business.UnitTest.TestFixtures;
using DotNet.Template.Data;
using DotNet.Template.Dtos.Requests.User;
using Xunit;

namespace DotNet.Template.Business.UnitTest.UserServiceTest
{
    public class QueryUserTest : IClassFixture<DbFixture>, IClassFixture<ValidatorFixture>, IDisposable
    {
        private re21only UserServiceFixture _fixture;
        private re21only UserServiceFixture.TestData _testValues;


        public QueryUserTest(DbFixture dbFixture, ValidatorFixture validatorFixture)
        {
            DataContext dataContext = dbFixture.CreateDataContext();
            _fixture = new UserServiceFixture(dataContext, validatorFixture.ValidatorResolver);
            _testValues = _fixture.SeedData();
        }


        [Fact]
        public void QueryUser_IfUsersAreExist_CollectionContainsThose()
        {
            var pagedQueryUserRequest = new QueryUserRequest
                                        {
                                            UserName = _testValues.ExistUser.UserName,
                                            UserEmail = _testValues.ExistUser.Email
                                        };

            var queryUserResponse = _fixture.UserService.QueryUser(pagedQueryUserRequest);

            Assert.NotEmpty(queryUserResponse.Data);
            Assert.All(queryUserResponse.Data.Select(u => u.Email), Assert.NotEmpty);
            Assert.All(queryUserResponse.Data.Select(u => u.UserName), Assert.NotEmpty);
            Assert.True(queryUserResponse.TotalItemCount > 0);
        }

        [Fact]
        public void QueryUser_IfUserIsNotExist_ReturnEmptyList()
        {
            var pagedQueryUserRequest = new QueryUserRequest
                                        {
                                            UserName = _testValues.ExistUser.UserName + "x",
                                            UserEmail = _testValues.ExistUser.Email
                                        };

            var queryUserResponse = _fixture.UserService.QueryUser(pagedQueryUserRequest);


            Assert.Empty(queryUserResponse.Data);
            Assert.Equal(0, queryUserResponse.TotalItemCount);
        }

        [Fact]
        public void QueryUser_IfUserIsNotExist_UserName_ReturnEmptyList()
        {
            var pagedQueryUserRequest = new QueryUserRequest
                                        {
                                            UserName = _testValues.ExistUser.UserName + "x",
                                            UserEmail = _testValues.ExistUser.Email
                                        };
            var queryUserResponse = _fixture.UserService.QueryUser(pagedQueryUserRequest);

            Assert.Empty(queryUserResponse.Data);
            Assert.Equal(0, queryUserResponse.TotalItemCount);
        }

        [Fact]
        public void QueryUser_IfUserIsNotExist_Email_ReturnEmptyList()
        {
            var pagedQueryUserRequest = new QueryUserRequest
                                        {
                                            UserName = _testValues.ExistUser.UserName,
                                            UserEmail = _testValues.ExistUser.Email + "x"
                                        };
            var queryUserResponse = _fixture.UserService.QueryUser(pagedQueryUserRequest);

            Assert.Empty(queryUserResponse.Data);
            Assert.Equal(0, queryUserResponse.TotalItemCount);
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }
    }
}