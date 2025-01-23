namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="double"/>.
/// </summary>
public sealed
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewDouble : NewType<double, NewDouble> {
    private NewDouble(double value) : base(value) { }

    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewDouble From(double value) => new(value);
}