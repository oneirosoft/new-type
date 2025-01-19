# NewType Core

A new type for primitives.

`Oneiro.NewType.Core` is a library that provides a way to create strongly-typed wrappers around primitive types. This can be useful for adding type safety and domain-specific logic to your code.

## Features

- Strongly-typed wrappers around primitive types.
- Custom validation rules.
- Type conversion support.
- Use of `NewType` for aspnet core query parameters.

## Installation

To install `NewType.Core`, add the following package to your project:

```sh
dotnet add package Oneiro.NewType.Core
```

## Usage

### Creating a New Type

To create a new type, inherit from the `NewType<T, TNewType>` class, where `T`
is the underlying type and `TNewType` is the new type you are creating.

_It is recommended to make your new type class `sealed` to prevent inheritance._

```csharp
public sealed record WrappedInt : NewType<int, WrappedInt> {
    private WrappedInt(int value) : base(value) { }
}
```

_If using the dotnet standard version, you can use the `NewType` class instead of the `record` keyword._

```csharp
public sealed class WrappedInt : NewType<int, WrappedInt> {
    private WrappedInt(int value) : base(value) { }
}
```

### Using the New Type

You can create instances of your new type using the `From` method:

```csharp
var wrappedInt = WrappedInt.From(42);
```

You can also use the `Default` property to get the default value of your new type:

```csharp
var defaultWrappedInt = WrappedInt.Default;
```

For aspnet core, you can use the `NewType` in query parameters
for both controller based and minimal APIs.

```csharp
[HttpGet]
public IActionResult Get([FromQuery] WrappedInt wrappedInt) {
    return Ok(wrappedInt.GetValue());
}
```

_The functionality is provided by the use of `TryParse` and `TypeConverter`._

### Validation

You can add custom validation rules to your new type by inheriting from `ValidatedNewType<T, TNewType>`:

```csharp
public sealed class ValidatedInt : ValidatedNewType<int, ValidatedInt> {
    public ValidatedInt(int value) : base(value) {
        RuleFor("Positive", x => x > 0, "Value must be positive");
    }
}
```

### Type Conversion

To enable type conversion, use the `NewTypeConverter` class:

```csharp
[TypeConverter(typeof(NewTypeConverter<int, WrappedInt>))]
public sealed class WrappedInt : NewType<int, WrappedInt> {
    public WrappedInt(int value) : base(value) { }
}
```

## Examples

### Creating and Using a New Type

```csharp
var wrappedInt = WrappedInt.From(42);
Console.WriteLine(wrappedInt.GetValue()); // Output: 42
```

### Validation

```csharp
var validatedInt = ValidatedInt.From(-1);
if (validatedInt.HasErrors()) {
    var errors = validatedInt.GetErrors();
    foreach (var error in errors) {
        Console.WriteLine($"{error.Name}: {error.ErrorMessage}");
    }
}
```
Optionally, you can throw errors caused during the type creation by calling `validatedInt.ThrowIfErrors()`.

```csharp
validatedInt.ThrowIfErrors();
```

### Type Conversion

Explicit casting is support for converting between the new type and the underlying type.

The optional use of the provided `TypeConverter` class allows for conversions.

```csharp
var converter = TypeDescriptor.GetConverter(typeof(WrappedInt));
var wrappedInt = (WrappedInt)converter.ConvertFrom(42);
var intValue = (int)converter.ConvertTo(wrappedInt, typeof(int));
```

## License

This project is licensed under the MIT License. See the `LICENSE` file for more details.