using System.ComponentModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace Ornekler;

public class Ders03_04
{
    [Description("Belirtilen şehir için güncel (örnek) hava durumunu döndürür.")]
    private static string GetWeather(
        [Description("Şehir adı, örn: İstanbul")]
        string city)
        => $"{city} için hava: 22°C, parçalı bulutlu.";

    [Description("İki tam sayıyı toplar.")]
    private static int Add([Description("Sayı 1")] int a, [Description("Sayı 1")] int b) => a + b;


    public static async Task CalistirAsync()
    {
        AIAgent agent = NvidiaClient.CreateChatClient().AsAIAgent(
            instructions: """
                          Sen yardımsever bir asistansın.
                          Gerektiğinde araçları kullan.
                          Bir araç çağrısı için onay istendiğinde kullanıcı onay verirse
                          mutlaka aracı çalıştır ve sonucunu kullanıcıya bildir.
                          """,
            name: "AracliAgent",
            tools:
            [
                AIFunctionFactory.Create(GetWeather),

                new ApprovalRequiredAIFunction(AIFunctionFactory.Create(Add))
            ]);

        AgentSession session = await agent.CreateSessionAsync();

        Console.WriteLine("[Ders03] Araçlı agent çalışıyor...\n");
        while (true) 
        {
            Console.WriteLine("Pront:\n");
            var pront = Console.ReadLine();
            
            var message = new Microsoft.Extensions.AI.ChatMessage(
                ChatRole.User,
                [
                    new TextContent(pront)
                ]);
            await foreach (var item in agent.RunStreamingAsync(
                               message,
                               session))
            {
                Console.Write(item.Text);

                foreach (var content in item.Contents)
                {
                    if (content is ToolApprovalRequestContent approvalRequest)
                    {
                        var toolCall = (FunctionCallContent)approvalRequest.ToolCall;

                        Console.WriteLine(
                            $"\n\n⚠️ [ONAY GEREKLİ] " +
                            $"'{toolCall.Name}' fonksiyonu çalıştırılmak isteniyor.");

                        Console.WriteLine($"CallId: {toolCall.CallId}");
                        Console.Write("Bu işlemi onaylıyor musunuz? (E/H): ");

                        var input = Console.ReadLine();

                        bool isApproved =
                            input?.Trim().Equals(
                                "E",
                                StringComparison.OrdinalIgnoreCase) == true;

                        Console.WriteLine(
                            isApproved
                                ? "\n✅ Onay verildi, işlem yürütülüyor...\n"
                                : "\n❌ İşlem reddedildi.\n");
                        var appcontent = new ToolApprovalResponseContent(approvalRequest.RequestId, isApproved,toolCall);

                        // ÖNEMLİ KISIM
                        var approvalMessage = new Microsoft.Extensions.AI.ChatMessage(
                            ChatRole.User,
                            [
                                appcontent,
                                new TextContent(pront),
                            ]);

                        // Aynı session ile devam et
                        await foreach (var result in agent.RunStreamingAsync(
                                           approvalMessage,
                                           session))
                        {
                            Console.Write(result.Text);
                        }
                        
                    }
                       
                    
                    
                }
                Console.Write(item.Text);
            }
        }
    }
}