# 05 — Streaming (Akış)

`RunAsync` tüm cevap hazır olana kadar bekler. Uzun cevaplarda kullanıcı bekler.
**Streaming** ile cevabı **token token**, üretildikçe ekrana basarız — tıpkı
ChatGPT'nin yazar gibi görünmesi gibi.

## Örnek kod

```csharp
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;

string apiKey  = Environment.GetEnvironmentVariable("OPENAI_API_KEY")!;
string modelId = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

AIAgent agent = new OpenAIClient(new System.ClientModel.ApiKeyCredential(apiKey))
    .GetChatClient(modelId)
    .AsAIAgent(instructions: "Detaylı ve akıcı anlat.", name: "AkisAgent");

// RunStreamingAsync bir IAsyncEnumerable döndürür — parça parça gelir.
await foreach (var update in agent.RunStreamingAsync(
    "Yapay zekânın tarihini 2 paragrafta anlat."))
{
    Console.Write(update.Text);   // her parça geldikçe yaz, satır atlamadan
}
Console.WriteLine();
```

## Kritik noktalar
- **`RunStreamingAsync`** → `await foreach` ile tükettiğin bir akış döner.
- Her `update` bir **parça** (chunk). `update.Text` o parçanın metnidir;
  `Console.Write` (WriteLine değil) ile yan yana eklersin.
- Oturumla da çalışır: `RunStreamingAsync(input, session)`.
- Tool çağıran agent'larda akış, ara adımları da yansıtabilir; başlangıçta
  sadece metin akışına odaklan.

## Neden önemli?
- **Algılanan hız**: kullanıcı ilk kelimeleri hemen görür.
- **UX**: web/masaüstü arayüzlerde "yazıyor..." efekti.
- **İptal edilebilirlik**: uzun cevabı ortada durdurabilirsin (CancellationToken).

## İptal (bonus)
```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
await foreach (var update in agent.RunStreamingAsync("Uzun bir masal anlat.",
                                                     cancellationToken: cts.Token))
{
    Console.Write(update.Text);
}
```

## Denemeler
1. `RunAsync` ve `RunStreamingAsync` versiyonlarını yan yana çalıştır, farkı hisset.
2. Streaming'i 04. dersteki REPL'e entegre et.

## Notlarım
> (Web API'de streaming, SignalR/SSE ile arayüze bağlama vb.)
