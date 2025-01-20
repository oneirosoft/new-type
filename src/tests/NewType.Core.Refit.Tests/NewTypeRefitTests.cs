using System.Net;

namespace Oneiro.Tests;

using Refit;

public sealed class NewTypeRefitTests {
    private readonly ITestApi _restClient;

    public NewTypeRefitTests() {
        var settings = new RefitSettings {
            HttpMessageHandlerFactory = () => new MockHandler()
        };
        _restClient = RestService
            .For<ITestApi>("http://newtype", settings);
    }
    
    [Test]
    public async Task Refit_Query_Should_Format_NewType_Value() {
        var result = await _restClient.GetFromQuery(WrappedInt.From(42));
        var actual = result.RequestMessage?.RequestUri?.ToString();
        const string expected = "http://newtype/test?value=42";
        await Assert.That(actual).IsEqualTo(expected);
    }
    
    [Test]
    public async Task Refit_Segment_Should_Format_NewType_Value() {
        var result = await _restClient.GeFromSegment(WrappedInt.From(42));
        var actual = result.RequestMessage?.RequestUri?.ToString();
        var expected = "http://newtype/test/42";
        await Assert.That(actual).IsEqualTo(expected);
    }
}

public sealed class MockHandler : DelegatingHandler {
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
            Content = new StringContent("hello"),
            RequestMessage = request
        });
    }
}

public sealed record WrappedInt : NewType<int, WrappedInt> {
    private WrappedInt(int value) : base(value) { }
}

public interface ITestApi {
    [Get("/test")]
    Task<IApiResponse> GetFromQuery([Query] WrappedInt value);
    [Get("/test/{value}")]
    Task<IApiResponse> GeFromSegment(WrappedInt value);
}