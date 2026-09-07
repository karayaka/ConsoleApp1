# 00 — Kurulum ve Ortam

## Amaç
Projeyi çalışır hale getirmek: paketleri kurmak, model anahtarını tanımlamak ve
ilk agent'ı çalıştırmak.

## 1. Paketler
Bu projeye zaten eklendi (`ConsoleApp1.csproj`):

```xml
<PackageReference Include="Microsoft.Agents.AI" Version="1.19.0" />
<PackageReference Include="Microsoft.Agents.AI.OpenAI" Version="1.19.0" />
```

- **Microsoft.Agents.AI** → çekirdek: `AIAgent`, `AgentSession`, orkestrasyon.
- **Microsoft.Agents.AI.OpenAI** → OpenAI / Azure OpenAI / OpenAI-uyumlu istemciler.

Kendi projene sıfırdan eklemek isteyen için:
```bash
dotnet add package Microsoft.Agents.AI
dotnet add package Microsoft.Agents.AI.OpenAI
```

## 2. Sağlayıcı: NVIDIA ücretsiz API
Bu eğitimde **NVIDIA'nın ücretsiz, OpenAI-uyumlu API'sini** kullanıyoruz.
Tek fark, istemcinin `base_url`'ünü NVIDIA'ya yöneltmek:

```
base_url = https://integrate.api.nvidia.com/v1
api_key  = nvapi-...
```

Bu ayar tek bir yerde toplandı: **`ConsoleApp1/Ornekler/NvidiaClient.cs`**.
Anahtar kolaylık olsun diye orada gömülü; istersen ortam değişkeniyle ez:

```bash
export NVIDIA_API_KEY="nvapi-..."
export NVIDIA_MODEL="meta/llama-3.3-70b-instruct"   # varsayılan
```

> **Model seçimi önemli:** Tool-calling (03. ders) için güçlü bir model gerekir.
> Küçük `meta/llama-3.1-8b-instruct` tool çağrısını bazen metin olarak *sızdırır*;
> **`meta/llama-3.3-70b-instruct`** temiz çalışır — bu yüzden varsayılan odur.

> Not: `Microsoft.Agents.AI` kodu değişmez. Yalnızca alttaki istemci NVIDIA'ya
> bağlanır. Bu, 01. dersteki "sağlayıcı bağımsızlığı" ilkesinin ta kendisi.

## 3. Çalıştırma
```bash
cd ConsoleApp1
dotnet run
```

`Program.cs` bir **başlatıcıdır (launcher)**: hangi dersin çalışacağını seçersin.
Varsayılan olarak 02. ders aktiftir; diğerleri yorum satırıdır — birini açıp
üsttekini kapatarak istediğin dersi çalıştırırsın.

Her dersin çalışan kodu **`ConsoleApp1/Ornekler/DersXX_*.cs`** dosyalarındadır
(static metotlar). Ders md'leri kavramı anlatır; örnek dosyalar onu çalıştırır.

## 4. Sık karşılaşılan hatalar
| Hata | Sebep | Çözüm |
|------|-------|-------|
| `401 Unauthorized` | Anahtar yanlış/limit doldu | `NVIDIA_API_KEY`'i kontrol et |
| Tool çağrısı metin olarak geliyor | Model zayıf | `NVIDIA_MODEL=meta/llama-3.3-70b-instruct` |
| `model_not_found` | Model adı yanlış | NVIDIA model kataloğundan doğru adı al |
| `model_not_found` | Model adı yanlış | `OPENAI_MODEL` değerini düzelt |

## Notlarım
> (Buraya anlatırken ekleyeceklerini yaz.)
