namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="short"/>.
/// </summary>
public sealed
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewShort : NewType<short, NewShort> {
    private NewShort(short value) : base(value) { }

    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewShort From(short value) => new(value);
}