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

AIAgent conciseAgent = chatClient.AsAIAgent(
    instructions: "Answer in exactly one short sentence.",
    name: "ConciseTeacher");

AIAgent stepAgent = chatClient.AsAIAgent(
    instructions: "Answer as exactly three short numbered steps for a beginner.",
    name: "StepTeacher");

const string Question = "How do I create a new C# console project?";

Console.WriteLine($"{conciseAgent.Name}:");
Console.WriteLine((await conciseAgent.RunAsync(Question)).Text);

Console.WriteLine($"\n{stepAgent.Name}:");
Console.WriteLine((await stepAgent.RunAsync(Question)).Text);
