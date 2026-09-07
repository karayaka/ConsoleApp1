using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;

namespace Ornekler.Ders10_workflows;

public sealed class GeneralAnswerExecutor(AIAgent agent) : Executor("GeneralAnswerExecutor")
{
    protected override ProtocolBuilder ConfigureProtocol(
        ProtocolBuilder protocolBuilder)
    {
        return protocolBuilder
            .SendsMessage<string>()
            .ConfigureRoutes(routes =>
            {
                routes.AddHandler<ClassificationResult, string>(
                    HandleAsync);
            });
    }
    
    private async ValueTask<string> HandleAsync(
        ClassificationResult input,
        IWorkflowContext context)
    {
        Console.WriteLine(
            $"[Answer] Category = {input.Category}");
        Console.WriteLine(
            $"[Answer] GeneralAnswerExecutor çalıştı");

        var response = await agent.RunAsync(
            $"""
             Kullanıcının genel sorusuna kısa ve anlaşılır
             bir cevap ver.

             Kategori: {input.Category}

             Soru:
             {input.Question}
             """);

        return response.Text;
    }
}