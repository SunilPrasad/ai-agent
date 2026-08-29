# Sources examined

## Repository refresh

- Official repository: `microsoft/agent-framework`, branch `main`
- Commit: `edfe115ea06bca57ae5a123d0fac5b3fdda13603`
- Verified: 2026-08-30 after fetch; zero local/remote divergence.

## Microsoft documentation

- [Agent concepts](https://learn.microsoft.com/en-us/agent-framework/concepts/agents/) — verified the agent run interface and message-based interaction model.
- [`AgentResponse`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.agentresponse?view=agent-framework-dotnet-latest) — verified messages, metadata, and `Text` behavior.
- [`AgentResponse.Messages`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.agentresponse.messages?view=agent-framework-dotnet-latest) — verified the list can contain multiple messages.

## Framework source, samples, and tests

- `dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs` — verified string, single-message, and collection `RunAsync` overloads.
- `dotnet/src/Microsoft.Agents.AI.Abstractions/AgentResponse.cs` — verified `ChatResponse` conversion, `Messages`, `Text`, and metadata preservation.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs` — verified request forwarding and response wrapping.
- `dotnet/samples/02-agents/AgentProviders/openai/Agent_With_OpenAIChatCompletion/Program.cs` — verified the official OpenAI-backed run pattern.
- `dotnet/tests/Microsoft.Agents.AI.Abstractions.UnitTests/AIAgentTests.cs` — verified convenience-overload behavior.
- `dotnet/tests/Microsoft.Agents.AI.Abstractions.UnitTests/AgentResponseTests.cs` — verified message and text behavior.

OpenRouter settings are authoring-project configuration, not framework documentation.
