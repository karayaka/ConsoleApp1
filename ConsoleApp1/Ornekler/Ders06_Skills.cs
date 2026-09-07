using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace Ornekler;

public class Ders06_Skills
{
    
    public static async Task CalistirAsync()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "skills");
        var skilProvider = new AgentSkillsProvider(skillPath:Path.Combine(AppContext.BaseDirectory,"skills"),null,null,new AgentSkillsProviderOptions(){
            DisableLoadSkillApproval = true,
            DisableReadSkillResourceApproval = true,
            DisableRunSkillScriptApproval = true
        });
        
        var skill = new AgentInlineSkill(
            new AgentSkillFrontmatter(
                "secret-skill",
                "Provides a secret phrase for testing"),
            """
            # Secret Skill

            The secret phrase is:

            SKILL_LOADED_12345

            If the user asks for the secret phrase,
            answer exactly:

            SKILL_LOADED_12345
            """);

        var skillProvider = new AgentSkillsProvider([skill],new AgentSkillsProviderOptions(){
            DisableLoadSkillApproval = true,
            DisableReadSkillResourceApproval = true,
            DisableRunSkillScriptApproval = true
        });

        var agent = NvidiaClient
            .CreateChatClient()
            .AsAIAgent(
                new ChatClientAgentOptions
                {
                    AIContextProviders =
                    [
                        skillProvider
                        //skilProvider
                    ]
                });

        var response = await agent.RunAsync(
            "Load the secret-skill and tell me the secret phrase.");

        Console.WriteLine(response.Text);
    }
}