namespace Oneiro.Tests;

public sealed class ValidatedNewTypeTests {
    [Test]
    public async Task ValidatableNewType_With_Valid_Value() {
        var actual = ValidatedWrappedInt.From(42);
        await Assert.That(actual.HasErrors()).IsFalse();
        await Assert.That(actual.GetErrors()).IsEmpty();
        await Assert.That(actual).IsEqualTo(ValidatedWrappedInt.From(42));
    }
    
    [Test]
    public async Task ValidatableNewType_With_Invalid_Value() {
        var actual = ValidatedWrappedInt.From(-42);
        await Assert.That(actual.HasErrors()).IsTrue();
        await Assert.That(actual.GetErrors()).HasCount().EqualTo(1);
        await Assert.That(actual.GetErrors().First().ErrorMessage)
            .IsEqualTo("[Violation of rule {Value}]: Value must be greater than 0");
        await Assert.That(actual.IsDefault).IsTrue();
    }
    
    [Test]
    public void ThrowIfErrored_With_Invalid_Value() {
        Assert.Throws<NewTypeValidationException>(
            () => ValidatedWrappedInt.From(-42).ThrowIfErrored()
        );
    }
    
    [Test]
    public async Task IfErrored_With_Invalid_Value() {
        var actual = ValidatedWrappedInt.From(-42);
        var otherActual = actual.IfErrored(_ => ValidatedWrappedInt.From(42));
        await Assert.That(actual.GetErrors()).HasCount().EqualTo(1);
        await Assert.That(actual.GetErrors().First().ErrorMessage)
            .IsEqualTo("[Violation of rule {Value}]: Value must be greater than 0");
        await Assert.That(otherActual).IsEqualTo(ValidatedWrappedInt.From(42));
    }

    [Test]
    public async Task Rules_Dont_Conflict() {
        var a1 = ValidatedWrappedInt.From(-42);
        var a2 = ValidatedWrappedLong.From(-42);
        await Assert.That(a1.HasErrors()).IsTrue();
        await Assert.That(a2.HasErrors()).IsFalse();
    }
    
    private sealed 
#if NET6_0_OR_GREATER
        record
#else
        class
#endif
    ValidatedWrappedInt : ValidatedNewType<int, ValidatedWrappedInt> {
        private ValidatedWrappedInt(int value) : base(value) {
            RuleFor("Value", v => v > 0, "Value must be greater than 0");
        }
    }

    private sealed
#if NET6_0_OR_GREATER
        record
#else
        class
#endif
        ValidatedWrappedLong : ValidatedNewType<long, ValidatedWrappedLong> {
        private ValidatedWrappedLong(long value) : base(value) {
            RuleFor("Value", v => v > -43, "Value must be greater than -43");
        }
    }
}