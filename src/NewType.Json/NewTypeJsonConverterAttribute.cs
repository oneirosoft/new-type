using System.Text.Json.Serialization;

namespace Oneiro.Json;

public class NewTypeJsonConverterAttribute<T, TNewType>()
    : JsonConverterAttribute(typeof(NewTypeJsonConverter<T, TNewType>))
    where TNewType : NewType<T, TNewType>
    where T : struct;