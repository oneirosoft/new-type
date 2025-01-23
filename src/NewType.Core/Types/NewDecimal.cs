namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="decimal"/>.
/// </summary>
public
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewDecimal : NewType<decimal, NewDecimal> {
    protected NewDecimal(decimal value) : base(value) { }

    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewDecimal From(decimal value) => new(value);
}