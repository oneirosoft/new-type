namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="uint"/>.
/// </summary>
public sealed
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewUInt : NewType<uint, NewUInt> {
    private NewUInt(uint value) : base(value) { }

    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewUInt From(uint value) => new(value);
}