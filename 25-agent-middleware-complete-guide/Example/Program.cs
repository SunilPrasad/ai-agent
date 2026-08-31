using System.ClientModel;
using System.Diagnostics;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

// ============================================================
// SECTION 1: Configuration and the inner agent
// ============================================================
// Secrets and model selection stay outside source code.
string apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY")
    ?? throw new InvalidOperationException("OPENROUTER_API_KEY is not set.");
string model = Environment.GetEnvironmentVariable("OPENROUTER_MODEL")
    ?? throw new InvalidOperationException("OPENROUTER_MODEL is not set.");

IChatClient chatClient = new OpenAIClient(
        new ApiKeyCredential(apiKey),
        new OpenAIClientOptions { Endpoint = new Uri("https://openrouter.ai/api/v1") })
    .GetChatClient(model)
    .AsIChatClient();

// This is the inner agent. Middleware will wrap it; it does not replace it.
AIAgent innerAgent = chatClient.AsAIAgent(
    instructions: "You are a practical assistant for C# developers.",
    name: "DeveloperAssistant");

// ============================================================
// SECTION 2: Build an outer middleware pipeline
// ============================================================
// The first registered middleware is outermost. The small second middleware
// exists only to create a reproducible failure inside the pipeline.
AIAgent agent = innerAgent
    .AsBuilder()
    .Use(TraceAndPolicyMiddleware, runStreamingFunc: null)
    .Use(DemoFailureMiddleware, runStreamingFunc: null)
    .Build();

// ============================================================
// SECTION 3: Run the successful path
// ============================================================
// The application calls the built agent. Middleware runs before and after the
// inner agent, while the returned AgentResponse still comes from that inner run.
Console.WriteLine("=== SUCCESSFUL RUN ===");
AgentResponse response = await agent.RunAsync(
    "Why is dependency injection useful in a console application?");
Console.WriteLine($"Application received: {response.Text}");

// ============================================================
// SECTION 4: Run one deterministic failure path
// ============================================================
// This marker makes the tutorial's failure path reproducible. The outer
// middleware forwards it, then the inner demo middleware throws.
Console.WriteLine("\n=== FAILURE RUN ===");
try
{
    await agent.RunAsync("[demo-failure]");
}
catch (InvalidOperationException exception)
{
    // Middleware logged the failure and rethrew it. Application code still
    // receives the original exception type and message and owns recovery.
    Console.WriteLine($"Application caught: {exception.GetType().Name}: {exception.Message}");
}

// ============================================================
// SECTION 5: The outer middleware handles cross-cutting behavior
// ============================================================
async Task<AgentResponse> TraceAndPolicyMiddleware(
    IEnumerable<ChatMessage> messages,
    AgentSession? session,
    AgentRunOptions? options,
    AIAgent nextAgent,
    CancellationToken cancellationToken)
{
    string requestId = Guid.NewGuid().ToString("N")[..8];
    Stopwatch stopwatch = Stopwatch.StartNew();

    // Materialize once because IEnumerable may be lazy. Keep the original
    // ChatMessage objects unchanged so later code can reason about its input.
    List<ChatMessage> originalMessages = messages.ToList();

    Console.WriteLine($"[middleware before] id={requestId} messages={originalMessages.Count}");

    try
    {
        // Request modification is powerful. Add only a fixed, developer-owned
        // instruction; never promote untrusted user text into a system message.
        List<ChatMessage> forwardedMessages =
        [
            new ChatMessage(
                ChatRole.System,
                "Middleware policy: keep the final answer to no more than two short sentences."),
            .. originalMessages
        ];

        Console.WriteLine($"[middleware change] id={requestId} added=fixed-length-policy");

        // Calling the supplied inner agent continues the pipeline. Forward the
        // same session, options, and cancellation token unless policy requires
        // a deliberate, documented change.
        AgentResponse innerResponse = await nextAgent.RunAsync(
            forwardedMessages,
            session,
            options,
            cancellationToken);

        Console.WriteLine(
            $"[middleware after] id={requestId} success=true elapsedMs={stopwatch.ElapsedMilliseconds}");

        return innerResponse;
    }
    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
    {
        // Cancellation is expected control flow. Log it separately and rethrow
        // so callers still observe cancellation rather than a replacement error.
        Console.WriteLine(
            $"[middleware canceled] id={requestId} elapsedMs={stopwatch.ElapsedMilliseconds}");
        throw;
    }
    catch (Exception exception)
    {
        // Observe safe diagnostics without logging prompts, secrets, or full
        // provider payloads. `throw;` preserves the original exception object
        // and stack; `throw exception;` would reset part of the stack trace.
        Console.WriteLine(
            $"[middleware error] id={requestId} type={exception.GetType().Name} " +
            $"elapsedMs={stopwatch.ElapsedMilliseconds}");
        throw;
    }
}

// ============================================================
// SECTION 6: Tutorial-only inner middleware creates the failure
// ============================================================
// This layer proves that the outer catch observes an exception crossing back
// through `nextAgent.RunAsync`. Production code would not trigger failures from
// prompt text; real downstream failures can come from inner agents or providers.
async Task<AgentResponse> DemoFailureMiddleware(
    IEnumerable<ChatMessage> messages,
    AgentSession? session,
    AgentRunOptions? options,
    AIAgent nextAgent,
    CancellationToken cancellationToken)
{
    List<ChatMessage> forwardedMessages = messages.ToList();

    if (forwardedMessages.Any(message =>
        message.Text.Contains("[demo-failure]", StringComparison.Ordinal)))
    {
        Console.WriteLine("[inner demo middleware] throwing deterministic failure");
        throw new InvalidOperationException("Demonstration failure inside the inner pipeline.");
    }

    return await nextAgent.RunAsync(
        forwardedMessages,
        session,
        options,
        cancellationToken);
}
