using Microsoft.Agents.AI;
using OpenAI.Chat;

string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
// Create a Microsoft Agent Framework agent backed by an OpenAI chat model.
AIAgent agent = new ChatClient("gpt-5.4-mini", apiKey).AsAIAgent(
    instructions: "You are a helpful assistant. Answer clearly and concisely.",
    name: "SimpleAgent");

string question = args.Length > 0
    ? string.Join(' ', args)
    : "What is retrieval-augmented generation?";

Console.WriteLine($"QUESTION\n{question}");

// RunAsync sends the question to the model and returns the agent's response.
AgentResponse response = await agent.RunAsync(question);

Console.WriteLine($"\nANSWER\n{response.Text}");
