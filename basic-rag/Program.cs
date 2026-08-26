using CommunityToolkit.VectorData.InMemory;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using OpenAI.Chat;
using OpenAI.Embeddings;

// 2. Create the in-memory vector store.


InMemoryVectorStore vectorStore = new();
VectorStoreCollection<int, DocumentChunk> documentCollection =
    vectorStore.GetCollection<int, DocumentChunk>("bike-shop-knowledge");
await documentCollection.EnsureCollectionExistsAsync();


string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")!;

// 1. Load the plain-text files as source documents.
List<SourceDocument> sourceDocuments = [];

foreach (string file in Directory
    .GetFiles(Path.Combine(AppContext.BaseDirectory, "documents"), "*.txt")
    .OrderBy(file => file))
{
    sourceDocuments.Add(new SourceDocument
    {
        Id = sourceDocuments.Count + 1,
        Source = Path.GetFileNameWithoutExtension(file),
        Text = await File.ReadAllTextAsync(file)
    });
}

// This generator is used below for both document and question embeddings.
using var embeddingGenerator =
    new EmbeddingClient("text-embedding-3-small", apiKey).AsIEmbeddingGenerator();

// This basic sample treats each small source document as one chunk.
foreach (SourceDocument sourceDocument in sourceDocuments)
{
    DocumentChunk chunk = new()
    {
        Id = sourceDocument.Id,
        Source = sourceDocument.Source,
        Text = sourceDocument.Text,
        Embedding = await embeddingGenerator.GenerateVectorAsync(sourceDocument.Text)
    };

    await documentCollection.UpsertAsync(chunk);

    Console.WriteLine(
        $"  Stored '{chunk.Source}' as a {chunk.Embedding.Length}-number vector.");
}

// 3. Tell TextSearchProvider how to retrieve relevant documents.
TextSearchProvider textSearchProvider = new(
    SearchDocumentsAsync,
    new TextSearchProviderOptions
    {
        SearchTime = TextSearchProviderOptions.TextSearchBehavior.BeforeAIInvoke
    });

// 4. Create the agent and attach the text search provider.
AIAgent agent = new ChatClient("gpt-5.4-mini", apiKey).AsAIAgent(new ChatClientAgentOptions
{
    ChatOptions = new ChatOptions
    {
        Instructions = """
            You answer questions about the bike shop using only the retrieved context.
            If the context does not contain the answer, say that you do not know.
            Cite the source name in square brackets.
            """
    },
    AIContextProviders = [textSearchProvider]
});

// 5. Ask a question. TextSearchProvider retrieves context before the agent responds.
string question = args.Length > 0
    ? string.Join(' ', args)
    : "How long do I have to return a bike, and what do I need?";

Console.WriteLine($"\nQUESTION\n  {question}");

AgentResponse answer = await agent.RunAsync(question);

Console.WriteLine($"\nAGENT ANSWER\n  {answer.Text}");

// TextSearchProvider calls this function with the user's question.
async Task<IEnumerable<TextSearchProvider.TextSearchResult>> SearchDocumentsAsync(
    string query,
    CancellationToken cancellationToken)
{
    ReadOnlyMemory<float> queryEmbedding = await embeddingGenerator.GenerateVectorAsync(
        query,
        cancellationToken: cancellationToken);

    Console.WriteLine($"\nRETRIEVAL\n  Query embedding size: {queryEmbedding.Length}");

    List<TextSearchProvider.TextSearchResult> searchResults = [];

    List<VectorSearchResult<DocumentChunk>> matches = await documentCollection
        .SearchAsync(
            queryEmbedding,
            top: 2,
            options: null,
            cancellationToken: cancellationToken)
        .ToListAsync(cancellationToken);

    foreach (VectorSearchResult<DocumentChunk> match in matches)
    {
        Console.WriteLine($"  Match: {match.Record.Source} (score: {match.Score:F4})");

        searchResults.Add(new TextSearchProvider.TextSearchResult
        {
            SourceName = match.Record.Source,
            Text = match.Record.Text,
            RawRepresentation = match
        });
    }

    return searchResults;
}
