namespace Oneiro;

using System;

public
#if NET6_0_OR_GREATER
    record
#else
    class
#endif
    NewString : NewType<ReadOnlyMemory<char>, NewString> {
    protected NewString(ReadOnlyMemory<char> value) : base(value) { }
    
    public static NewString From(string value) => new(value.AsMemory());
    public new static NewString From(ReadOnlyMemory<char> value) => new(value);
    
    public ReadOnlyMemory<char> AsMemory() => _value;
    public ReadOnlySpan<char> AsSpan() => _value.Span;
}