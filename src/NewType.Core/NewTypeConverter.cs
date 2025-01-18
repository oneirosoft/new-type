using System.ComponentModel;
using System.Globalization;

namespace Oneiro;

/// <summary>
/// Type converter for NewType
/// </summary>
/// <typeparam name="T">
/// The type of the value being wrapped.
/// </typeparam>
/// <typeparam name="TNewType">
///  The type of the new type.
/// </typeparam>
public class NewTypeConverter<T, TNewType> : TypeConverter 
    where TNewType : NewType<T, TNewType>
    where T : struct
{
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) =>
        destinationType != null &&
        (destinationType == typeof(T) || base.CanConvertTo(context, destinationType));
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(T) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) {
        if (value is TNewType newType) {
            return newType.GetValue();
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }

    public override object ConvertFrom(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object? value
    ) {
        try {
            return value switch {
                string stringValue when NewType<T, TNewType>.TryParse(stringValue, out var result) => result,
                T x => NewType<T, TNewType>.From(x),
                null => NewType<T, TNewType>.Default,
                _ => NewType<T, TNewType>.From((T)Convert.ChangeType(value, typeof(T)))
            };
        }
        catch {
            return NewType<T, TNewType>.Default;
        }
    }
}