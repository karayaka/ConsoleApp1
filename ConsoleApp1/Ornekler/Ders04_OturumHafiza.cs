using System.Runtime.InteropServices;
using System.Text.Json;
using Microsoft.Agents.AI;
using OpenAI.Chat;

namespace Ornekler;

/// <summary>
/// 04 — Oturum & Hafıza. Aynı AgentSession verildiğinde agent önceki turları
/// hatırlar. Sonda küçük bir konsol REPL'i de var (yorumda).
/// İlgili ders: Dersler/04-oturum-hafiza.md
/// </summary>
public static class Ders04_OturumHafiza
{
    public static async Task CalistirAsync()
    {
        AIAgent agent = NvidiaClient.CreateChatClient().AsAIAgent(
            instructions: "Kısa ve samimi konuş.", name: "SohbetAgent");

        // Oturum = konuşmanın hafızası.
        AgentSession session = await agent.CreateSessionAsync();

        Console.WriteLine("[Ders04] 1. tur:");
        var a1 = await agent.RunAsync("Merhaba, benim adım Çağrı.", session);
        Console.WriteLine(a1.Text);

        Console.WriteLine("\n[Ders04] 2. tur (ismi hatırlamalı):");
        var a2 = await agent.RunAsync("Benim adım neydi?", session);
        Console.WriteLine(a2.Text);

        // --- İstersen canlı REPL'i aç: ---
         Console.WriteLine("\nSohbet ('cikis' ile çık):");
         while (true)
         {
             Console.Write("> ");
             string? input = Console.ReadLine();
             if (string.IsNullOrWhiteSpace(input) || input == "cikis") break;
             var resp = await agent.RunAsync(input, session);
             Console.WriteLine(resp.Text);
         }
    }
    public static async Task CalistirAsync2()
    {
        AIAgent agent = NvidiaClient.CreateChatClient().AsAIAgent(
            instructions: "Kısa ve samimi konuş.", name: "SohbetAgent");
        
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || input == "cikis") break;
            var resp = await agent.RunAsync(input);
            Console.WriteLine(resp.Text);
        }
    }
    public static async Task CalistirAsync3()
    {
        AIAgent agent = NvidiaClient.CreateChatClient().AsAIAgent(
            instructions: "Kısa ve samimi konuş.", name: "SohbetAgent");
        
        // Session oluştur = hafıza ile sohbet
        AgentSession session = await agent.CreateSessionAsync();
        //session datası jsona çevrilip yeniden yüklenebilir 
        Console.WriteLine("[Ders04-Session] Oturum başladı. 'cikis' ile çık.\n");
        
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input) || input == "cikis") break;
            
            var resp = await agent.RunAsync(input, session);
            Console.WriteLine(resp.Text);
            
            // Session yapısını JSON olarak yazdır
        }
        
        //json elemnt örneği
        var ss= await agent.SerializeSessionAsync(session);
        
        
        var sobject = await agent.DeserializeSessionAsync(ss);
        Console.WriteLine();
    }
    
    
}
