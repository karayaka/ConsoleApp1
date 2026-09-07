using Microsoft.Agents.AI;
using OpenAI.Chat;

namespace Ornekler;

/// <summary>
/// 02 — İlk Agent. Bir chat client'ı AIAgent'a çevirip tek bir soru sorar.
/// İlgili ders: Dersler/02-ilk-agent.md
/// </summary>
public static class Ders02_IlkAgent
{
    public static async Task CalistirAsync()
    {
        // 1) NVIDIA'ya bağlı chat client → AIAgent
        AIAgent agent = NvidiaClient.CreateChatClient().AsAIAgent(
            instructions: "Sen yardımsever, kısa ve net cevap veren bir asistansın. Türkçe konuş.",
            name: "IlkAgent");
        agent.AsBuilder().UseLogging();
        // 2) Çalıştır
        Console.WriteLine("[Ders02] Agent düşünüyor...\n");
        var response = await agent.RunAsync(
            "Microsoft Agent Framework nedir? Üç maddede özetle.");

        Console.WriteLine(response.Text);
    }
}
