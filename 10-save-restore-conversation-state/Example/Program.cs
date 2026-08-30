using System.ClientModel;
using System.Text.Json;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

string apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY")
    ?? throw new InvalidOperationException("OPENROUTER_API_KEY is not set.");
string model = Environment.GetEnvironmentVariable("OPENROUTER_MODEL")
    ?? throw new InvalidOperationException("OPENROUTER_MODEL is not set.");

IChatClient chatClient = new OpenAIClient(
        new ApiKeyCredential(apiKey),
        new OpenAIClientOptions { Endpoint = new Uri("https://openrouter.ai/api/v1") })
    .GetChatClient(model)
    .AsIChatClient();

AIAgent agent = chatClient.AsAIAgent(
    instructions: "Remember trip preferences within the conversation. Answer briefly.",
    name: "TripPlanner");

AgentSession session = await agent.CreateSessionAsync();
Console.WriteLine(await agent.RunAsync("I am visiting Lisbon and prefer vegetarian food.", session));

JsonElement savedState = await agent.SerializeSessionAsync(session);
string stateFile = Path.Combine(AppContext.BaseDirectory, "trip-session.json");
await File.WriteAllTextAsync(stateFile, savedState.GetRawText());
Console.WriteLine($"Saved session to {stateFile}");

using JsonDocument document = JsonDocument.Parse(await File.ReadAllTextAsync(stateFile));
AgentSession restoredSession = await agent.DeserializeSessionAsync(document.RootElement.Clone());

Console.WriteLine(await agent.RunAsync("Which city and food preference did I mention?", restoredSession));
