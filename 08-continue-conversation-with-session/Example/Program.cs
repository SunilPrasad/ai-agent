using System.ClientModel;
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
    instructions: "Help plan short city breaks. Answer briefly.",
    name: "TripPlanner");

AgentSession session = await agent.CreateSessionAsync();

Console.WriteLine("You: Plan a Saturday in Edinburgh focused on history.");
Console.WriteLine($"TripPlanner: {await agent.RunAsync("Plan a Saturday in Edinburgh focused on history.", session)}");

Console.WriteLine("\nYou: Make the morning activity free.");
Console.WriteLine($"TripPlanner: {await agent.RunAsync("Make the morning activity free.", session)}");
