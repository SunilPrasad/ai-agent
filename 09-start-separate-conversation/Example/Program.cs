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
    instructions: "Remember details only within the supplied conversation. Answer briefly.",
    name: "TripPlanner");

AgentSession parisSession = await agent.CreateSessionAsync();
AgentSession osloSession = await agent.CreateSessionAsync();

await agent.RunAsync("My trip is to Paris and I enjoy art.", parisSession);
await agent.RunAsync("My trip is to Oslo and I enjoy hiking.", osloSession);

Console.WriteLine($"Paris session: {await agent.RunAsync("Which city and interest did I mention?", parisSession)}");
Console.WriteLine($"Oslo session: {await agent.RunAsync("Which city and interest did I mention?", osloSession)}");
