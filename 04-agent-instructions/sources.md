# Sources examined

## Repository refresh

- Official repository: `microsoft/agent-framework`, branch `main`
- Commit: `edfe115ea06bca57ae5a123d0fac5b3fdda13603`
- Verified: 2026-08-30 after fetch; local and remote had zero divergence.

## Microsoft documentation

- [Agent concepts](https://learn.microsoft.com/en-us/agent-framework/concepts/agents/) — verified that instructions are part of an agent's behavior/configuration.
- [`ChatClientAgent` constructor](https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.chatclientagent.-ctor?view=agent-framework-dotnet-latest) — verified that instructions guide behavior and are supplied to `IChatClient` for invocations.
- [`ChatClientAgentOptions`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.chatclientagentoptions?view=agent-framework-dotnet-latest) — verified the agent options and `ChatOptions` relationship.

## Framework source, samples, and tests

- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs` — verified constructor mapping to `ChatOptions.Instructions` and the client call.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgentOptions.cs` — verified configuration ownership.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientExtensions.cs` — verified the `AsAIAgent(instructions: ...)` convenience path.
- `dotnet/samples/02-agents/AgentProviders/openai/Agent_With_OpenAIChatCompletion/Program.cs` — verified official instruction configuration with an OpenAI chat client.
- `dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgentTests.cs` — verified instruction storage and forwarding through `ChatOptions`.

OpenRouter values are project configuration, not authority for framework behavior.
