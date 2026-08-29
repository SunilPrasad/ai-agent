# Sources examined

Framework behavior was verified against the current official repository rather than assumed from pretrained API knowledge.

## Repository refresh

- Official repository: `microsoft/agent-framework`
- Branch: `main`
- Examined commit: `edfe115ea06bca57ae5a123d0fac5b3fdda13603`
- Verified on: 2026-08-30
- Refresh result: `git fetch origin --prune` succeeded; local `main` and `origin/main` had zero commits of divergence, so no pull was required.

## Microsoft documentation

- [Agent concepts](https://learn.microsoft.com/en-us/agent-framework/concepts/agents/)
  - Verified the agent mental model, the consistent run interface, `ChatClientAgent` using `IChatClient`, and application/orchestration code working through the shared agent abstraction.
- [`AIAgent` class API reference](https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.aiagent?view=agent-framework-dotnet-latest)
  - Verified that the C# type is an abstract class in `Microsoft.Agents.AI` and reviewed its public surface and derived types.
- [`AIAgent.RunAsync` API reference](https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.aiagent.runasync?view=agent-framework-dotnet-latest)
  - Verified the non-streaming invocation overloads used by the example.
- [`AIAgent.CreateSessionAsync` API reference](https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.aiagent.createsessionasync?view=agent-framework-dotnet-latest)
  - Verified that session creation is part of the common abstraction; this lesson mentions it but deliberately defers teaching sessions.
- [Custom agents](https://learn.microsoft.com/en-us/agent-framework/user-guide/agents/agent-types/custom-agent)
  - Verified that custom implementations derive from `AIAgent` and implement the core run behavior.
- [`DelegatingAIAgent` API reference](https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.delegatingaiagent?view=agent-framework-dotnet-latest)
  - Verified the wrapper-oriented derived abstraction shown in the type diagram.

## Framework implementation

All paths are relative to `framework/agent-framework/`.

- `dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs`
  - Verified the abstract base-class declaration, identity members, `GetService`, session operations, `RunAsync` overloads, string-to-user-message conversion, current run context, and delegation to abstract `RunCoreAsync`.
- `dotnet/src/Microsoft.Agents.AI.Abstractions/DelegatingAIAgent.cs`
  - Verified that agent decorators can share the same `AIAgent` contract while forwarding work to an inner agent.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs`
  - Verified that `ChatClientAgent` is a sealed `AIAgent` implementation and that its core run path prepares a request, calls `IChatClient`, and produces `AgentResponse`.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientExtensions.cs`
  - Verified that `IChatClient.AsAIAgent(...)` creates `ChatClientAgent`.
- `dotnet/src/Microsoft.Agents.AI.OpenAI/Microsoft.Agents.AI.OpenAI.csproj`
  - Verified the framework adapter project's dependency on `Microsoft.Extensions.AI.OpenAI`, which supplies the OpenAI-specific `AsIChatClient()` adapter.
- `dotnet/src/Microsoft.Agents.AI.A2A/A2AAgent.cs`
  - Verified the public `A2AAgent : AIAgent` relationship used only in the type diagram.
- `dotnet/src/Microsoft.Agents.AI.Harness/HarnessAgent.cs`
  - Verified the public `HarnessAgent : DelegatingAIAgent` relationship used only in the type diagram.

## Official samples

- `dotnet/samples/01-get-started/01_hello_agent/Program.cs`
  - Verified the current minimal `AIAgent` creation and `RunAsync` usage pattern.
- `dotnet/samples/02-agents/AgentProviders/openai/Agent_With_OpenAIChatCompletion/Program.cs`
  - Verified declaring the result as `AIAgent`, adapting an OpenAI chat client, and invoking `RunAsync`.

## Tests

- `dotnet/tests/Microsoft.Agents.AI.Abstractions.UnitTests/AIAgentTests.cs`
  - Verified base-class invocation behavior, convenience overloads, validation, run context, and service access.
- `dotnet/tests/Microsoft.Agents.AI.Abstractions.UnitTests/DelegatingAIAgentTests.cs`
  - Verified delegation behavior through the shared base abstraction.
- `dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgentTests.cs`
  - Verified that `ChatClientAgent` invokes its `IChatClient` and returns agent responses.

## OpenRouter project configuration

The OpenRouter endpoint and the `OPENROUTER_API_KEY` / `OPENROUTER_MODEL` variable names come from the authoring project's `AGENTS.md`. They are transport configuration, not sources for Microsoft Agent Framework behavior. No third-party documentation was used for framework claims.
