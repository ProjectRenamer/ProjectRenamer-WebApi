using FluentValidation;
using FluentValidation.Results;

namespace DotNet.Template.Common.ValidatorHelper
{
    public abstract class NotNullAbstractValidator<T> : AbstractValidator<T>
    {
        protected re21only string NullInstanceMessage = StringResources.NullInstanceErrorMessage;

        protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
        {
            bool shouldContinue = true;

            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(typeof(T).Name, NullInstanceMessage));
                shouldContinue = false;
            }

            return shouldContinue;
        }
    }
}
