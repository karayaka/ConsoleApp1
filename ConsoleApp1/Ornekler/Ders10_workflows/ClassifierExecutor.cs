using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;

namespace Ornekler.Ders10_workflows;

public sealed class ClassifierExecutor
    : Executor
{
    private readonly AIAgent _agent;

    public ClassifierExecutor(AIAgent agent)
        : base("ClassifierExecutor")
    {
        _agent = agent;
    }

    protected override ProtocolBuilder ConfigureProtocol(
        ProtocolBuilder protocolBuilder)
    {
        return protocolBuilder
            .SendsMessage<ClassificationResult>()
            .ConfigureRoutes(routes =>
            {
                routes.AddHandler<UserInput, ClassificationResult>(
                    HandleAsync);
            });
    }

    private async ValueTask<ClassificationResult> HandleAsync(
        UserInput input,
        IWorkflowContext context)
    {
        var response = await _agent.RunAsync(
            $"""
             Aşağıdaki soruyu sınıflandır.

             Sadece şu kategorilerden birini kullan:
             TECHNICAL
             GENERAL

             Soru:
             {input.Question}

             Sadece kategori adını döndür.
             """);

        var category = response.Text.Trim();

        Console.WriteLine(
            $"[Classifier] {category}");

        return new ClassificationResult(
            input.Question,
            category);
    }
}