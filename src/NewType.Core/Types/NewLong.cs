namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="long"/>.
/// </summary>
public sealed
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewLong : NewType<long, NewLong>
{
    private NewLong(long value) : base(value) { }

    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewLong From(long value) => new(value);
}