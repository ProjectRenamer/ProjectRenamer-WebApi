using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentValidation;
using ProjectRenamer.Api.Helper;
using ProjectRenamer.Api.Requests;

namespace ProjectRenamer.Api.Test.Fixture
{
    public class ValidationFixture : IDisposable
    {
        public ValidatorResolver ValidatorResolver;

        public ValidationFixture()
        {
            IEnumerable<Type> validatorTypes = Startup.GetValidators(Startup.GetAssemblies());
            var validators = new List<IValidator>();

            foreach (Type validatorType in validatorTypes)
            {
                var v = Alternatives.Extensions.ReflectionExtensions.CreateInstance(validatorType) as IValidator;
                validators.Add(v);
            }

            ValidatorResolver = new ValidatorResolver(validators);
        }

        public void Dispose()
        {
        }
    }
}