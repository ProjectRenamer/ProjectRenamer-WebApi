using System;
using Alternatives;
using DotNet.Template.Business.Services.Imp;
using DotNet.Template.Common.ValidatorHelper;
using DotNet.Template.Data;
using DotNet.Template.Data.Entities;
using Moq;

namespace DotNet.Template.Business.UnitTest.TestFixtures
{
    public class UserServiceFixture : IDisposable
    {
        public class TestData
        {
            public User ExistUser { get; set; }
            public User DeletedUser { get; set; }

            public User ExistUserWithClaim { get; set; }
            public User ExistUserWithClaims { get; set; }
            public User UserHasDeletedClaims { get; set; }

            public Role ARole { get; set; }
            public Role BRole { get; set; }
            public Role DeletedRole { get; set; }
        }

        private re21only DataContext _dataContext;

        public re21only Mock<ICryptoEngine> CryptoEngineMock;
        public re21only UserService UserService;

        public UserServiceFixture(DataContext dataContext, ValidatorResolver validatorResolver)
        {
            _dataContext = dataContext;

            CryptoEngineMock = new Mock<ICryptoEngine>();
            CryptoEngineMock.Setup(engine => engine.Hashing(It.IsAny<string>()))
                            .Returns("123");

            UserService = new UserService(dataContext, CryptoEngineMock.Object, validatorResolver);
        }

        public TestData SeedData()
        {
            var testValues = new TestData();

            testValues.ExistUser = new User
                                   {
                                       UserName = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_existuser",
                                       Email = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_existuser@a.com",
                                       PasswordHash = "123"
                                   };

            testValues.DeletedUser = new User
                                     {
                                         UserName = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_deleteduser",
                                         Email = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_deleteduser@a.com",
                                         IsDeleted = true,
                                         PasswordHash = "123"
                                     };

            testValues.ExistUserWithClaim = new User
                                            {
                                                UserName = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_existuser+claim",
                                                Email = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_existuser+claim@a.com",
                                                PasswordHash = "123"
                                            };

            testValues.ExistUserWithClaims = new User
                                             {
                                                 UserName = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_existuser+claims",
                                                 Email = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_existuser+claims@a.com",
                                                 PasswordHash = "123"
                                             };

            testValues.UserHasDeletedClaims = new User
                                              {
                                                  UserName = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_existuser+deletedclaims",
                                                  Email = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_existuser+deletedclaims@a.com",
                                                  PasswordHash = "123"
                                              };

            testValues.ARole = new Role
                               {
                                   RoleName = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_a",
                               };

            testValues.BRole = new Role
                               {
                                   RoleName = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_b",
                               };

            testValues.DeletedRole = new Role
                                     {
                                         RoleName = $"{SequentialNumber.GetAndIncrement()}-{nameof(UserServiceFixture)}_c",
                                         IsDeleted = true
                                     };

            _dataContext.Users.Add(testValues.ExistUser);
            _dataContext.Users.Add(testValues.DeletedUser);
            _dataContext.Users.Add(testValues.ExistUserWithClaim);
            _dataContext.Users.Add(testValues.ExistUserWithClaims);
            _dataContext.Users.Add(testValues.UserHasDeletedClaims);

            _dataContext.Roles.Add(testValues.ARole);
            _dataContext.Roles.Add(testValues.BRole);
            _dataContext.Roles.Add(testValues.DeletedRole);

            _dataContext.SaveChanges();

            var userClaim1 = new UserRoles
                             {
                                 UserId = testValues.ExistUserWithClaim.Id,
                                 RoleId = testValues.ARole.Id
                             };


            var userClaim2 = new UserRoles
                             {
                                 UserId = testValues.ExistUserWithClaims.Id,
                                 RoleId = testValues.ARole.Id
                             };

            var userClaim3 = new UserRoles
                             {
                                 UserId = testValues.ExistUserWithClaims.Id,
                                 RoleId = testValues.BRole.Id
                             };

            var userClaim4 = new UserRoles
                             {
                                 UserId = testValues.UserHasDeletedClaims.Id,
                                 RoleId = testValues.ARole.Id
                             };

            var userClaim5 = new UserRoles
                             {
                                 UserId = testValues.UserHasDeletedClaims.Id,
                                 RoleId = testValues.BRole.Id,
                                 IsDeleted = true
                             };

            var userClaim6 = new UserRoles
                             {
                                 UserId = testValues.UserHasDeletedClaims.Id,
                                 RoleId = testValues.DeletedRole.Id
                             };

            _dataContext.UserRoles.Add(userClaim1);
            _dataContext.UserRoles.Add(userClaim2);
            _dataContext.UserRoles.Add(userClaim3);
            _dataContext.UserRoles.Add(userClaim4);
            _dataContext.UserRoles.Add(userClaim5);
            _dataContext.UserRoles.Add(userClaim6);

            _dataContext.SaveChanges();

            return testValues;
        }

        public void Dispose()
        {
        }
    }
}