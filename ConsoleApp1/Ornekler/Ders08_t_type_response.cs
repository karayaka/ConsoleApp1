using System.ComponentModel;
using System.Text.Json;
using Microsoft.Agents.AI;
using OpenAI.Chat;

namespace Ornekler;

public class Ders08_t_type_response
{
    public static async Task CalistirAsync()
    {
        // 1) NVIDIA'ya bağlı chat client → AIAgent
        AIAgent agent = NvidiaClient
            .CreateChatClient()
            .AsAIAgent(
                instructions: """
                              You are a helpful assistant who gives concise and clear answers..
                              Find the world's most populous city.
                              
                              Enter the city name in the City Name field,
                              and the population as a whole number in the Population field.
                              For example, use 3,400,000 instead of 3400000.
                              """,
                name: "IlkAgent");

        agent = agent.AsBuilder()
            //.UseLogging()
            .Build();

        Console.WriteLine("[Ders08] Agent düşünüyor...\n");

        var response = await agent.RunAsync<city>(
            "What is the most populous city in the world?");

        Console.WriteLine("Şehir:");
        Console.WriteLine(response.Result);
        Console.WriteLine(JsonSerializer.Serialize(response.Result));
    }
    public class  city
    {
        [Description("City Name")]
        public string Name { get; set; } = string.Empty;
        
        [Description("Population as an integer number")]
        public int Population { get; set; }

        public override string ToString()
        {
            return $"Adı: {Name}, Nüfusu: {Population}";
        }
    }
}