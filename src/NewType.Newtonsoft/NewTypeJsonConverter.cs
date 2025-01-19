using System;
using Newtonsoft.Json;

namespace Oneiro;

public sealed class NewTypeJsonConverter : JsonConverter {

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        if (value is null) {
            writer.WriteNull();
            return;
        }
        
        var typeToConvert = value.GetType().BaseType!.GetGenericArguments();
        var typeConverter = typeof(NewTypeJsonConverter<,>)
            .MakeGenericType(typeToConvert);
        var converter = Activator.CreateInstance(typeConverter, []);
        var writeJson = typeConverter.GetMethod("WriteJson", [typeof(JsonWriter), value.GetType(), typeof(JsonSerializer)]);
        writeJson!.Invoke(converter, [writer, value, serializer]);
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer
    ) {
        var typeToConvert = objectType.BaseType!.GetGenericArguments();
        var typeConverter = typeof(NewTypeJsonConverter<,>)
            .MakeGenericType(typeToConvert);
        var converter = Activator.CreateInstance(typeConverter, []);
        var readJson = typeConverter.GetMethod("ReadJson", [typeof(JsonReader), typeof(Type), objectType, typeof(bool), typeof(JsonSerializer)]);
        var convertedValue = readJson!.Invoke(converter, [reader, objectType, existingValue, false, serializer]);
        return convertedValue;
    }
    
    public override bool CanConvert(Type objectType) =>
        typeof(NewType<,>).IsAssignableFrom(objectType);
}

public sealed class NewTypeJsonConverter<T, TNewType> : JsonConverter<TNewType>
    where TNewType : NewType<T, TNewType>
    where T : struct {
    
    public override TNewType ReadJson(JsonReader reader, Type objectType, TNewType existingValue, bool hasExistingValue, JsonSerializer serializer) {
        if(reader.TokenType == JsonToken.Null) {
            return NewType<T, TNewType>.Default;
        }
        var value = serializer.Deserialize<T>(reader);
        return NewType<T, TNewType>.From(value);
    }

    public override void WriteJson(JsonWriter writer, TNewType value, JsonSerializer serializer) {
        serializer.Serialize(writer, value.GetValue());
    }
}