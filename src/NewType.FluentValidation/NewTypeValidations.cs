using FluentValidation;
using FluentValidation.Results;

namespace Oneiro;

public static class NewTypeValidations {
    public static IValidator<TNewType> CreateValidatorFor<T, TNewType>()
        where TNewType : ValidatedNewType<T, TNewType>
        where T : struct
    => new ValidatedNewTypeValidator<T, TNewType>();
    
    public static IValidator<TNewType> CreateValidatorFor<T, TNewType>(Action<IRuleBuilderInitial<TNewType, T>> builder)
        where TNewType : NewType<T, TNewType>
        where T : struct {
        var validator = new InlineValidator<TNewType>();
        builder(validator.RuleFor(x => (T) x));
        return validator;
    }
}

internal sealed class ValidatedNewTypeValidator<T, TNewType> : IValidator<TNewType>
    where TNewType : ValidatedNewType<T, TNewType>
    where T : struct {
    
    public ValidationResult Validate(IValidationContext context) {
        if (context.InstanceToValidate is not TNewType instance)
            return new ValidationResult([
                new ValidationFailure("", "Invalid instance type")
            ]);

        if (!instance.HasErrors())
            return new ValidationResult();
        
        if (!context.ThrowOnFailures)
            return Validate(instance);
        
        var message = $"Validation for {{{typeof(TNewType)}}} was not successful";
        var errors = instance.GetErrors()
            .Select(e => new ValidationFailure(e.RuleName, e.ErrorMessage, e.Value));

        throw new ValidationException(message, errors);
    }

    public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = new()) =>
        Task.Run(() => Validate(context), cancellation);

    public IValidatorDescriptor CreateDescriptor() =>
        new ValidatorDescriptor<TNewType>([]);

    public bool CanValidateInstancesOfType(Type type) =>
        typeof(TNewType) == type;

    public ValidationResult Validate(TNewType instance) =>
        new (
            instance.GetErrors()
                .Select(e => new ValidationFailure(e.RuleName, e.ErrorMessage, e.Value))
        );

    public Task<ValidationResult> ValidateAsync(
        TNewType instance,
        CancellationToken cancellation = new ()
    ) => Task.Run(() => Validate(instance), cancellation);
}
