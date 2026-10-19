using NUnit.Framework;
using SOACA1.Services;
using System.Net;

[TestFixture]
public class SynonymServiceTests
{
    [Test]
    public async Task Returns_parsed_words()
    {
        var handler = new FakeHttpMessageHandler(_ =>
            FakeHttpMessageHandler.Json("""[{"word":"glad","score":99},{"word":"happy","score":90}]"""));
        var service = new SynonymService(new HttpClient(handler));

        var result = await service.GetSynonymsAsync("joyful");

        Assert.That(result, Is.EqualTo(new[] { "glad", "happy" }));
    }

    [Test]
    public async Task Url_encodes_the_word()
    {
        var handler = new FakeHttpMessageHandler(_ => FakeHttpMessageHandler.Json("[]"));
        var service = new SynonymService(new HttpClient(handler));

        await service.GetSynonymsAsync("big word");

        Assert.That(handler.LastRequest!.RequestUri!.Query, Does.Contain("rel_syn=big%20word"));
    }

    [Test]
    public async Task Returns_empty_on_non_success()
    {
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError));
        var service = new SynonymService(new HttpClient(handler));

        var result = await service.GetSynonymsAsync("test");

        Assert.That(result, Is.Empty);
    }
}