# NewType FluentValidation

Please reference [FluentValidation](https://github.com/FluentValidation/FluentValidation) for more information on how to use the library.

Oneiro.NewType.FluentValidation provides extensions to support validation of new types using FluentValidation.

## Installation

To install the package, use the following command:

```sh
dotnet add package Oneiro.NewType.FluentValidation
```

## Usage

### Creating a New Type

First, create a new type by inheriting from `NewType<T, TNewType>` or use one
of the many built in new types:

### Creating a Validator

Next, create a validator for your new type using FluentValidation. 

```csharp
var validator = Validations.CreateValidatorFor<int, NewInt>(
    builder => builder.GreaterThan(0)
);
```

### Using the Validator

You can now use the validator to validate instances of your new type:

```csharp
 var newInt = NewInt.From(100);
var validator = Validations.CreateValidatorFor<int, NewInt>(
    builder => builder.GreaterThan(0)
);
ValidationResult result = validator.Validate(newInt);

if (result.IsValid)
    Console.WriteLine("Validation succeeded.");
else {
    Console.WriteLine("Validation failed:");
    foreach (var error in result.Errors)
        Console.WriteLine($"- {error.ErrorMessage}");
}
```

## `ValidatedNewType` and `IValidator`

Any `ValidatedNewType<T, TNewType>` rules defined can be used and converted into
an `IValidator<TNewType>` using the `Validations.CreateValidatorFor` method.

```csharp
sealed record ValidatedNewInt : ValidatedNewType<int, NewInt> {
    public ValidatedNewInt(int value) : base(value) {
        RuleFor(x => x).GreaterThan(0);
    }
}

var validator = Validations.CreateValidatorFor<int, ValidatedNewInt>();
```
## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
