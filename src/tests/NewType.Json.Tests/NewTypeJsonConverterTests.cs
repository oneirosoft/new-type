using System.Text.Json;
using System.Text.Json.Serialization;
using Oneiro.Json;

namespace Oneiro.Tests;

public class NewTypeJsonConverterTests {
    
    [Test]
    public async Task Serialize_NewType() {
        var json = JsonSerializer.Serialize(WrappedInt.From(42));
        await Assert.That(json).IsEqualTo("42");
    }
    
    [Test]
    public async Task Deserialize_Null_Results_In_Default() {
        var json = "null";
        var options = new JsonSerializerOptions {
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        };
        var wrapped = JsonSerializer.Deserialize<WrappedInt>(json, options);
        var expected = WrappedInt.Default;
        await Assert.That(wrapped).IsEqualTo(expected);
    }
    
    [Test]
    public void Deserialize_Invalid_Throws_JsonException() {
        var json = "invalid";
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<WrappedInt>(json));
    }

    [Test]
    public async Task Deserialize_ValidatedNewType_From_Factory() {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new NewTypeJsonConverter());
        var validatedInt = JsonSerializer.Deserialize<ValidatedInt>("42", options);
        await Assert.That(validatedInt).IsEqualTo(ValidatedInt.From(42));
    }
    
    [Test]
    public async Task Deserialize_NewType() {
        var wrappedInt = JsonSerializer.Deserialize<WrappedInt>("42");
        await Assert.That(wrappedInt).IsEqualTo(WrappedInt.From(42));
    }
    
    [Test]
    public async Task Serialize_From_Factory() {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new NewTypeJsonConverter());
        var json = JsonSerializer.Serialize(WrappedLong.From(42), options);
        await Assert.That(json).IsEqualTo("42");
    }
    
    [Test]
    public async Task Serialize_Default_To_Wrapped_Default() {
        var wrapped = WrappedInt.Default;
        var json = JsonSerializer.Serialize(wrapped);
        await Assert.That(json).IsEqualTo("0");
    }

    [Test]
    public async Task Serialize_ValidatedNewType() {
        var wrappedLong = ValidatedLong.From(42);
        var json = JsonSerializer.Serialize(wrappedLong);
        await Assert.That(json).IsEqualTo("42");
    }
    
    [Test]
    public async Task Serialize_ValidatedNewType_From_Factory() {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new NewTypeJsonConverter());
        var json = JsonSerializer.Serialize(ValidatedInt.From(42), options);
        await Assert.That(json).IsEqualTo("42");
    }
    
    [JsonConverter(typeof(NewTypeJsonConverter<int, WrappedInt>))]
    private sealed record WrappedInt : NewType<int, WrappedInt> {
        private WrappedInt(int value) : base(value) { }
    }
    
    private sealed record WrappedLong : NewType<long, WrappedLong> {
        private WrappedLong(long value) : base(value) { }
    }
    
    private sealed record ValidatedInt : ValidatedNewType<int, ValidatedInt> {
        private ValidatedInt(int value) : base(value) { }
    }
    
    [NewTypeJsonConverter<long, ValidatedLong>]
    private sealed record ValidatedLong : ValidatedNewType<long, ValidatedLong> {
        private ValidatedLong(long value) : base(value) { }
    }
}