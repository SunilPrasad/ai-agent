# Sources examined

## Repository refresh

- Official repository: `microsoft/agent-framework`
- Branch: `main`
- Commit: `edfe115ea06bca57ae5a123d0fac5b3fdda13603`
- Verified: 2026-08-30 after successful `git fetch origin --prune`; local `main` and `origin/main` had zero divergence.

## Microsoft documentation

- [Agent concepts](https://learn.microsoft.com/en-us/agent-framework/concepts/agents/) — verified the shared agent and chat-client mental model.
- [Agent pipeline architecture](https://learn.microsoft.com/en-us/agent-framework/concepts/agents/agent-pipeline) — verified that `ChatClientAgent` uses `IChatClient` and that the chat-client layer communicates with the LLM service.

## Framework source

- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientExtensions.cs` — verified that `IChatClient.AsAIAgent()` creates `ChatClientAgent`.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs` — verified the `IChatClient.GetResponseAsync` call and `AgentResponse` conversion.
- `dotnet/src/Microsoft.Agents.AI.OpenAI/Microsoft.Agents.AI.OpenAI.csproj` — verified the `Microsoft.Extensions.AI.OpenAI` adapter dependency.
- `dotnet/samples/02-agents/AgentProviders/openai/Agent_With_OpenAIChatCompletion/Program.cs` — verified the official OpenAI chat-completion construction path.
- `dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgentTests.cs` — verified calls and option forwarding to `IChatClient`.

OpenRouter endpoint and environment-variable names are project configuration. No third-party source was used for Agent Framework claims.
