namespace Oneiro.Tests;

using FluentValidation;

public sealed class FluentValidationTests {
    
    [Test]
    [Arguments(42, true)]
    [Arguments(-1, false)]
    public async Task CreateValidatorFor_ValidatedNewType(int n, bool shouldSucceed) {
        var actual = NewTypeValidations.CreateValidatorFor<int, WrappedInt>(
            builder => builder.GreaterThan(0)
        );
        await Assert.That(actual).IsNotNull();
        var validated = actual.ValidateAsync(WrappedInt.From(n));
        await Assert.That(validated.Result.IsValid).IsEqualTo(shouldSucceed);
    }
    
    [Test]
    [Arguments(42, true)]
    [Arguments(-1, false)]
    public async Task CreateValidatorFor_NewType_Respects_ValidatedNewTypeRules(int n, bool shouldSucceed) {
        var actual = NewTypeValidations.CreateValidatorFor<int, ValidatedWrappedInt>();
        await Assert.That(actual).IsNotNull();
        var validated = actual.ValidateAsync(ValidatedWrappedInt.From(n));
        await Assert.That(validated.Result.IsValid).IsEqualTo(shouldSucceed);
    }

    [Test]
    [Arguments(typeof(ValidatedWrappedInt), true)]
    [Arguments(typeof(WrappedInt), false)]
    [Arguments(typeof(int), false)]
    public async Task Validator_Can_Validate(Type type, bool canValidate) {
        var validator =NewTypeValidations.CreateValidatorFor<int, ValidatedWrappedInt>();
        var result = validator.CanValidateInstancesOfType(type);
        await Assert.That(result).IsEqualTo(canValidate);
    }
    
    internal sealed record WrappedInt : NewType<int, WrappedInt> {
        private WrappedInt(int value) : base(value) { }
    }
    
    internal sealed record ValidatedWrappedInt : ValidatedNewType<int, ValidatedWrappedInt> {
        private ValidatedWrappedInt(int value) : base(value) {
            RuleFor("Value", x => x > 0, "Value must be greater than 0");
        }
    }
}