using System.ClientModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

// ============================================================
// SECTION 1: Configuration and model client
// ============================================================
// The OpenRouter key and model stay outside source code.
string apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY")
    ?? throw new InvalidOperationException("OPENROUTER_API_KEY is not set.");
string model = Environment.GetEnvironmentVariable("OPENROUTER_MODEL")
    ?? throw new InvalidOperationException("OPENROUTER_MODEL is not set.");

// OpenAIClient belongs to the OpenAI .NET SDK. AsIChatClient adapts its
// OpenRouter-backed chat client to Microsoft.Extensions.AI.IChatClient.
IChatClient chatClient = new OpenAIClient(
        new ApiKeyCredential(apiKey),
        new OpenAIClientOptions { Endpoint = new Uri("https://openrouter.ai/api/v1") })
    .GetChatClient(model)
    .AsIChatClient();

// Microsoft Agent Framework wraps the IChatClient in an AIAgent.
AIAgent agent = chatClient.AsAIAgent(
    instructions: "Create concise city-break plans from the user's requirements.",
    name: "CityPlanner");

const string Request = "Create a two-day Berlin plan with exactly three activities and an estimated daily budget in GBP.";

// ============================================================
// SECTION 2: Free-form text is easy for people to read
// ============================================================
// The non-generic RunAsync returns AgentResponse. Its Text property is one
// string whose headings, punctuation, and ordering are chosen by the model.
AgentResponse freeFormResponse = await agent.RunAsync(Request);

Console.WriteLine("=== FREE-FORM RESPONSE ===");
Console.WriteLine(freeFormResponse.Text);
Console.WriteLine($"Application received a {freeFormResponse.Text.GetType().Name}.");

// A person can understand this text, but application code has no Destination,
// Days, EstimatedDailyBudgetGbp, or Activities properties to read. Parsing the
// prose would depend on formatting that the model may change on another run.

// ============================================================
// SECTION 3: Define the exact shape the application needs
// ============================================================
// TripPlan is an ordinary C# class. Its property types become part of the JSON
// schema sent to a model that supports structured output. Descriptive property
// names make the requested meaning and units clear in this small contract.

// ============================================================
// SECTION 4: Request one typed result with RunAsync<T>
// ============================================================
// Supplying TripPlan as T asks the framework for that schema and returns
// AgentResponse<TripPlan>. A fresh implicit session keeps this comparison
// independent from the earlier free-form run.
AgentResponse<TripPlan> typedResponse = await agent.RunAsync<TripPlan>(Request);

// Text exposes the concatenated response text containing the JSON. Result
// deserializes that JSON into the requested C# type using System.Text.Json.
TripPlan plan = typedResponse.Result;

Console.WriteLine("\n=== STRUCTURED JSON ===");
Console.WriteLine(typedResponse.Text);

// ============================================================
// SECTION 5: Use the result as normal typed C# data
// ============================================================
// No heading detection, regular expression, or manual JSON parsing is needed.
Console.WriteLine("\n=== TYPED C# VALUES ===");
Console.WriteLine($"Destination: {plan.Destination}");
Console.WriteLine($"Days: {plan.Days}");
Console.WriteLine($"Daily budget: £{plan.EstimatedDailyBudgetGbp:0.00}");
Console.WriteLine($"Activity count: {plan.Activities.Count}");

// Typed values can drive ordinary application decisions.
string budgetCategory = plan.EstimatedDailyBudgetGbp <= 150m
    ? "Within the demo's budget threshold"
    : "Above the demo's budget threshold";

Console.WriteLine($"Budget check: {budgetCategory}");

for (int index = 0; index < plan.Activities.Count; index++)
{
    Console.WriteLine($"{index + 1}. {plan.Activities[index]}");
}

sealed class TripPlan
{
    public required string Destination { get; init; }

    public required int Days { get; init; }

    public required decimal EstimatedDailyBudgetGbp { get; init; }

    public required List<string> Activities { get; init; }
}
