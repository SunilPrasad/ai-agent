using Microsoft.Extensions.VectorData;

internal sealed class DocumentChunk
{
    [VectorStoreKey]
    public int Id { get; init; }

    [VectorStoreData]
    public required string Source { get; init; }

    [VectorStoreData]
    public required string Text { get; init; }

    [VectorStoreVector(1536, DistanceFunction = DistanceFunction.CosineSimilarity)]
    public ReadOnlyMemory<float> Embedding { get; set; }
}
