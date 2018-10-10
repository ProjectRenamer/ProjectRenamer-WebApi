using FluentValidation;
using FluentValidation.Results;

namespace ProjectRenamer.Api.Helper
{
    public static class PreValidations
    {
        private const string NullObjectMessage = "Object should not be null";

        public static bool NullPreValidation<T>(ValidationContext<T> context, ValidationResult validationResult)
        {
            bool result = true;
            if (context.InstanceToValidate == null)
            {
                validationResult.Errors.Add(new ValidationFailure(typeof(T).Name, NullObjectMessage));
                result = false;
            }

            return result;
        }
    }
}