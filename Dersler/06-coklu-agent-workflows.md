# 06 — Çoklu Agent & Workflows

Tek bir agent çoğu işi görür. Ama karmaşık problemleri **uzmanlaşmış birden
çok agent'a** bölmek daha temiz ve daha güvenilirdir. Örnek: bir "Araştırmacı"
agent bilgi toplar, bir "Editör" agent onu düzenler.

## Yaklaşım A — Elle orkestrasyon (kurulu paketlerle çalışır)

En basit haliyle: agent'ları sırayla kendi kodunla bağlarsın. Bir agent'ın
çıktısı diğerinin girdisi olur.

```csharp
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;

string apiKey  = Environment.GetEnvironmentVariable("OPENAI_API_KEY")!;
string modelId = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

var client = new OpenAIClient(new System.ClientModel.ApiKeyCredential(apiKey))
    .GetChatClient(modelId);

AIAgent arastirmaci = client.AsAIAgent(
    instructions: "Konu hakkında maddeler halinde ham bilgi/notlar üret.",
    name: "Arastirmaci");

AIAgent editor = client.AsAIAgent(
    instructions: "Verilen ham notları akıcı, tek paragraflık bir metne dönüştür.",
    name: "Editor");

// 1) Araştırmacı notları üretir
var notlar = await arastirmaci.RunAsync("Konu: Elektrikli araçların avantajları");
Console.WriteLine("--- HAM NOTLAR ---\n" + notlar.Text);

// 2) Editör notları temize çeker (bir agent'ın çıktısı → diğerinin girdisi)
var makale = await editor.RunAsync(notlar.Text);
Console.WriteLine("\n--- SON METİN ---\n" + makale.Text);
```

Bu desen **sıralı (sequential)** orkestrasyondur. Aynı mantıkla:
- **Paralel**: birkaç agent'ı `Task.WhenAll` ile aynı anda çalıştır.
- **Yönlendirme (routing)**: bir "yönlendirici" agent, işi hangi uzmana
  vereceğine karar versin.
- **Agent'ı tool yap**: bir agent'ı, başka bir agent'ın `tools` listesine
  fonksiyon olarak koyabilirsin (03. dersle birleştir).

## Yaklaşım B — Workflows paketi (graf tabanlı, ileri seviye)

Karmaşık senaryolar (dallanma, döngü, checkpoint, human-in-the-loop) için
Microsoft ayrı bir paket sunar:

```bash
dotnet add package Microsoft.Agents.AI.Workflows
```

Bu paket, agent'ları ve fonksiyonları **executor** (işlem birimi) ve **edge**
(veri akış yolu) olarak bir **graf** halinde bağlamanı sağlar. Sağladıkları:
- Sıralı / eşzamanlı / grup sohbeti / devretme (handoff) desenleri
- Streaming, checkpoint (durumu kaydet/geri yükle)
- Human-in-the-loop (insan onayı adımları)
- Imperatif veya bildirimsel (deklaratif) tanım

> Bu paketin API'si daha geniştir; ayrı bir ileri-seviye derste ele almak en iyisi.
> Başlamak için: <https://github.com/microsoft/agent-framework> → "Getting Started".

## Ne zaman hangisi?
| İhtiyaç | Seçim |
|---------|-------|
| 2-3 agent'ı sırayla/paralel bağlamak | Yaklaşım A (elle) |
| Dallanma, döngü, durum kaydı, onay adımları | Yaklaşım B (Workflows) |

## Denemeler
1. Araştırmacı → Editör hattına 3. bir "Çevirmen" agent ekle (İngilizceye çevirsin).
2. İki agent'ı `Task.WhenAll` ile paralel çalıştırıp sonuçları birleştir.

## Notlarım
> (Kendi çok-agent senaryoların, handoff örnekleri vb.)
