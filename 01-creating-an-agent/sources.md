# Sources examined

Framework behavior was verified against the checked-out Microsoft Agent Framework repository. Repository paths below are relative to its root.

## Framework implementation

- `dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs`
  - Verified the common agent abstraction, `RunAsync` overloads, and conversion of string input into a user message.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientExtensions.cs`
  - Verified that `IChatClient.AsAIAgent(...)` constructs a `ChatClientAgent` with the supplied instructions and name.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs`
  - Verified that `ChatClientAgent` derives from `AIAgent`, prepares messages and options, calls `IChatClient.GetResponseAsync(...)`, and returns an `AgentResponse`.
- `dotnet/src/Microsoft.Agents.AI.Abstractions/AgentResponse.cs`
  - Verified the response abstraction and its `Text` property.
- `dotnet/src/Microsoft.Agents.AI.OpenAI/Extensions/OpenAIChatClientExtensions.cs`
  - Verified the OpenAI chat-client adapter path used by the example.

## Official .NET sample

- `dotnet/samples/02-agents/AgentProviders/openai/Agent_With_OpenAIChatCompletion/Program.cs`
  - Verified the current pattern for creating an OpenAI chat-completion agent and calling `RunAsync`.

## Tests

- `dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgentTests.cs`
  - Verified instruction and identity propagation, chat-client invocation, and response handling.
- `dotnet/tests/Microsoft.Agents.AI.OpenAI.UnitTests/Extensions/OpenAIChatClientExtensionsTests.cs`
  - Verified the OpenAI adapter creates an agent with the requested instructions and name.

## OpenRouter compatibility

- `https://openrouter.ai/docs/quickstart`
  - Verified the OpenAI-compatible base URL, API-key authentication pattern, and OpenRouter model identifiers.
