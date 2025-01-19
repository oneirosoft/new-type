namespace Oneiro.Json;

using System.Text.Json;
using System.Text.Json.Serialization;

public sealed class NewTypeJsonConverter : JsonConverterFactory {
    public override bool CanConvert(Type typeToConvert) {
        var currentType = typeToConvert;
        while (currentType != null && currentType != typeof(object)) {
            if (currentType.IsGenericType && 
                typeof(NewType<,>).IsAssignableFrom(currentType.GetGenericTypeDefinition())
            ) {
                return true;
            }
            currentType = currentType.BaseType;
        }
        return false;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
        var genericArguments = typeToConvert.BaseType!.GetGenericArguments();
        var converterType = typeof(NewTypeJsonConverter<,>).MakeGenericType(genericArguments);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

public sealed class NewTypeJsonConverter<T, TNewType> : JsonConverter<TNewType>
    where TNewType : NewType<T, TNewType>
    where T : struct {
    public override bool HandleNull { get; } = true;

    public override TNewType Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options
    ) {
        if (reader.TokenType == JsonTokenType.Null) {
            return NewType<T, TNewType>.Default;
        }
        var value = JsonSerializer.Deserialize<T>(ref reader, options);
        return NewType<T, TNewType>.From(value);
    }

    public override void Write(Utf8JsonWriter writer, TNewType value, JsonSerializerOptions options) {
        JsonSerializer.Serialize(writer, value.GetValue(), options);
    }
}