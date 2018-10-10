using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace ProjectRenamer.Api.Helper
{
    public class ValidatorResolver
    {
        private readonly IEnumerable<IValidator> _validators;

        public ValidatorResolver(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }

        public T Resolve<T>() where T : class
        {
            return Resolve<T>(true);
        }

        public T Resolve<T>(bool throwIfNotFound) where T : class
        {
            var validator = _validators.FirstOrDefault(v => v is T) as T;

            if (throwIfNotFound && validator == null)
                throw new ArgumentNullException($"{typeof(T).Name} cannot be resolved");

            return validator;
        }
    }
}