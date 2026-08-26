# Basic RAG with Microsoft Agent Framework

This small console app shows the complete RAG path without hiding it behind extra application layers:

```text
sample text -> embedding vectors -> in-memory vector collection
user question -> query vector -> nearest chunks -> TextSearchProvider -> agent answer
```

The console prints all three important stages:

1. the size of each stored embedding;
2. the retrieval query, matched chunks, and similarity scores;
3. the final grounded answer from the agent.

## Requirements

- .NET 10 SDK
- An OpenAI API key

Set the key in PowerShell:

```powershell
$env:OPENAI_API_KEY = "your-key"
```

The sample uses `gpt-5.4-mini` for chat and `text-embedding-3-small` for 1,536-dimensional embeddings. The model names are written directly beside their client creation in `Program.cs`, so no additional environment variables are needed.

If you change to an embedding model that does not return 1,536 dimensions, update the `[VectorStoreVector(1536, ...)]` schema in `Program.cs`.

## Run

Run the built-in question:

```powershell
dotnet run
```

Or supply a question:

```powershell
dotnet run -- "When is bike setup support available?"
```

## Where the RAG work happens

- **Knowledge:** each `.txt` file is loaded into a `SourceDocument`. Its filename becomes the source name used in citations.
- **Ingestion:** this basic sample treats each source document as one chunk. It creates a `DocumentChunk` containing the source text and generated vector, then stores that chunk in `InMemoryVectorStore`.
- **Retrieval:** `SearchAsync` embeds the question and calls the collection's vector `SearchAsync` method.
- **Context injection:** `TextSearchProvider` converts the nearest chunks into context and adds it before the model call.
- **Generation:** the Agent Framework agent answers using that retrieved context.

`InMemoryVectorStore` is ideal for learning and prototypes because it needs no database, but all vectors disappear when the program exits. Use a persistent vector-store provider in a production application.

## Package versions

These were the current stable releases when this sample was created on 2026-08-26:

- `Microsoft.Agents.AI.OpenAI` 1.19.0
- `CommunityToolkit.VectorData.InMemory` 1.0.1

References:

- [Microsoft Agent Framework `TextSearchProvider` API](https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.textsearchprovider?view=agent-framework-dotnet-latest)
- [.NET vector-store guide](https://learn.microsoft.com/en-us/dotnet/ai/vector-stores/how-to/use-vector-stores)
- [`Microsoft.Agents.AI.OpenAI` on NuGet](https://www.nuget.org/packages/Microsoft.Agents.AI.OpenAI)
- [`CommunityToolkit.VectorData.InMemory` on NuGet](https://www.nuget.org/packages/CommunityToolkit.VectorData.InMemory)
