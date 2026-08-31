using System.ClientModel;
using System.ComponentModel;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

// ============================================================
// SECTION 1: Configuration and the shared model client
// ============================================================
// Secrets and the model name stay outside source code.
string apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY")
    ?? throw new InvalidOperationException("OPENROUTER_API_KEY is not set.");
string model = Environment.GetEnvironmentVariable("OPENROUTER_MODEL")
    ?? throw new InvalidOperationException("OPENROUTER_MODEL is not set.");

// OpenAIClient is from the OpenAI .NET SDK. Pointing it at OpenRouter's
// OpenAI-compatible endpoint gives us an SDK chat client. AsIChatClient()
// adapts that client to Microsoft.Extensions.AI.IChatClient.
IChatClient chatClient = new OpenAIClient(
        new ApiKeyCredential(apiKey),
        new OpenAIClientOptions { Endpoint = new Uri("https://openrouter.ai/api/v1") })
    .GetChatClient(model)
    .AsIChatClient();

// ============================================================
// SECTION 2: Turn ordinary C# methods into function tools
// ============================================================
// AIFunctionFactory reads each delegate's name, descriptions, parameters,
// and return type. It creates metadata the model can see plus an invocable
// delegate the framework can execute locally.
AIFunction weatherTool = AIFunctionFactory.Create(GetWeather);
AIFunction currencyTool = AIFunctionFactory.Create(ConvertCurrency);
AIFunction travelTimeTool = AIFunctionFactory.Create(GetTravelTime);

// ============================================================
// SECTION 3: Turn a specialist agent into another function tool
// ============================================================
// The specialist has a narrow responsibility. AsAIFunction() exposes it as
// a tool whose input is a query string and whose result is the agent's text.
AIAgent packingSpecialist = chatClient.AsAIAgent(
    instructions: "Recommend exactly three practical packing items. Be concise.",
    name: "PackingSpecialist",
    description: "Creates a short packing list for a destination and weather.");

AIFunction packingSpecialistTool = packingSpecialist.AsAIFunction();

// ============================================================
// SECTION 4: Register every tool on the main agent
// ============================================================
// The descriptions and parameter schemas help the model choose a tool and
// construct its arguments. The model chooses; C# executes the selected tool.
AIAgent travelAgent = chatClient.AsAIAgent(
    instructions: """
        You are a travel assistant.
        Use the available tools when a request needs their data.
        Never invent a tool result.
        If a tool returns Success=false, explain that failure clearly.
        Keep each final answer short.
        """,
    name: "TravelAssistant",
    tools: [weatherTool, currencyTool, travelTimeTool, packingSpecialistTool]);

// ============================================================
// SECTION 5: One tool call and a returned result
// ============================================================
// Flow: model requests GetWeather -> C# method runs -> its string result is
// sent back to the model -> the model writes the final natural-language answer.
await AskAsync(
    "1. ONE TOOL",
    "Use the weather tool to tell me the weather in Edinburgh.");

// ============================================================
// SECTION 6: Tool arguments become typed C# parameters
// ============================================================
// The model produces arguments matching the generated schema. The function
// invocation layer converts them into decimal/string parameters before calling
// ConvertCurrency. Its console log makes those received values visible.
await AskAsync(
    "2. PARAMETERS AND RESULT",
    "Use the currency tool to convert 125 GBP to EUR.");

// ============================================================
// SECTION 7: Let the model choose between registered tools
// ============================================================
// This request needs weather, not currency or travel time. Tool names,
// descriptions, and parameter schemas guide the model's selection.
await AskAsync(
    "3. TOOL CHOICE",
    "I only need the fixed demonstration weather for Lisbon. Choose the relevant tool.");

// ============================================================
// SECTION 8: Represent an expected tool-domain failure explicitly
// ============================================================
// GetTravelTime returns Success=false for an unsupported route. That keeps the
// failure visible as data instead of throwing away the reason or pretending the
// call worked. The agent is instructed to explain the failure to the user.
await AskAsync(
    "4. EXPECTED TOOL-DOMAIN FAILURE",
    "Call the travel-time tool for Atlantis to Paris, even if the route is unsupported.");

// ============================================================
// SECTION 9: Use an agent as a tool
// ============================================================
// The main agent can delegate this narrow request to PackingSpecialist through
// the AIFunction created above, then use the specialist's text as a tool result.
await AskAsync(
    "5. AGENT AS A TOOL",
    "Ask the packing specialist what to pack for rainy Edinburgh.");

async Task AskAsync(string heading, string prompt)
{
    Console.WriteLine($"\n=== {heading} ===");
    Console.WriteLine($"You: {prompt}");

    // Each demonstration gets a fresh outer session so earlier prompts and tool
    // results cannot influence which tool the model chooses in this run.
    AgentSession demonstrationSession = await travelAgent.CreateSessionAsync();
    AgentResponse response = await travelAgent.RunAsync(prompt, demonstrationSession);

    Console.WriteLine($"TravelAssistant: {response.Text}");
}

// ============================================================
// LOCAL TOOL IMPLEMENTATIONS
// ============================================================

[Description("Gets fixed demonstration weather data for a supported city.")]
static string GetWeather(
    [Description("The city whose weather is required.")] string city)
{
    Console.WriteLine($"[tool] GetWeather received city='{city}'");

    return city.Trim().ToLowerInvariant() switch
    {
        "edinburgh" => "Edinburgh: light rain, 12°C.",
        "lisbon" => "Lisbon: sunny, 24°C.",
        _ => $"Weather data is unavailable for {city}."
    };
}

[Description("Converts an amount between supported currencies using fixed demonstration rates.")]
static CurrencyResult ConvertCurrency(
    [Description("The amount to convert.")] decimal amount,
    [Description("Three-letter source currency code, such as GBP.")] string fromCurrency,
    [Description("Three-letter target currency code, such as EUR.")] string toCurrency)
{
    Console.WriteLine(
        $"[tool] ConvertCurrency received amount={amount}, from='{fromCurrency}', to='{toCurrency}'");

    string pair = $"{fromCurrency.Trim().ToUpperInvariant()}-{toCurrency.Trim().ToUpperInvariant()}";
    decimal? rate = pair switch
    {
        "GBP-EUR" => 1.17m,
        "EUR-GBP" => 0.85m,
        "USD-EUR" => 0.92m,
        _ => null
    };

    return rate is null
        ? new CurrencyResult(false, null, $"The demonstration has no rate for {pair}.")
        : new CurrencyResult(true, decimal.Round(amount * rate.Value, 2), $"Converted using rate {rate}.");
}

[Description("Gets an estimated direct travel time between two supported cities.")]
static TravelTimeResult GetTravelTime(
    [Description("The departure city.")] string origin,
    [Description("The destination city.")] string destination)
{
    Console.WriteLine($"[tool] GetTravelTime received origin='{origin}', destination='{destination}'");

    string route = $"{origin.Trim().ToLowerInvariant()}-{destination.Trim().ToLowerInvariant()}";

    return route switch
    {
        "edinburgh-london" => new TravelTimeResult(true, 270, "Direct train estimate."),
        "lisbon-porto" => new TravelTimeResult(true, 180, "Direct train estimate."),
        _ => new TravelTimeResult(false, null, $"No supported route from {origin} to {destination}.")
    };
}

// Records become JSON-shaped tool results. Success makes failure or success
// explicit; the optional numeric value is present only when the operation works.
sealed record CurrencyResult(bool Success, decimal? ConvertedAmount, string Message);
sealed record TravelTimeResult(bool Success, int? Minutes, string Message);
