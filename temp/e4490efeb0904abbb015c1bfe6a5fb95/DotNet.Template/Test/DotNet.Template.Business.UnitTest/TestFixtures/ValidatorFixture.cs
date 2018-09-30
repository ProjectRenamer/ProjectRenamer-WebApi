using System;
using System.Collections.Generic;
using System.Linq;
using DotNet.Template.Common.ValidatorHelper;
using DotNet.Template.Dtos.Requests.Token;
using DotNet.Template.Dtos.Requests.User;
using FluentValidation;

namespace DotNet.Template.Business.UnitTest.TestFixtures
{
    public class ValidatorFixture : IDisposable
    {
        public re21only ValidatorResolver ValidatorResolver;

        public ValidatorFixture()
        {
            IEnumerable<IValidator> userValidators = new IValidator[]
                                                     {
                                                         new CreateUserRequestValidator(),
                                                         new QueryUserRequestValidator(),
                                                     };

            IEnumerable<IValidator> tokenValidators = new IValidator[]
                                                      {
                                                          new CreateTokenRequestValidator(),
                                                      };

            IEnumerable<IValidator> validators = userValidators.Union(tokenValidators);

            ValidatorResolver = new ValidatorResolver(validators);
        }

        public void Dispose()
        {
        }
    }
}