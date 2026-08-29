using System.ClientModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

string apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY")
    ?? throw new InvalidOperationException("OPENROUTER_API_KEY is not set.");
string model = Environment.GetEnvironmentVariable("OPENROUTER_MODEL")
    ?? throw new InvalidOperationException("OPENROUTER_MODEL is not set.");

OpenAIClient openAIClient = new(
    new ApiKeyCredential(apiKey),
    new OpenAIClientOptions
    {
        Endpoint = new Uri("https://openrouter.ai/api/v1")
    });

IChatClient chatClient = openAIClient
    .GetChatClient(model)
    .AsIChatClient();

AIAgent agent = chatClient.AsAIAgent(
    instructions: "Explain the requested AI concept in one short sentence for a .NET developer.",
    name: "DotNetTeacher");

AgentResponse response = await agent.RunAsync("What is an AI agent?");

Console.WriteLine(response.Text);
