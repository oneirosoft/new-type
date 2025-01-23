namespace Oneiro;

using System;

public sealed
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewString : NewType<ReadOnlyMemory<char>, NewString> {
    private NewString(ReadOnlyMemory<char> value) : base(value) { }
    
    public static NewString From(string value) => new(value.AsMemory());
    
    public ReadOnlyMemory<char> AsMemory() => _value;
    public ReadOnlySpan<char> AsSpan() => _value.Span;
}