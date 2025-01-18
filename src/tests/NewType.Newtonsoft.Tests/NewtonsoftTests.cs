using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Oneiro.Tests {
    public sealed class NewtonsoftTests {
        [JsonConverter(typeof(NewTypeJsonConverter))]
        internal sealed record WrappedInt : NewType<int, WrappedInt> {
            private WrappedInt(int value) : base(value) { }
        }

        [JsonConverter(typeof(NewTypeJsonConverter))]
        internal sealed record ValidatedLong : NewType<long, ValidatedLong> {
            private ValidatedLong(long value) : base(value) { }
        }

        [Test]
        public async Task Serialize() {
            var wrapped = WrappedInt.From(42);
            var json = JsonConvert.SerializeObject(wrapped);
            await Assert.That(json).IsEqualTo("42");
        }
        
        [Test]
        public async Task Serialize_Default_To_Wrapped_Default() {
            var wrapped = WrappedInt.Default;
            var json = JsonConvert.SerializeObject(wrapped);
            await Assert.That(json).IsEqualTo("0");
        }
        
        [Test]
        public async Task Deserialize() {
            var json = "42";
            var wrapped = JsonConvert.DeserializeObject<WrappedInt>(json);
            var expected = WrappedInt.From(42);
            await Assert.That(wrapped).IsEqualTo(expected);
        }
        
        [Test]
        public async Task Deserialize_Null_Results_In_Default() {
            var json = "null";
            var wrapped = JsonConvert.DeserializeObject<WrappedInt>(json);
            var expected = WrappedInt.Default;
            await Assert.That(wrapped).IsEqualTo(expected);
        }
        
        [Test]
        public void Deserialize_Invalid_Throws_JsonReaderException() {
            var json = "invalid";
            Assert.Throws<JsonReaderException>(() => JsonConvert.DeserializeObject<WrappedInt>(json));
        }
        
        [Test]
        public async Task Deserialize_Validated() {
            var json = "42";
            var wrapped = JsonConvert.DeserializeObject<ValidatedLong>(json);
            var expected = ValidatedLong.From(42);
            await Assert.That(wrapped).IsEqualTo(expected);
        }

        [Test]
        public async Task Serialized_Validated() {
            var wrapped = ValidatedLong.From(42);
            var json = JsonConvert.SerializeObject(wrapped);
            await Assert.That(json).IsEqualTo("42");
        }
    }
}
