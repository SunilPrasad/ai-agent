using System.ClientModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

// ============================================================
// SECTION 1: Configuration and model client
// ============================================================
// Secrets and the model name stay outside source code.
string apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY")
    ?? throw new InvalidOperationException("OPENROUTER_API_KEY is not set.");
string model = Environment.GetEnvironmentVariable("OPENROUTER_MODEL")
    ?? throw new InvalidOperationException("OPENROUTER_MODEL is not set.");

IChatClient chatClient = new OpenAIClient(
        new ApiKeyCredential(apiKey),
        new OpenAIClientOptions { Endpoint = new Uri("https://openrouter.ai/api/v1") })
    .GetChatClient(model)
    .AsIChatClient();

// ============================================================
// SECTION 2: Create one provider with dynamic context and memory
// ============================================================
// This variable represents live application state. A real application might
// read it from a database, API, feature flag, or authenticated user profile.
string supportStatus = "Travel support is open until 17:00 UTC.";

TravelContextProvider contextProvider = new(
    getSupportStatus: () => supportStatus);

// ChatClientAgentOptions registers the provider with the agent. The framework
// will call it before and after every run made by this agent.
AIAgent agent = chatClient.AsAIAgent(new ChatClientAgentOptions
{
    Name = "TravelAssistant",
    ChatOptions = new ChatOptions
    {
        Instructions =
            "You are a concise travel assistant. Use supplied application context when it is relevant. " +
            "Never claim that you remember a preference unless the context explicitly supplies it."
    },
    AIContextProviders = [contextProvider]
});

// ============================================================
// SECTION 3: Store one selected fact after a successful run
// ============================================================
// Conversation A has its own AgentSession and therefore its own chat history.
AgentSession conversationA = await agent.CreateSessionAsync();

Console.WriteLine("=== CONVERSATION A: SAVE ONE PREFERENCE ===");
AgentResponse saveResponse = await agent.RunAsync(
    "Remember preference: transport=train. Confirm briefly.",
    conversationA);
Console.WriteLine($"Agent: {saveResponse.Text}");

// StoreAIContextAsync has now copied only the explicit preference into the
// provider's session state. It did not store the whole conversation as memory.
TravelProfile savedProfile = contextProvider.GetProfile(conversationA);
Console.WriteLine($"Application memory: transport={savedProfile.PreferredTransport}");

// ============================================================
// SECTION 4: Dynamic context is collected again on the next run
// ============================================================
// Change the application state between turns. ProvideAIContextAsync reads the
// current value each time, so the next model request sees the new status.
supportStatus = "Travel support is closed; it reopens at 09:00 UTC.";

AgentResponse followUpResponse = await agent.RunAsync(
    "Recommend how I should travel and tell me whether support is available now.",
    conversationA);

Console.WriteLine("\n=== SAME CONVERSATION: HISTORY + CONTEXT + MEMORY ===");
Console.WriteLine($"Agent: {followUpResponse.Text}");

// ============================================================
// SECTION 5: Start a new conversation to separate history from memory
// ============================================================
// Conversation B starts with blank chat history and blank provider state.
AgentSession conversationB = await agent.CreateSessionAsync();

Console.WriteLine("\n=== CONVERSATION B: FIRST RUN WITHOUT A'S HISTORY OR MEMORY ===");
AgentResponse blankMemoryResponse = await agent.RunAsync(
    "What transport do I prefer? Answer only from supplied context.",
    conversationB);
Console.WriteLine($"Agent: {blankMemoryResponse.Text}");

// Conversation B now has its own first question and answer, but none of A's
// transcript. An application may deliberately load selected user memory.
contextProvider.SetProfile(conversationB, savedProfile);

Console.WriteLine("\n=== CONVERSATION B: OWN HISTORY, COPIED MEMORY VALUE ===");
AgentResponse recalledResponse = await agent.RunAsync(
    "What transport do I prefer? Answer only from supplied context.",
    conversationB);
Console.WriteLine($"Agent: {recalledResponse.Text}");

// ============================================================
// SECTION 6: The custom context provider
// ============================================================
// AIContextProvider belongs to Microsoft Agent Framework. This provider has a
// before phase for retrieval and an after phase for selective storage.
sealed class TravelContextProvider : AIContextProvider
{
    private const string PreferencePrefix = "Remember preference: transport=";
    private readonly Func<string> _getSupportStatus;
    private readonly ProviderSessionState<TravelProfile> _profiles;

    public TravelContextProvider(Func<string> getSupportStatus)
    {
        _getSupportStatus = getSupportStatus;

        // ProviderSessionState keeps this provider's state inside each
        // AgentSession. Its key also participates in session serialization.
        _profiles = new ProviderSessionState<TravelProfile>(
            _ => new TravelProfile(),
            stateKey: nameof(TravelContextProvider));
    }

    // Tell Agent Framework which session-state key belongs to this provider.
    public override IReadOnlyList<string> StateKeys => [_profiles.StateKey];

    public TravelProfile GetProfile(AgentSession session)
        => _profiles.GetOrInitializeState(session);

    public void SetProfile(AgentSession session, TravelProfile profile)
        // Copy the selected value instead of sharing the same profile object.
        => _profiles.SaveState(session, new TravelProfile
        {
            PreferredTransport = profile.PreferredTransport
        });

    // BEFORE the model call: retrieve fresh context and selected memory, then
    // return extra instructions that the framework merges into this request.
    protected override ValueTask<AIContext> ProvideAIContextAsync(
        InvokingContext context,
        CancellationToken cancellationToken = default)
    {
        TravelProfile profile = _profiles.GetOrInitializeState(context.Session);
        string currentSupportStatus = _getSupportStatus();
        string rememberedPreference = profile.PreferredTransport is null
            ? "No transport preference is stored for this conversation."
            : $"The user's remembered transport preference is {profile.PreferredTransport}.";

        Console.WriteLine($"[provider before] {currentSupportStatus} {rememberedPreference}");

        return new ValueTask<AIContext>(new AIContext
        {
            Instructions = $"Trusted application context:\n- {currentSupportStatus}\n- {rememberedPreference}"
        });
    }

    // AFTER a successful model call: inspect external request messages and save
    // only the explicit demo preference. The base class skips this method when
    // the agent invocation fails.
    protected override ValueTask StoreAIContextAsync(
        InvokedContext context,
        CancellationToken cancellationToken = default)
    {
        foreach (ChatMessage message in context.RequestMessages.Where(m => m.Role == ChatRole.User))
        {
            string text = message.Text?.Trim() ?? string.Empty;
            int prefixIndex = text.IndexOf(PreferencePrefix, StringComparison.OrdinalIgnoreCase);

            if (prefixIndex < 0)
            {
                continue;
            }

            string value = text[(prefixIndex + PreferencePrefix.Length)..]
                .Split(['.', ',', ';'], 2)[0]
                .Trim();
            string normalizedValue = value.ToLowerInvariant();

            // Keep this tutorial behavior deterministic and narrowly scoped.
            if (normalizedValue is "train" or "plane" or "car")
            {
                TravelProfile updatedProfile = new() { PreferredTransport = normalizedValue };
                _profiles.SaveState(context.Session, updatedProfile);
                Console.WriteLine($"[provider after] Stored transport preference: {normalizedValue}");
            }
        }

        return ValueTask.CompletedTask;
    }
}

// This is application-owned memory: one selected fact, not the chat transcript.
sealed class TravelProfile
{
    public string? PreferredTransport { get; init; }
}
