namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="int"/>.
/// </summary>
public
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewInt : NewType<int, NewInt> {
    protected NewInt(int value) : base(value) { }

    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewInt From(int value) => new(value);
}