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
    instructions: "Explain clearly in four short sentences.",
    name: "StreamingTeacher");

Console.Write("StreamingTeacher: ");

int updateCount = 0;
await foreach (AgentResponseUpdate update in agent.RunStreamingAsync(
    "Why does async/await help a web server?"))
{
    Console.Write(update.Text);
    updateCount++;
}

Console.WriteLine($"\n\nUpdates received: {updateCount}");
