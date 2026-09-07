# 02 — İlk Agent

> ▶ **Çalışan örnek:** `ConsoleApp1/Ornekler/Ders02_IlkAgent.cs`
> (Program.cs'te varsayılan olarak bu çağrılır. `dotnet run` ile dene.)

Bu ders, bir chat client'ı `AIAgent`'a çevirip tek bir soru sormanın açıklamasıdır.
Aşağıdaki kod **kavramı** gösterir; çalışan hali NVIDIA sağlayıcısıyla yukarıdaki dosyadadır.

## Kod (satır satır)

```csharp
using System.ClientModel;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;   // AsAIAgent uzantısı burada!

// 1) Chat client: alttaki modele giden bağlantı.
string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")!;
string modelId = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

ChatClient chatClient = new OpenAIClient(new ApiKeyCredential(apiKey))
    .GetChatClient(modelId);

// 2) Chat client'ı bir AIAgent'a dönüştür.
AIAgent agent = chatClient.AsAIAgent(
    instructions: "Sen yardımsever, kısa ve net cevap veren bir asistansın. Türkçe konuş.",
    name: "IlkAgent");

// 3) Çalıştır.
AgentRunResponse response = await agent.RunAsync(
    "Microsoft Agent Framework nedir? Üç maddede özetle.");

Console.WriteLine(response.Text);
```

## Kritik noktalar
- **`ApiKeyCredential`** → `System.ClientModel` içinde (OpenAI SDK v2 standardı).
- **`.GetChatClient(modelId)`** → OpenAI SDK'nın `ChatClient`'ını verir.
- **`.AsAIAgent(...)`** → uzantı metodu, **`OpenAI.Chat`** namespace'inde.
  `using OpenAI.Chat;` yoksa "AsAIAgent bulunamadı" hatası alırsın.
- **`instructions`** = system prompt. Agent'ın kişiliğini/kurallarını burada belirle.
- **`RunAsync`** senkron bekler ve tam yanıtı döner. Akış için 05. derse bak.
- **`response.Text`** = düz metin cevap. (Yanıtta ayrıca kullanılan mesajlar,
  token sayısı gibi meta veriler de bulunur.)

## Azure OpenAI kullanıyorsan
Sadece 1. adımı değiştir; gerisi **aynı kalır**:
```csharp
using Azure.AI.OpenAI;              // dotnet add package Azure.AI.OpenAI
using Azure.Identity;

ChatClient chatClient = new AzureOpenAIClient(
        new Uri(Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")!),
        new DefaultAzureCredential())
    .GetChatClient("gpt-4o-mini");   // Azure'daki deployment adı
```
> Buradaki güç şu: **agent kodu değişmiyor.** Sağlayıcı bağımsızlığı bu demek.

## Denemeler (öğrencilere ödev)
1. `instructions`'ı "korsan gibi konuş" yap, çıktının nasıl değiştiğini gör.
2. `RunAsync`'e başka bir soru ver.
3. `agent.Name` ve `agent.Id` değerlerini ekrana yazdır.

## Notlarım
> (Instructions yazma ipuçları, prompt kalıpları vb.)
