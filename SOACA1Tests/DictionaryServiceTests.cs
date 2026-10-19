using NUnit.Framework;
using SOACA1.Services;
using System.ComponentModel.Design;
using System.Net;

[TestFixture]
public class DictionaryServiceTests
{
    [Test]
    public async Task Returns_entry_on_success()
    {
        var handler = new FakeHttpMessageHandler(_ => FakeHttpMessageHandler.Json("""
            [{"word":"hello","phonetic":"həˈləʊ",
              "meanings":[{"partOfSpeach":"noun",
                           "definitions":[{"definition":"a greeting"}]}]}]
            """));
        var service = new DictionaryService(new HttpClient(handler));

        var entry = await service.GetEntryAsync("hello");

        Assert.That(entry, Is.Not.Null);
        Assert.That(entry!.Word, Is.EqualTo("hello"));
        Assert.That(entry.Meanings[0].PartOfSpeach, Is.EqualTo("noun"));
        Assert.That(entry.Meanings[0].Definitions[0].Definition, Is.EqualTo("a greeting"));
    }

    [Test]
    public async Task Returns_null_on_404()
    {
        var handler = new FakeHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.NotFound));
        var service = new DictionaryService(new HttpClient(handler));

        var entry = await service.GetEntryAsync("notaword");

        Assert.That(entry, Is.Null);
    }
}