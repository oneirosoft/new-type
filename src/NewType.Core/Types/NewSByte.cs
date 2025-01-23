namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="sbyte"/>.
/// </summary>
public
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewSByte : NewType<sbyte, NewSByte>
{
    protected NewSByte(sbyte value) : base(value) { }

    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewSByte From(sbyte value) => new(value);
}