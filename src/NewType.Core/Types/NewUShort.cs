namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="ushort"/>.
/// </summary>
public
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewUShort : NewType<ushort, NewUShort> {
    protected NewUShort(ushort value) : base(value) { }

    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewUShort From(ushort value) => new(value);
}