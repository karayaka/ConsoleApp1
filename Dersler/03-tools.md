# 03 — Tools (Fonksiyon Çağırma)

> ▶ **Çalışan örnek:** `ConsoleApp1/Ornekler/Ders03_Tools.cs`
> Program.cs'te ilgili satırı aç: `await Ders03_Tools.CalistirAsync();`
> ⚠️ Tool-calling için güçlü model gerekir: `NVIDIA_MODEL=meta/llama-3.3-70b-instruct` (varsayılan).

Agent'ı "chatbot"tan ayıran şey **araç kullanabilmesi**dir. Bir tool, agent'ın
ihtiyaç duyduğunda çağırdığı bir **C# fonksiyonu**dur.

## Nasıl çalışır?
1. Agent'a fonksiyonları tanıtırsın.
2. Kullanıcı bir şey ister ("İstanbul'da hava nasıl?").
3. Model, `GetWeather("İstanbul")` çağırması gerektiğine **kendi** karar verir.
4. Framework fonksiyonu çalıştırır, sonucu modele geri verir.
5. Model nihai cevabı doğal dille üretir.

Bu döngüyü sen yönetmezsin — `RunAsync` içinde otomatik döner.

## Örnek kod

```csharp
using System.ComponentModel;   // [Description] için
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;  // AIFunctionFactory için
using OpenAI;
using OpenAI.Chat;

string apiKey  = Environment.GetEnvironmentVariable("OPENAI_API_KEY")!;
string modelId = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

// --- Tool olacak fonksiyonlar (sıradan C# metotları) ---
[Description("Belirtilen şehir için güncel (örnek) hava durumunu döndürür.")]
static string GetWeather(
    [Description("Şehir adı, örn: İstanbul")] string city)
{
    // Gerçekte burada bir hava durumu API'si çağrılırdı.
    return $"{city} için hava: 22°C, parçalı bulutlu.";
}

[Description("İki sayıyı toplar.")]
static int Add(int a, int b) => a + b;

// --- Agent'ı tool'larla kur ---
AIAgent agent = new OpenAIClient(new System.ClientModel.ApiKeyCredential(apiKey))
    .GetChatClient(modelId)
    .AsAIAgent(
        instructions: "Sen yardımsever bir asistansın. Gerektiğinde araçları kullan.",
        name: "AracliAgent",
        tools:
        [
            AIFunctionFactory.Create(GetWeather),
            AIFunctionFactory.Create(Add),
        ]);

var r1 = await agent.RunAsync("Ankara'da hava nasıl, ve 17 + 25 kaç eder?");
Console.WriteLine(r1.Text);
```

## Kritik noktalar
- **`AIFunctionFactory.Create(metot)`** → C# metodunu bir `AIFunction`'a çevirir.
- **`[Description(...)]`** attribute'ları çok önemli! Model, fonksiyonu ve
  parametreleri **bu açıklamalara bakarak** ne zaman/ nasıl çağıracağına karar verir.
  Açıklama ne kadar net → model o kadar isabetli.
- Parametre tipleri basit olmalı (string, int, bool, enum, kayıt/record…).
  Framework JSON şemasını otomatik üretir.
- Tek `RunAsync` çağrısında model **birden çok** tool çağırabilir (yukarıda hava + toplama).

## Güvenlik notu
Tool'lar gerçek sistemlerine dokunur (DB, dosya, ödeme…). Modelin "yanlış"
çağırma ihtimaline karşı: tool içinde **doğrulama** yap, tehlikeli işlemleri
onaya bağla. Agent = otomatik güç; sınırlarını sen koyarsın.

## Denemeler
1. `GetTime()` adında, o anki saati döndüren bir tool ekle.
2. Fonksiyonun `[Description]`'ını sil ve modelin ne zaman şaşırdığını gözlemle.
3. Bir tool'a `throw new Exception(...)` koy; agent'ın hatayı nasıl ele aldığını gör.

## Notlarım
> (Kendi domain tool'ların, dış API entegrasyonu vb.)
