# Sources examined

## Repository refresh

- Official repository: `microsoft/agent-framework`, branch `main`
- Commit: `edfe115ea06bca57ae5a123d0fac5b3fdda13603`
- Verified: 2026-08-30 after fetch; zero local/remote divergence.

## Microsoft documentation

- [`AIAgent.RunStreamingAsync`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.aiagent.runstreamingasync?view=agent-framework-dotnet-latest) — verified overloads, return type, update semantics, and string-to-user-message behavior.
- [`AgentResponseUpdate`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.agentresponseupdate?view=agent-framework-dotnet-latest) — verified that it represents one streaming update and exposes `Text`.
- [Agent pipeline architecture](https://learn.microsoft.com/en-us/agent-framework/concepts/agents/agent-pipeline) — verified the `ChatClientAgent` and `IChatClient` pipeline.

## Framework source, samples, and tests

- `dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs` — verified streaming overloads and delegation to `RunCoreStreamingAsync`.
- `dotnet/src/Microsoft.Agents.AI.Abstractions/AgentResponseUpdate.cs` — verified update content and `Text` behavior.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs` — verified `GetStreamingResponseAsync` and `ChatResponseUpdate` conversion.
- `dotnet/samples/01-get-started/01_hello_agent/Program.cs` — verified official streaming enumeration.
- `dotnet/samples/01-get-started/03_multi_turn/Program.cs` — verified another official `await foreach` pattern.
- `dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgentTests.cs` — verified multiple streaming updates and their text.
- `dotnet/tests/Microsoft.Agents.AI.Abstractions.UnitTests/AgentResponseUpdateTests.cs` — verified update properties and text.

OpenRouter configuration is project policy, not a source for Agent Framework behavior.
