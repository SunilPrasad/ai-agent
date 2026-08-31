# Sources

Framework commit examined: `6a0773ba2180e8036d138dbb9794ae64ec2d978b`

## Official documentation

- `https://learn.microsoft.com/en-us/agent-framework/journey/adding-context-providers` — verified that context providers run proactively before and after invocations, their contrast with model-selected tools, session awareness, and token/latency considerations.
- `https://learn.microsoft.com/en-us/agent-framework/integrations/by-component/context-providers/` — verified the distinction between conversation storage and memory as selected durable knowledge.
- `https://learn.microsoft.com/en-us/agent-framework/agents/agent-pipeline` — verified the separate `ChatHistoryProvider` and `AIContextProviders` roles in the chat-client agent context layer.
- `https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.aicontextprovider?view=agent-framework-dotnet-latest` — verified the two-phase lifecycle and security boundary for injected context.
- `https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.chathistoryprovider?view=agent-framework-dotnet-latest` — verified the chat-history provider's ordered transcript storage/retrieval responsibility.

## Framework implementation

- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AIContextProvider.cs` — verified before/after hooks, context merging, message filtering, failure behavior, session access, and storage security guidance.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/ProviderSessionState{TState}.cs` — verified provider state initialization, session state-bag storage, and serialization behavior.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs` — verified where chat history and registered context providers participate in a chat-client agent run.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgentOptions.cs` — verified the `AIContextProviders` registration surface and separate `ChatHistoryProvider` property.

## Official samples

- `framework/agent-framework/dotnet/samples/01-get-started/04_memory/Program.cs` — verified the supported custom `AIContextProvider` pattern, `ProviderSessionState<T>`, state keys, before/after overrides, and explicit transfer of selected state to a new session.
- `framework/agent-framework/dotnet/samples/02-agents/Agents/Agent_Step11_Middleware/Program.cs` — verified the minimal dynamic date/time context-provider pattern.
- `framework/agent-framework/dotnet/samples/02-agents/AgentWithMemory/AgentWithMemory_Step01_ChatHistoryMemory/Program.cs` — verified the official cross-conversation memory example and the use of distinct storage and search scopes.

## Tests

- `framework/agent-framework/dotnet/tests/Microsoft.Agents.AI.Abstractions.UnitTests/ProviderSessionStateTests.cs` — verified initialization, isolation across sessions, state keys, save/read behavior, and serialization round trips.
- `framework/agent-framework/dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgentTests.cs` — verified provider invocation ordering and after-hook notification behavior in the chat-client agent pipeline.
