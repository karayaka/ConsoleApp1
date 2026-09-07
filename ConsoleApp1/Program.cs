// ============================================================================
//  Microsoft Agent Framework — Ders Örnekleri Başlatıcısı (Launcher)
//  Sağlayıcı: NVIDIA ücretsiz API (OpenAI-uyumlu) → Ornekler/NvidiaClient.cs
//  Her dersin çalışan örneği: ConsoleApp1/Ornekler/DersXX_*.cs
//
//  Çalıştırmak istediğin dersi AŞAĞIDA aktif bırak, diğerlerini yorumda tut.
//  Anahtarı ortam değişkeniyle değiştirmek istersen:
//    export NVIDIA_API_KEY="nvapi-..."
//    export NVIDIA_MODEL="meta/llama-3.1-8b-instruct"
// ============================================================================

using Microsoft.Agents.AI.Workflows;
using Ornekler;
using Ornekler.Ders10_workflows;

// ▶ ÇALIŞAN ÖRNEK (benim çağırdığım):
//await Ders02_IlkAgent.CalistirAsync();

// ▼ DİĞER ÖRNEKLER — denemek için birini aç (üsttekini kapatmayı unutma):
//await Ders03_Tools.CalistirAsync();          // 03 — Tools / fonksiyon çağırma
//await Ders03_04.CalistirAsync();          // 03 — Tools / fonksiyon çağırma
//await Ders04_OturumHafiza.CalistirAsync();   // 04 — Oturum & hafıza (sessions)
//await Ders05_Streaming.CalistirAsync();       // 05 — Streaming (token token akış)
// await Ders06_CokluAgent.CalistirAsync();      // 06 — Çoklu agent orkestrasyonu
 //await Ders06_Skills.CalistirAsync();  /// skillerin kullanımı
 //await Ders07_MCP.CalistirAsync();
 //await Ders08_t_type_response.CalistirAsync();
 //await Ders09_RAG.CreateRagDb();
 await Ders09_RAG.CalistirAsync();
 
 //await Workflows.CalistirAsync();