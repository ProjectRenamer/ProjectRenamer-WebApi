using System;
using System.Linq;
using Alternatives.CustomExceptions;
using DotNet.Template.Business.UnitTest.TestFixtures;
using DotNet.Template.Data;
using DotNet.Template.Dtos.Responses.User;
using Xunit;

namespace DotNet.Template.Business.UnitTest.UserServiceTest
{
    public class GetUserRolesTest : IClassFixture<DbFixture>,IClassFixture<ValidatorFixture>, IDisposable
    {
        private re21only UserServiceFixture _fixture;
        private re21only UserServiceFixture.TestData _testValues;

        public GetUserRolesTest(DbFixture dbFixture,ValidatorFixture validatorFixture)
        {
            DataContext dataContext = dbFixture.CreateDataContext();
            _fixture = new UserServiceFixture(dataContext,validatorFixture.ValidatorResolver);
            _testValues = _fixture.SeedData();
        }


        [Fact]
        public void GetUserRoles_IfUserIsNotExist_CustomApiExceptionOccur()
        {
            Assert.Throws<CustomApiException>(() => _fixture.UserService.GetUserRoles(0));
        }

        [Fact]
        public void GetUserRoles_IfUserIsDeleted_CustomApiExceptionOccur()
        {
            Assert.Throws<CustomApiException>(() => _fixture.UserService.GetUserRoles(_testValues.DeletedUser.Id));
        }

        [Fact]
        public void GetUserRoles_IfUserHasNotClaim_ResponseMustBeEmptyList()
        {
            GetUserRolesResponse getUserRolesResponse = _fixture.UserService.GetUserRoles(_testValues.ExistUser.Id);

            Assert.NotNull(getUserRolesResponse);
            Assert.Empty(getUserRolesResponse.Roles);
        }

        [Fact]
        public void GetUserRoles_IfUserHasClaim_ResponseMustBeNotEmptyList()
        {
            GetUserRolesResponse getUserRolesResponse = _fixture.UserService.GetUserRoles(_testValues.ExistUserWithClaim.Id);

            Assert.NotNull(getUserRolesResponse);
            Assert.Single(getUserRolesResponse.Roles);
        }

        [Fact]
        public void GetUserRoles_IfUserHasClaims_ResponseMustBeNotEmptyList()
        {
            GetUserRolesResponse getUserRolesResponse = _fixture.UserService.GetUserRoles(_testValues.ExistUserWithClaims.Id);

            Assert.NotNull(getUserRolesResponse);
            Assert.Equal(2, getUserRolesResponse.Roles.Count);
        }

        [Fact]
        public void GetUserRoles_IfUserHasDeletedClaimsOrDeletedRoles_ResponseMustNotContainsDeletedClaims()
        {
            GetUserRolesResponse getUserRolesResponse = _fixture.UserService.GetUserRoles(_testValues.UserHasDeletedClaims.Id);

            Assert.NotNull(getUserRolesResponse);
            Assert.Single(getUserRolesResponse.Roles);
            Assert.Contains(_testValues.ARole.RoleName, getUserRolesResponse.Roles.Select(r => r.RoleName));
            Assert.DoesNotContain(_testValues.DeletedRole.RoleName, getUserRolesResponse.Roles.Select(r => r.RoleName));
            Assert.DoesNotContain(_testValues.BRole.RoleName, getUserRolesResponse.Roles.Select(r => r.RoleName));
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }
    }
}