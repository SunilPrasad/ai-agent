// Copyright (c) Microsoft. All rights reserved.

// This example isolates agent creation by adapting an OpenRouter-backed IChatClient into an AIAgent.

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
    name: "ConceptExplorer",
    description: "Shows the smallest IChatClient-to-AIAgent adapter path.");

AgentResponse response = await agent.RunAsync("What is an agent?");

Console.WriteLine($"Agent: {agent.Name}");
Console.WriteLine($"Response: {response.Text}");
