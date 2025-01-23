// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Oneiro;

BenchmarkRunner.Run<NewTypeBenchmarks>();

[MemoryDiagnoser]
public class NewTypeBenchmarks {
    
    private readonly ReadOnlyMemory<char> _value = "Hello, World!".AsMemory();
    
    [Benchmark]
    public NewString NewStringFromString() => NewString.From("Hello, World!");
    
    [Benchmark(Baseline = true)]
    public NewString NewStringFromReadOnlyMemory() => NewString.From(_value);
}