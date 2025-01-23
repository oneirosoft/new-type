namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="ulong"/>.
/// </summary>
public sealed
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewULong : NewType<ulong, NewULong> {
    private NewULong(ulong value) : base(value) { }

    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewULong From(ulong value) => new(value);
}