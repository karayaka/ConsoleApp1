using System.ClientModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Embeddings;

namespace Ornekler;

/// <summary>
/// Tüm derslerin ortak istemci fabrikası.
/// NVIDIA'nın OpenAI-uyumlu ÜCRETSİZ API'sini kullanır:
///   base_url = https://integrate.api.nvidia.com/v1
/// Agent Framework kodu değişmez; yalnızca alttaki istemci NVIDIA'ya bağlanır.
/// (Bu, 01. dersteki "sağlayıcı bağımsızlığı" ilkesinin canlı örneğidir.)
/// </summary>
public static class NvidiaClient
{
    private const string BaseUrl = "https://integrate.api.nvidia.com/v1";

    // Anahtar önce ortam değişkeninden okunur; yoksa aşağıdaki değere düşer.
    // Üretimde anahtarı koda gömme — burada ders kolaylığı için duruyor.
    private const string DefaultApiKey =
        "";

    // NVIDIA'da ücretsiz, tool-calling destekli iyi bir varsayılan.
    private const string DefaultModel = "nvidia/nemotron-3.5-lightning-30b-a3b";
    

    /// <summary>NVIDIA endpoint'ine bağlı bir ChatClient üretir.</summary>
    public static ChatClient CreateChatClient(string? model = null)
    {
        var options = new OpenAIClientOptions { Endpoint = new Uri(BaseUrl) };
        return new OpenAIClient(new ApiKeyCredential(DefaultApiKey), options)
            .GetChatClient(DefaultModel);
    }
    
    public static EmbeddingClient GetEmbeddingClient(string? model = null)
    {
        var options = new OpenAIClientOptions { Endpoint = new Uri(BaseUrl) };
        return new OpenAIClient(new ApiKeyCredential(DefaultApiKey), options)
            .GetEmbeddingClient("nvidia/nemotron-3-embed-1b");
    }
    
    public static async Task<float[]> GenerateEmbeddingAsync(
        string text)
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", DefaultApiKey);

        var request = new
        {
            model = "nvidia/nemotron-3-embed-1b",
            input = text,
            encoding_format = "float"
        };

        var json = JsonSerializer.Serialize(request);

        using var response = await client.PostAsync(
            "https://integrate.api.nvidia.com/v1/embeddings",
            new StringContent(
                json,
                Encoding.UTF8,
                "application/json"));

        var responseText =
            await response.Content.ReadAsStringAsync();

        Console.WriteLine($"HTTP: {response.StatusCode}");

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(responseText);
            throw new Exception(
                $"Embedding request failed: {response.StatusCode}");
        }

        using var document =
            JsonDocument.Parse(responseText);

        var embedding =
            document
                .RootElement
                .GetProperty("data")[0]
                .GetProperty("embedding");

        return embedding
            .EnumerateArray()
            .Select(x => x.GetSingle())
            .ToArray();
    }
}
