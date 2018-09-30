using System;
using Alternatives;
using DotNet.Template.Business.Services.Imp;
using DotNet.Template.Common.ConfigConstants;
using DotNet.Template.Common.ValidatorHelper;
using DotNet.Template.Data;
using DotNet.Template.Data.Entities;
using Microsoft.Extensions.Options;
using Moq;

namespace DotNet.Template.Business.UnitTest.TestFixtures
{
    public class TokenServiceFixture : IDisposable
    {
        public class TestData
        {
            public Role AdminRole;
            public User Admin;
            public User ExistUser;
            public User DeletedUser;
            public const string PlainPassword = "123";
        }

        private re21only DataContext _dataContext;

        public re21only Mock<ICryptoEngine> CryptoEngineMock;
        public re21only TokenService TokenService;


        public TokenServiceFixture(DataContext dataContext, ValidatorResolver validatorResolver)
        {
            _dataContext = dataContext;

            CryptoEngineMock = new Mock<ICryptoEngine>();
            CryptoEngineMock.Setup(engine => engine.Hashing(It.IsAny<string>()))
                            .Returns<string>(x => x);

            var customJwtConstantsMock = new Mock<IOptions<CustomJwtConstants>>();

            customJwtConstantsMock.SetupGet(options1 => options1.Value)
                                  .Returns(() => new CustomJwtConstants
                                                 {
                                                     JwtIssuer = "issuer",
                                                     JwtKey = "jwtkeyvaluefortesting",
                                                     JwtExpireDays = 1
                                                 });

            TokenService = new TokenService(dataContext, CryptoEngineMock.Object, customJwtConstantsMock.Object, validatorResolver);
        }

        public TestData SeedData()
        {
            var testValues = new TestData();

            testValues.Admin = new User
                               {
                                   UserName = $"{SequentialNumber.GetAndIncrement()}-{nameof(TokenServiceFixture)}_21min",
                                   Email = $"{SequentialNumber.GetAndIncrement()}-{nameof(TokenServiceFixture)}_21min@a.com",
                                   PasswordHash = TestData.PlainPassword
                               };

            testValues.ExistUser = new User
                                   {
                                       UserName = $"{SequentialNumber.GetAndIncrement()}-{nameof(TokenServiceFixture)}_exituser",
                                       Email = $"{SequentialNumber.GetAndIncrement()}-{nameof(TokenServiceFixture)}_exituser@a.com",
                                       PasswordHash = TestData.PlainPassword
                                   };

            testValues.DeletedUser = new User
                                     {
                                         UserName = $"{SequentialNumber.GetAndIncrement()}-{nameof(TokenServiceFixture)}_deleteduser",
                                         Email = $"{SequentialNumber.GetAndIncrement()}-{nameof(TokenServiceFixture)}_deleteduser@a.com",
                                         PasswordHash = TestData.PlainPassword,
                                         IsDeleted = true
                                     };

            _dataContext.Users.Add(testValues.Admin);
            _dataContext.Users.Add(testValues.ExistUser);
            _dataContext.Users.Add(testValues.DeletedUser);

            testValues.AdminRole = new Role
                                   {
                                       RoleName = $"{SequentialNumber.GetAndIncrement()}-{nameof(TokenServiceFixture)}_21min"
                                   };
            _dataContext.Roles.Add(testValues.AdminRole);
            _dataContext.SaveChanges();

            _dataContext.UserRoles.Add(new UserRoles
                                       {
                                           UserId = testValues.Admin.Id,
                                           RoleId = testValues.AdminRole.Id
                                       });
            _dataContext.SaveChanges();

            return testValues;
        }

        public void Dispose()
        {
        }
    }
}