using System.Text.Json;
using CommunityToolkit.VectorData.SqliteVec;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using OpenAI.Chat;

namespace Ornekler;

public class Ders09_RAG
{
    public static async Task CalistirAsync()
    {
        var vectorStore = new SqliteVectorStore("Data Source=rag.db");

        var collection =
            vectorStore.GetCollection<int, Document>("documents");

        await collection.EnsureCollectionExistsAsync();
        // 1) NVIDIA'ya bağlı chat client → AIAgent

        //seach işlemi en yakın sonucu bulan seach işlemi 
        await SearchAsync(collection, "Kafka event streaming");

        // ragdaki bilgileri provider olarak agente gönderme!
        
        var ragProvider =
            new RagContextProvider(
                collection);
        var agent = NvidiaClient
            .CreateChatClient()
            .AsAIAgent(
                new ChatClientAgentOptions
                {
                    AIContextProviders =
                    [
                        ragProvider
                    ],
                });
        while (true)
        {
            Console.WriteLine("Pront:");
            var pront = Console.ReadLine();
            if (pront == "q") return;
            await foreach (var update in agent.RunStreamingAsync(pront))
            {
                Console.Write(update.Text);   // WriteLine değil: parçaları yan yana ekle
            }
        }
        
    }

    public static async Task CreateRagDb()
    {
        var vectorStore = new SqliteVectorStore("Data Source=rag.db");

        var collection =
            vectorStore.GetCollection<int, Document>("documents");

        await collection.EnsureCollectionExistsAsync();

        var documents = new[]
        {
            new Document
            {
                Id = 1,
                Title = "RabbitMQ",
                Content =
                    "RabbitMQ bir message broker sistemidir. " +
                    "Producer tarafından gönderilen mesajları alır " +
                    "ve consumer uygulamalarına iletir."
            },

            new Document
            {
                Id = 2,
                Title = "Redis",
                Content =
                    "Redis yüksek performanslı bir in-memory " +
                    "key-value veri deposudur. Cache, session ve " +
                    "geçici veri saklama amacıyla kullanılabilir."
            },

            new Document
            {
                Id = 3,
                Title = "Kafka",
                Content =
                    "Apache Kafka dağıtık event streaming platformudur. " +
                    "Yüksek miktarda event'in üretilmesi, saklanması " +
                    "ve tüketilmesi için kullanılır."
            }
        };

        IEmbeddingGenerator<string, Embedding<float>>
            embeddingGenerator = NvidiaClient
                .GetEmbeddingClient()
                .AsIEmbeddingGenerator();

        
        foreach (var document in documents)
        {
            var embedding =
                await embeddingGenerator.GenerateAsync(
                    document.Content);

            document.Embedding = embedding.Vector;

            await collection.UpsertAsync(document);
        }
        
    }

    private static async Task SearchAsync(
        VectorStoreCollection<int, Document> collection,
        string query)
    {
        IEmbeddingGenerator<string, Embedding<float>>
            embeddingGenerator = NvidiaClient
                .GetEmbeddingClient()
                .AsIEmbeddingGenerator();
        var queryVector =
            await embeddingGenerator.GenerateAsync(query);

        await foreach (var result in collection.SearchAsync(
                           queryVector,
                           top: 3))
        {
            Console.WriteLine(
                $"Score: {result.Score:F4}");

            Console.WriteLine(
                $"Başlık: {result.Record.Title}");

            Console.WriteLine(
                result.Record.Content);

            Console.WriteLine("-------------------");
        }
    }


    public sealed class RagContextProvider : AIContextProvider
    {
        private readonly VectorStoreCollection<int, Document> _collection;

        public RagContextProvider(
            VectorStoreCollection<int, Document> collection)
        {
            _collection = collection;
        }

        protected override async ValueTask<AIContext> ProvideAIContextAsync(
            AIContextProvider.InvokingContext context,
            CancellationToken cancellationToken = default)
        {
            // 1. Kullanıcının mesajını al
            var messages = context.AIContext.Messages;

            var userMessage = messages
                .LastOrDefault(x => x.Role == ChatRole.User);

            if (userMessage is null)
                return new AIContext();

            var query = userMessage.Text;

            Console.WriteLine(
                $"[RAG] Aranıyor: {query}");

            // 2. Query -> embedding
            IEmbeddingGenerator<string, Embedding<float>>
                embeddingGenerator = NvidiaClient
                    .GetEmbeddingClient()
                    .AsIEmbeddingGenerator();
            var queryVector =
                await embeddingGenerator.GenerateAsync(query);

            // 3. SQLiteVec semantic search
            var results = new List<string>();

            await foreach (var result in _collection.SearchAsync(
                               queryVector,
                               top: 3,
                               cancellationToken: cancellationToken))
            {
                Console.WriteLine(
                    $"[RAG] {result.Record.Title} " +
                    $"Distance: {result.Score:F4}");

                results.Add(
                    $"Başlık: {result.Record.Title}\n" +
                    $"İçerik: {result.Record.Content}");
            }

            if (results.Count == 0)
                return new AIContext();

            // 4. Bulunan bilgileri AI Context'e koy
            var ragContext =
                string.Join(
                    "\n\n---\n\n",
                    results);

            return new AIContext
            {
                Instructions =
                    """
                    Aşağıdaki bilgiler bilgi tabanından
                    semantic search ile getirildi.

                    Cevabını öncelikle bu bilgilere
                    dayanarak oluştur.

                    Eğer bilgi tabanında sorunun cevabı
                    yoksa bunu açıkça belirt.

                    BİLGİ TABANI:
                    """
                    + "\n\n"
                    + ragContext
            };
        }
    }

    //Tablo entitiysi
    public class Document
    {
        [VectorStoreKey] public int Id { get; set; }

        [VectorStoreData] public string Title { get; set; } = "";

        [VectorStoreData] public string Content { get; set; } = "";

        [VectorStoreVector(2048)] public ReadOnlyMemory<float> Embedding { get; set; }
    }
}