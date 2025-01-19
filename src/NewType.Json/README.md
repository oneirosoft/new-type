# NewType Json

`Oneiro.NewType.Json` is a library that extends the core functionality of `Oneiro.NewType.Core` by providing support for JSON serialization and deserialization using the `System.Text.Json` library. This allows you to easily convert your new types to and from JSON.

## Features

- JSON serialization and deserialization support for new types.
- Integration with `System.Text.Json`.

## Installation

To install `NewType.Json`, add the following package to your project:

```sh
dotnet add package Oneiro.NewType.Json
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

You can also use the `JsonConverterAttribute` that comes with `System.Text.Json` to specify a custom JSON converter for your new type.

```csharp
[JsonConverter(typeof(NewTypeJsonConverter<int, WrappedInt>))]
public sealed record WrappedInt : NewType<int, WrappedInt> {
    private WrappedInt(int value) : base(value) { }
}
```

Finally, a factory to register the custom converter via `JsonSerializerOptions` can be used.
That way all new types will be serialized and deserialized using the `NewTypeJsonConverter`.

```csharp
var options = new JsonSerializerOptions();
options.Converters.Add(new NewTypeJsonConverter());
```
## Examples

### Serialization

```csharp
var wrappedInt = WrappedInt.From(42);
var json = JsonSerializer.Serialize(wrappedInt);
Console.WriteLine(json); // Output: 42
```

### Deserialization

```csharp
var json = "42";
var wrappedInt = JsonSerializer.Deserialize<WrappedInt>(json);
Console.WriteLine(wrappedInt.GetValue()); // Output: 42
```

## License

This project is licensed under the MIT License. See the `LICENSE` file for more details.