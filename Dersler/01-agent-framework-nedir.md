# 01 — Microsoft Agent Framework Nedir?

## Bir cümlede
Farklı LLM sağlayıcıları için **tek ve tutarlı bir API** ile agent'lar
(araç kullanabilen, hafızası olan, birbirleriyle konuşabilen yapay zekâ
bileşenleri) oluşturmayı sağlayan bir .NET kütüphanesi.

## "Chatbot" ile "Agent" farkı
- **Chatbot:** Sen sorarsın, model cevaplar. Tek atış.
- **Agent:** Bir hedef verirsin; model **karar verir**, gerektiğinde
  **araç (tool) çağırır**, sonucu görür, tekrar düşünür ve döngüyü kendi
  bitirir. Yani "düşün → eylem → gözlem → tekrar" döngüsü.

```
Kullanıcı ── hedef ──► [ AGENT ]
                         │  ▲
                 tool çağır │  │ tool sonucu
                         ▼  │
                     [ Dış dünya: API, DB, hesaplama ]
```

## Neden Microsoft Agent Framework?
Microsoft'un iki eski projesi vardı: **Semantic Kernel** ve **AutoGen**.
Agent Framework bunların **birleşimi ve halefi**dir. Öne çıkanlar:

1. **Sağlayıcı bağımsızlığı** — OpenAI, Azure OpenAI, Anthropic, yerel Ollama…
   Kod (`RunAsync`) hepsinde **aynı** kalır. Sadece istemciyi değiştirirsin.
2. **`Microsoft.Extensions.AI` üzerine kurulu** — .NET'in standart AI
   soyutlamalarıyla uyumlu (DI, logging, telemetry hazır gelir).
3. **Basitten karmaşığa** — tek satırlık "hello world" agent'tan,
   graf tabanlı çok-agent'lı **workflow**'lara kadar aynı çatı.
4. **Cross-platform** — .NET 8, .NET Standard 2.0, .NET Framework.

## Temel kavramlar (bu eğitimde göreceğimiz)
| Kavram | Tip | Ne işe yarar |
|--------|-----|--------------|
| Agent | `AIAgent` | Ana soyutlama. Bir görev/karakteri temsil eder. |
| Instructions | `string` | Agent'ın system prompt'u; karakteri ve görevi. |
| Tool | `AIFunction` | Agent'ın çağırabileceği C# fonksiyonu. |
| Oturum | `AgentSession` | Çok turlu konuşmanın hafızası. |
| Yanıt | `AgentRunResponse` | `RunAsync` sonucu; `.Text` ile metni alırsın. |
| Workflow | — | Birden çok agent'ı bağlayan orkestrasyon. |

## Zihinsel model
> Bir `AIAgent`, alttaki bir **chat client**'ın (ör. OpenAI `ChatClient`)
> "karakter + araçlar + hafıza yönetimi" ile giydirilmiş halidir.
> Sen sadece `RunAsync` çağırırsın; alttaki tool-calling döngüsünü framework yürütür.

## Notlarım
> (Semantic Kernel/AutoGen geçmişi, kendi kullanım senaryoların vb.)
