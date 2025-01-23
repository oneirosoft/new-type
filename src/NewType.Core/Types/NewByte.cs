namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="byte"/>.
/// </summary>
public sealed
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewByte : NewType<byte, NewByte> {
    private NewByte(byte value) : base(value) { }

    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewByte From(byte value) => new(value);
}