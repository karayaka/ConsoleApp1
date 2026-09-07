# Microsoft Agent Framework — Eğitim Müfredatı

Bu klasör, bir grup yazılımcıya **Microsoft Agent Framework**'ü (.NET) sıfırdan
anlatmak için hazırlanmış ders ders bir eğitim setidir. Her ders bağımsız bir
markdown dosyasıdır; kod örnekleri kuruludaki sürüme (**1.19.0**) birebir uyumludur.

> Bu, senin dolduracağın/genişleteceğin bir iskelettir. Her dersin sonunda
> "Notlarım" bölümü var — anlatırken eklemek istediklerini oraya yazabilirsin.

## Kimler için?
- .NET/C# bilen ama LLM/agent dünyasına yeni olan yazılımcılar
- "Chatbot" ile "agent" arasındaki farkı merak edenler

## Ön koşullar
- .NET 8 SDK (`dotnet --version` → 8.x)
- Bir model sağlayıcısı anahtarı (OpenAI API key en kolayı) — bkz. [00 - Kurulum](00-kurulum.md)
- Temel C#: `async/await`, LINQ, attribute'lar

## Müfredat

| # | Ders | Konu |
|---|------|------|
| 00 | [Kurulum ve ortam](00-kurulum.md) | Paketler, API anahtarı, ilk çalıştırma |
| 01 | [Agent Framework nedir?](01-agent-framework-nedir.md) | Neden var, mimari, temel kavramlar |
| 02 | [İlk Agent](02-ilk-agent.md) | `AsAIAgent`, `RunAsync`, instructions |
| 03 | [Tools — Fonksiyon çağırma](03-tools.md) | Agent'a C# fonksiyonları verme |
| 04 | [Oturum & Hafıza (Sessions)](04-oturum-hafiza.md) | Çok turlu konuşma, `AgentSession` |
| 05 | [Streaming](05-streaming.md) | Token token akış, `RunStreamingAsync` |
| 06 | [Çoklu Agent & Workflows](06-coklu-agent-workflows.md) | Birden çok agent'ı orkestre etme |

## Nasıl ilerlemeli?
Her ders, kod parçalarını `ConsoleApp1/Program.cs` içine yapıştırıp
`dotnet run` ile deneyeceğin şekilde tasarlandı. "Çalıştır → gör → değiştir"
döngüsü en iyi öğretme yöntemidir.

## Kaynaklar
- Resmi blog: <https://devblogs.microsoft.com/dotnet/introducing-microsoft-agent-framework-preview/>
- GitHub: <https://github.com/microsoft/agent-framework>
- Site: <https://agentframework.net/>
