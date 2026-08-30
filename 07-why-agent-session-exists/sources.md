# Sources

Framework commit examined: `edfe115ea06bca57ae5a123d0fac5b3fdda13603`

- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AgentSession.cs` — verified that a session represents one conversation's state, is created by an agent, may contain history or references, and supports serialization.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs` — verified the public `CreateSessionAsync`, `RunAsync`, serialization, and deserialization boundaries.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs` — verified that the chat-client implementation creates `ChatClientAgentSession` and prepares session-backed history for a run.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgentSession.cs` — verified the concrete session shape, conversation identifier, state-bag serialization, and JSON validation.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/InMemoryChatHistoryProvider.cs` — verified that the default provider stores and retrieves messages through each session's state bag.
- `framework/agent-framework/dotnet/samples/01-get-started/03_multi_turn/Program.cs` — verified the official basic pattern of creating an `AgentSession` and passing it to runs.
- `framework/agent-framework/dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgent_CreateSessionTests.cs` — verified the concrete session type used by `ChatClientAgent`.
