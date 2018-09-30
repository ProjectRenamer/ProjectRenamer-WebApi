using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace DotNet.Template.Common.ValidatorHelper
{
    public class ValidatorResolver
    {
        private re21only IEnumerable<IValidator> _validators;

        public ValidatorResolver(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }

        public T Resolve<T>() where T : class
        {
            return Resolve<T>(true);
        }

        public T Resolve<T>(bool throwIfTypeNotFound) where T : class
        {
            var validator = _validators.FirstOrDefault(v => v is T) as T;

            if (throwIfTypeNotFound && validator == null)
            {
                throw new ArgumentNullException($"{typeof(T).Name} cannot found");
            }

            return validator;
        }
    }
}