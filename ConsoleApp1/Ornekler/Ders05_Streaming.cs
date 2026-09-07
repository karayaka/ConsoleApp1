using Microsoft.Agents.AI;
using OpenAI.Chat;

namespace Ornekler;

/// <summary>
/// 05 — Streaming. Cevabı token token, üretildikçe ekrana basar.
/// İlgili ders: Dersler/05-streaming.md
/// </summary>
public static class Ders05_Streaming
{
    public static async Task CalistirAsync()
    {
        AIAgent agent = NvidiaClient.CreateChatClient().AsAIAgent(
            instructions: "Detaylı ve akıcı anlat.", name: "AkisAgent");

        Console.WriteLine("[Ders05] Akış başlıyor:\n");

        // RunStreamingAsync bir IAsyncEnumerable döndürür — parça parça gelir.
        await foreach (var update in agent.RunStreamingAsync(
                           "Yapay zekânın tarihini 2 paragrafta anlat."))
        {
            Console.Write(update.Text);   // WriteLine değil: parçaları yan yana ekle
        }

        Console.WriteLine();
    }
}
