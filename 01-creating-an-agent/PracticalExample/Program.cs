// Copyright (c) Microsoft. All rights reserved.

// This example creates an OpenRouter-backed agent for first-pass incident triage.

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

AIAgent triageAgent = chatClient.AsAIAgent(
        instructions:
            """
            You triage software incidents. Return exactly three labelled lines:
            Severity: Sev-1, Sev-2, or Sev-3
            Reason: one short sentence
            Next action: one concrete action
            Treat security incidents, widespread outages, and active data loss as Sev-1.
            """,
        name: "IncidentTriage",
        description: "Classifies incoming software incidents and recommends the first action.");

const string Incident =
    "Customers in every region receive HTTP 500 responses, and orders cannot be submitted.";

AgentResponse response = await triageAgent.RunAsync(Incident);

Console.WriteLine($"Agent: {triageAgent.Name}");
Console.WriteLine(response.Text);
