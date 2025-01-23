namespace Oneiro;

/// <summary>
/// A new type that wraps a value of type <see cref="float"/>.
/// </summary>
public sealed
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewFloat : NewType<float, NewFloat> {
    private NewFloat(float value) : base(value) { }

    /// <inheritdoc cref="NewType{T,TNewType}.From" />
    public new static NewFloat From(float value) => new(value);
}