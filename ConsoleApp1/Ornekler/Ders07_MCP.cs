using System.ComponentModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using OpenAI.Chat;

namespace Ornekler;

public class Ders07_MCP
{
    [Description("Belirtilen şehir için güncel (örnek) hava durumunu döndürür.")]
    private static string GetWeather(
        [Description("Şehir adı, örn: İstanbul")] string city)
        => $"{city} için hava: 22°C, parçalı bulutlu.";

    [Description("İki tam sayıyı toplar.")]
    private static int Add([Description("Sayı 1")]int a,[Description("Sayı 2")] int b) => a + b;

    public static async Task CalistirAsync()
    {
        await using McpClient mcpCliend = await McpClient.CreateAsync(new HttpClientTransport(new()
        {
            Endpoint = new Uri("https://learn.microsoft.com/api/mcp"),
            Name = "learn.microsoft.com",
            
        }));
        IList<McpClientTool> tools = await mcpCliend.ListToolsAsync();
        
        AIAgent agent = NvidiaClient.CreateChatClient().AsAIAgent(
            instructions: "Sen yardımsever bir asistansın. Gerektiğinde araçları kullan.",
            name: "AracliAgent",
            tools:
            [
                AIFunctionFactory.Create(GetWeather),
                AIFunctionFactory.Create(Add),
                ..tools.Cast<AITool>()
            ]);

        Console.WriteLine("[Ders03] Araçlı agent çalışıyor...\n");
        while (true)
        {
            Console.WriteLine("Pront:");
            var pront = Console.ReadLine();
            await foreach (var update in agent.RunStreamingAsync(pront))
            {
                Console.Write(update.Text);   // WriteLine değil: parçaları yan yana ekle
            }
        }
        
        
    }
}