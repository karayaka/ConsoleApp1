using Microsoft.Agents.AI.Workflows;
using OpenAI.Chat;

namespace Ornekler.Ders10_workflows;

public class Workflows
{
    public static async Task CalistirAsync()
    {
        var chatClient = NvidiaClient.CreateChatClient();

        var classifierAgent = chatClient.AsAIAgent(
            instructions: "Sen bir soru sınıflandırma asistanısın.",
            name: "ClassifierAgent");

        var technicalAnswerAgent = chatClient.AsAIAgent(
            instructions: "Sen yardımsever bir asistansın.",
            name: "TechnicalAnswerAgent");
        
        var generalAnswerAgent = chatClient.AsAIAgent(
            instructions: "Sen yardımsever bir asistansın.",
            name: "GeneralAnswerAgent");
        
        var classifier =
            new ClassifierExecutor(classifierAgent);
        
        var technicalAnswer =
            new TechnicalAnswerExecutor(technicalAnswerAgent);
        
        var generalAnswer =
            new GeneralAnswerExecutor(generalAnswerAgent);
        
        
        var workflow = new WorkflowBuilder(classifier)
            .AddEdge<ClassificationResult>(
                classifier,
                technicalAnswer,
                condition: result => result?.Category == "TECHNICAL")
            .AddEdge<ClassificationResult>(
                classifier,
                generalAnswer,
                condition: result =>
                    result?.Category == "GENERAL")
            .WithOutputFrom(technicalAnswer,generalAnswer)
            .Build();
        
        var run = await InProcessExecution.RunAsync(
            workflow,
            new UserInput("RabbitMQ nedir?"));

        Console.WriteLine($"Status: {await run.GetStatusAsync()}");
        Console.WriteLine($"Event count: {run.OutgoingEvents.Count()}");

        foreach (var evt in run.OutgoingEvents)
        {
            Console.WriteLine("================================");
            Console.WriteLine($"Event Type: {evt.GetType().Name}");
            Console.WriteLine(evt);
        }
    }
}