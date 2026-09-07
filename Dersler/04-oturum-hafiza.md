# 04 — Oturum & Hafıza (Sessions)

> ▶ **Çalışan örnek:** `ConsoleApp1/Ornekler/Ders04_OturumHafiza.cs`
> Program.cs'te ilgili satırı aç: `await Ders04_OturumHafiza.CalistirAsync();`


Varsayılan olarak her `RunAsync` çağrısı **birbirinden bağımsızdır** — agent
önceki mesajı hatırlamaz. Çok turlu bir konuşma (chat) için **oturum
(`AgentSession`)** kullanırız. Oturum, konuşma geçmişini taşır.

## Örnek: hatırlayan bir sohbet

```csharp
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;

string apiKey  = Environment.GetEnvironmentVariable("OPENAI_API_KEY")!;
string modelId = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

AIAgent agent = new OpenAIClient(new System.ClientModel.ApiKeyCredential(apiKey))
    .GetChatClient(modelId)
    .AsAIAgent(instructions: "Kısa ve samimi konuş.", name: "SohbetAgent");

// Bir oturum oluştur — bu, konuşmanın hafızasıdır.
AgentSession session = await agent.CreateSessionAsync();

// 1. tur
var a1 = await agent.RunAsync("Merhaba, benim adım Çağrı.", session);
Console.WriteLine(a1.Text);

// 2. tur — aynı session verildiği için ismi HATIRLAR
var a2 = await agent.RunAsync("Benim adım neydi?", session);
Console.WriteLine(a2.Text);   // "Adın Çağrı." benzeri
```

## Karşılaştır: oturumsuz
Aynı iki çağrıyı `session` vermeden yaparsan, 2. soruda agent ismini
**bilemez** — çünkü her çağrı sıfırdan başlar. Bu farkı sınıfta canlı göster.

## Basit bir REPL (konsol sohbeti)

```csharp
AgentSession session = await agent.CreateSessionAsync();
Console.WriteLine("Sohbet başladı ('cikis' ile çık).");
while (true)
{
    Console.Write("> ");
    string? input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input) || input == "cikis") break;

    var resp = await agent.RunAsync(input, session);
    Console.WriteLine(resp.Text);
}
```

## Kritik noktalar
- **`CreateSessionAsync()`** → yeni, boş bir oturum döndürür.
- Oturumu `RunAsync(message, session)` çağrısına vererek geçmişi sürdürürsün.
- Her kullanıcı / her konuşma için **ayrı** bir `AgentSession` tut.
- Oturum, konuşma **büyüdükçe token maliyetini** artırır (tüm geçmiş modele gider).
  Uzun konuşmalarda özetleme/kırpma stratejileri gerekir (ileri seviye).
- Oturumu kalıcı hale getirmek (DB'ye kaydet/yükle) için agent'ın
  `SerializeSessionAsync` / `DeserializeSessionAsync` metotları vardır.

## Denemeler
1. REPL'i çalıştır, agent'a birkaç bilgi ver, sonra "hepsini özetle" de.
2. İki farklı `session` oluştur; birinde söylediğinin diğerine geçmediğini gör.

## Notlarım
> (Kalıcı oturum, kullanıcı başına session yönetimi vb.)
