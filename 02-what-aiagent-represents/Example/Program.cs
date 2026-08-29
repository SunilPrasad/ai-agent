// Copyright (c) Microsoft. All rights reserved.

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
    instructions: "Explain one .NET concept in one short sentence.",
    name: "DotNetTeacher");

Console.WriteLine($"Declared type in source: {nameof(AIAgent)}");
Console.WriteLine($"Runtime type: {agent.GetType().Name}");

await PrintAnswerAsync(agent, "What does abstraction mean in C#?");

static async Task PrintAnswerAsync(AIAgent agent, string question)
{
    AgentResponse response = await agent.RunAsync(question);
    Console.WriteLine($"{agent.Name}: {response.Text}");
}
