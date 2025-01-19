# NewType Newtonsoft

`Oneiro.NewType.Newtonsoft` is a library that extends the core functionality of `Oneiro.NewType.Core` by providing support for JSON serialization and deserialization using the `Newtonsoft.Json` library. This allows you to easily convert your new types to and from JSON.

## Features

- JSON serialization and deserialization support for new types.
- Integration with `Newtonsoft.Json`.

## Installation

To install `NewType.Newtonsoft`, add the following package to your project:

```sh
dotnet add package Oneiro.NewType.Newtonsoft
```

## Usage

### Creating a New Type

To create a new type, inherit from the `NewType<T, TNewType>` class, where `T` is the underlying type and `TNewType` is the new type you are creating.

To enable JSON serialization and deserialization you can add the `NewTypeJsonConverter` attribute to your new type class.

```csharp
[NewTypeJsonConverter<int, WrappedInt>]
public sealed record WrappedInt : NewType<int, WrappedInt> {
    private WrappedInt(int value) : base(value) { }
}
```

You can also use the `JsonConverterAttribute` that comes with `Newtonsoft.Json` to specify a custom JSON converter for your new type.

```csharp
[JsonConverter(typeof(NewTypeJsonConverter<int, WrappedInt>))]
public sealed record WrappedInt : NewType<int, WrappedInt> {
    private WrappedInt(int value) : base(value) { }
}
```

Finally, a factory to register the custom converter via `JsonSerializerSettings` can be used.
That way all new types will be serialized and deserialized using the `NewTypeJsonConverter`.

```csharp
var settings = new JsonSerializerSettings();
settings.Converters.Add(new NewTypeJsonConverter());
```

## Examples

### Serialization

```csharp
var wrappedInt = WrappedInt.From(42);
var json = JsonConvert.SerializeObject(wrappedInt);
Console.WriteLine(json); // Output: 42
```

### Deserialization

```csharp
var json = "42";
var wrappedInt = JsonConvert.DeserializeObject<WrappedInt>(json);
Console.WriteLine(wrappedInt.GetValue()); // Output: 42
```

## License

This project is licensed under the MIT License. See the `LICENSE` file for more details.