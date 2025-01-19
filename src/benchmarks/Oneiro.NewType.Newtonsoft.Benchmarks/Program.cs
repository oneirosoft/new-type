// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Newtonsoft.Json;
using Oneiro;

BenchmarkRunner.Run<JsonConverterBenchmarks>();

[MemoryDiagnoser, SimpleJob]
public class JsonConverterBenchmarks {
    [JsonConverter(typeof(NewTypeJsonConverter))]
    public sealed record WrappedInt : NewType<int, WrappedInt> {
        private WrappedInt(int value) : base(value) {} 
    }
    
    [Benchmark]
    public string Serialize() {
        var wrapped = WrappedInt.From(42);
        return JsonConvert.SerializeObject(wrapped);
    }
    
    [Benchmark]
    public WrappedInt Deserialize() {
        var json = "42";
        return JsonConvert.DeserializeObject<WrappedInt>(json)!;
    }
}