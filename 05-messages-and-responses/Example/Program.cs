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
    instructions: "Answer in one short sentence.",
    name: "MessageExplorer");

ChatMessage request = new(ChatRole.User, "What is dependency injection?");
AgentResponse response = await agent.RunAsync(request);

Console.WriteLine($"Sent role: {request.Role}");
Console.WriteLine($"Returned messages: {response.Messages.Count}");

foreach (ChatMessage message in response.Messages)
{
    Console.WriteLine($"{message.Role}: {message.Text}");
}

Console.WriteLine($"Convenient combined text: {response.Text}");
