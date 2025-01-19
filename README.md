# New Types

[![NewType CI](https://github.com/oneirosoft/new-type/actions/workflows/new-type.yml/badge.svg)](https://github.com/oneirosoft/new-type/actions/workflows/new-type.yml)
![NuGet Version - Oneiro.NewType.Core](https://img.shields.io/nuget/v/Oneiro.NewType.Core?label=nuget%20-%20Oneiro.NewType.Core)
![NuGet Version - Oneiro.NewType.Json](https://img.shields.io/nuget/v/Oneiro.NewType.Json?label=nuget%20-%20Oneiro.NewType.Json)
![NuGet Version - Oneiro.NewType.Newtonsoft](https://img.shields.io/nuget/v/Oneiro.NewType.Newtonsoft?label=nuget%20-%20Oneiro.NewType.Newtonsoft)

The Oneiro project is a collection of libraries designed to provide strongly-typed wrappers around primitive types,
adding type safety and domain-specific logic to your code. The project is divided into three main components:

1. **NewType Core**
2. **NewType Json**
3. **NewType Newtonsoft**

## NewType Core

`Oneiro.NewType.Core` is the foundational library that provides the core functionality for creating and using new types.
It includes features such as custom validation rules, type conversion support, and integration with ASP.NET Core query parameters.

For detailed information on how to use `Oneiro.NewType.Core`, please refer to the [NewType Core README](src/NewType.Core/README.md).

## NewType Json

`Oneiro.NewType.Json` extends the core functionality by providing support for JSON serialization and deserialization
using the `System.Text.Json` library. This allows you to easily convert your new types to and from JSON.

For detailed information on how to use `Oneiro.NewType.Json`, please refer to the [NewType Json README](src/NewType.Json/README.md).

## NewType Newtonsoft

`Oneiro.NewType.Newtonsoft` provides similar JSON serialization and deserialization support as `Oneiro.NewType.Json`,
but using the popular `Newtonsoft.Json` library. This is useful if your project relies on `Newtonsoft.Json` for JSON handling.

For detailed information on how to use `Oneiro.NewType.Newtonsoft`, please refer to the [NewType Newtonsoft README](src/NewType.Newtonsoft/README.md).

## License

This project is licensed under the MIT License. See the `LICENSE` file for more details.
