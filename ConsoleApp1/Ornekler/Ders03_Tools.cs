using System.ComponentModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace Ornekler;

/// <summary>
/// 03 — Tools (fonksiyon çağırma). Agent'a C# fonksiyonları verir; model
/// gerektiğinde bunları kendi çağırır.
/// İlgili ders: Dersler/03-tools.md
/// Not: NVIDIA'da tool-calling desteği modele göre değişir (llama-3.1 destekler).
/// </summary>
public static class Ders03_Tools
{
    [Description("Belirtilen şehir için güncel (örnek) hava durumunu döndürür.")]
    private static string GetWeather(
        [Description("Şehir adı, örn: İstanbul")] string city)
        => $"{city} için hava: 22°C, parçalı bulutlu.";

    [Description("İki tam sayıyı toplar.")]
    private static int Add([Description("Sayı 1")]int a,[Description("Sayı 2")] int b) => a + b;

    public static async Task CalistirAsync()
    {
        AIAgent agent = NvidiaClient.CreateChatClient().AsAIAgent(
            instructions: "Sen yardımsever bir asistansın. Gerektiğinde araçları kullan.",
            name: "AracliAgent",
            tools:
            [
                AIFunctionFactory.Create(GetWeather),
                AIFunctionFactory.Create(Add),
                //new HostedWebSearchTool() //internet aramalarına açma
                //new HostedFileSearchTool() sunucu içindeki dosyaları tarama
                //new HostedVectorStoreContent()
            ]);

        Console.WriteLine("[Ders03] Araçlı agent çalışıyor...\n");
        var response = await agent.RunAsync("Ankara'da hava nasıl, ve 17 + 25 kaç eder?");
        Console.WriteLine(response.Text);
    }
    
    
}
