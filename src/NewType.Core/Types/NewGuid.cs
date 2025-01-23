namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="Guid"/>.
/// </summary>
public
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewGuid : NewType<Guid, NewGuid> {
    
    protected NewGuid(Guid value) : base(value) {}
    
    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewGuid From(Guid value) => new(value);
}