using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
    ?? throw new InvalidOperationException("Set the OPENAI_API_KEY environment variable before running the sample.");

// Instructions describe stable agent behavior: how the agent should answer.
// Support hours are application data and may change, so SupportContextProvider
// supplies them separately each time the agent runs.
AIAgent agent = new ChatClient("gpt-5.4-mini", apiKey).AsAIAgent(
    new ChatClientAgentOptions
    {
        ChatOptions = new ChatOptions
        {
            Instructions = """
                You are a support assistant.
                Answer using the application context supplied to you.
                If the context does not contain the answer, say that you do not know.
                """
        },
        AIContextProviders = [new SupportContextProvider()]
    });

string question = args.Length > 0
    ? string.Join(' ', args)
    : "When is support available?";

Console.WriteLine($"QUESTION\n{question}");

// Before the model is called, the framework asks SupportContextProvider for the
// latest application data and adds it to this invocation's model context.
AgentResponse response = await agent.RunAsync(question);

Console.WriteLine($"\nANSWER\n{response.Text}");
