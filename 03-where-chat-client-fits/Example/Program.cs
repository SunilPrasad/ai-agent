using System.ClientModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;

string apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY")
    ?? throw new InvalidOperationException("OPENROUTER_API_KEY is not set.");
string model = Environment.GetEnvironmentVariable("OPENROUTER_MODEL")
    ?? throw new InvalidOperationException("OPENROUTER_MODEL is not set.");

OpenAIClient openAIClient = new(
    new ApiKeyCredential(apiKey),
    new OpenAIClientOptions { Endpoint = new Uri("https://openrouter.ai/api/v1") });

ChatClient sdkChatClient = openAIClient.GetChatClient(model);
IChatClient chatClient = sdkChatClient.AsIChatClient();
AIAgent agent = chatClient.AsAIAgent(
    instructions: "Answer in one short sentence.",
    name: "LayerExplorer");

Console.WriteLine($"1. SDK client: {sdkChatClient.GetType().Name}");
Console.WriteLine($"2. Common client: {chatClient.GetType().Name}");
Console.WriteLine($"3. Agent: {agent.GetType().Name}");

AgentResponse response = await agent.RunAsync("Why is a software boundary useful?");
Console.WriteLine($"4. Answer: {response.Text}");
