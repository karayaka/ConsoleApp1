namespace Ornekler.Ders10_workflows;

public class UserInput(string rabbitmqNedir)
{
    public string Question { get; set; } = rabbitmqNedir;
}

public class ClassificationResult(string inputQuestion, string category)
{
    public string Question { get; set; }=inputQuestion;
    public string Category { get; set; }=category;
}