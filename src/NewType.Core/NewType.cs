using System.Globalization;

namespace Oneiro;

/// <summary>
/// Represents a new type that wraps a value of type <typeparamref name="T"/>.
/// <code>
/// public sealed class WrappedInt : NewType&lt;int, WrappedInt> {
///     private WrappedInt(int value) : base(value) {}
/// }
/// </code>
/// </summary>
/// <typeparam name="T">The type of the value being wrapped.</typeparam>
/// <typeparam name="TNewType">The type of the new type.</typeparam>
/// <remarks>
/// When creating a new type, it is recommended to keep the
/// constructor <c>private</c> or <c>protected</c>.
/// </remarks>
public abstract 

#if NET6_0_OR_GREATER
    record 
#else
    class
#endif
    
NewType<T, TNewType> : IFormattable
    where TNewType : NewType<T, TNewType>
    where T : struct {
    
    protected readonly T _value;
    /// <summary>
    /// Determines if the wrapped value is the default value of the underlying type.
    /// </summary>
    public bool IsDefault { get; }
    /// <summary>
    /// Default value for the new type. Contains the default value of the underlying type.
    /// </summary>
    public static readonly TNewType Default = From(default);

    protected NewType(T value) {
        _value = value;
        IsDefault = value.Equals(default(T));
    }

    /// <summary>
    /// Gets the wrapped value.
    /// </summary>
    /// <returns>The wrapped value.</returns>
    public T GetValue() => _value;
    
    /// <summary>
    /// Factory method to create a new instance of the new type.
    /// The From method is preferred over the constructor.
    /// </summary>
    /// <param name="value">
    /// The value to wrap.
    /// </param>
    /// <returns>
    /// A new instance of the new type.
    /// </returns>
    public static TNewType From(T value) =>
        ActivatorUtil.CreateInstanceFactory<T, TNewType>()(value);


    public string ToString(string? format, IFormatProvider? formatProvider = null) =>
        _value switch {
            IFormattable value => value.ToString(format, formatProvider ?? CultureInfo.CurrentCulture),
            _ => ToString()
        };
    
    /// <inheritdoc />
    public override string ToString() =>
        _value.ToString() ?? "";
    
    /// <summary>
    /// Attempts to parse a string into a new type.
    /// </summary>
    /// <param name="input">
    /// The string to parse.
    /// </param>
    /// <param name="type">
    /// The new type if parsing was successful.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if parsing was successful; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryParse(string? input, out TNewType type) {
        if (input is null) {
            type = Default;
            return false;
        }

        try {
            var value = (T)Convert.ChangeType(input, typeof(T));
            type = From(value);
            return true;
        }
        catch {
            type = Default;
            return false;
        }
    }
    
    public static explicit operator T(NewType<T, TNewType> newType) => newType._value;
    
    /// <summary>
    /// Will return the wrapped value if the new type is not the default value;
    /// otherwise, it will return the other value.
    /// </summary>
    /// <param name="newType">
    /// The new type to evaluate and unwrap if not the default value.
    /// </param>
    /// <param name="value">
    /// The value to return if the new type is the default value.
    /// </param>
    /// <returns>
    /// The wrapped value if the new type is not the default value; otherwise, the other value.
    /// </returns>
    public static T operator |(NewType<T, TNewType> newType, T value) =>
        newType.IsDefault ? value : newType._value;
    
    /// <summary>
    /// Will return the wrapped value if the new type is not the default value;
    /// otherwise, it will return the other value.
    /// </summary>
    /// <param name="newType">
    /// The new type to evaluate.
    /// </param>
    /// <param name="other">
    /// The value to return if the new type is the default value.
    /// </param>
    /// <returns>
    /// The wrapped value if the new type is not the default value; otherwise, the other value.
    /// </returns>
    public static NewType<T, TNewType> operator |(NewType<T, TNewType> newType, TNewType other) =>
        newType.IsDefault ? other : newType;
    
#if !NET6_0_OR_GREATER
    public override bool Equals(object? obj) =>
        obj is NewType<T, TNewType> other && _value.Equals(other._value);
    
    public override int GetHashCode() => _value.GetHashCode();
    
    public static bool operator ==(NewType<T, TNewType> left, NewType<T, TNewType> right) =>
        left.Equals(right);
    
    public static bool operator !=(NewType<T, TNewType> left, NewType<T, TNewType> right) =>
        !left.Equals(right);    
#endif
}