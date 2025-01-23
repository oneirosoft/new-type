using System.Globalization;

namespace Oneiro.Tests;

public sealed class NewTypeTests {

    [Test]
    public async Task From() {
        var actual = WrappedInt.From(42);
        await Assert.That(actual).IsEqualTo(WrappedInt.From(42));
    }
    
    [Test]
    public async Task TryParse_Is_Successful() {
        var actual = WrappedInt.TryParse("42", out var result);
        await Assert.That(actual).IsTrue();
        await Assert.That(result).IsEqualTo(WrappedInt.From(42));
    }
    
    [Test]
    public async Task TryParse_Is_Unsuccessful() {
        var actual = WrappedInt.TryParse("not a number", out var result);
        await Assert.That(actual).IsFalse();
        await Assert.That(result).IsEqualTo(WrappedInt.Default);
    }
    
    [Test]
    public async Task Can_Be_Casted_To_Underlying_Type() {
        var actual = (int)WrappedInt.From(42);
        await Assert.That(actual).IsEqualTo(42);
    }
    
    [Test]
    public async Task Pipe_Operator_Returns_Wrapped_Value() {
        var actual = WrappedInt.From(42) | 0;
        await Assert.That(actual).IsEqualTo(42);
    }
    
    [Test]
    public async Task Pipe_Operator_Returns_Other_Value() {
        var actual = WrappedInt.Default | 0;
        await Assert.That(actual).IsEqualTo(0);
    }
    
    [Test]
    public async Task Pipe_Returns_Non_Default_Value() {
        var actual = WrappedInt.From(42) | WrappedInt.Default;
        await Assert.That(actual).IsEqualTo(WrappedInt.From(42));
    }
    
    [Test]
    public async Task Pipe_Returns_Other_Value() {
        var actual = WrappedInt.Default | WrappedInt.From(42);
        await Assert.That(actual).IsEqualTo(WrappedInt.From(42));
    }
    
    [Test]
    public async Task Equals_Operator_Returns_True() {
        var expected = WrappedInt.From(42);
        var actual = WrappedInt.From(42);
        await Assert.That(actual == expected).IsTrue();
    }
    
    [Test]
    public async Task Equals() {
        var actual = WrappedInt.From(42).Equals(WrappedInt.From(42));
        await Assert.That(actual).IsTrue();
    }
    
    [Test]
    public async Task Equals_Operator_Returns_False() {
        var actual = WrappedInt.From(42) == WrappedInt.Default;
        await Assert.That(actual).IsFalse();
    }
    
    [Test]
    public async Task Not_Equals_Operator_Returns_True() {
        var actual = WrappedInt.From(42) != WrappedInt.Default;
        await Assert.That(actual).IsTrue();
    }
    
    [Test]
    public async Task Not_Equals_Operator_Returns_False() {
        var actual = WrappedInt.From(42);
        var expected = WrappedInt.From(42);
        await Assert.That(actual != expected).IsFalse();
    }

    [Test]
    [Arguments("C")]
    [Arguments("D4")]
    [Arguments(null)]
    public async Task NewType_Can_Be_Formatted(string? format) {
        var actual = WrappedInt.From(42).ToString(format, new CultureInfo("en-US"));
        await (format switch {
            "C" => async () => await Assert.That(actual).IsEqualTo("$42.00"),
            "D4" => async () => await Assert.That(actual).IsEqualTo("0042"),
            _ => (Func<Task<string?>>) (async () => await Assert.That(actual).IsEqualTo("42"))
        })();
    }
    
    private sealed 
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
        WrappedInt : NewType<int, WrappedInt> {
        public WrappedInt(int value) : base(value) { }
    }
}