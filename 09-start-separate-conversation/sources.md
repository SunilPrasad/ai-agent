# Sources

Framework commit examined: `edfe115ea06bca57ae5a123d0fac5b3fdda13603`

- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AgentSession.cs` — verified that an agent session represents a specific conversation and may not be reusable across different agents.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs` — verified that each `CreateSessionAsync` call returns a new session for use with runs.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs` — verified per-session preparation and history-provider behavior.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/InMemoryChatHistoryProvider.cs` — verified that the default provider stores and retrieves messages through each session's state bag.
- `framework/agent-framework/dotnet/samples/01-get-started/03_multi_turn/Program.cs` — verified that a fresh `CreateSessionAsync` call starts a distinct conversation in the official sample.
- `framework/agent-framework/dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgent_ChatHistoryManagementTests.cs` — verified session-scoped history persistence and later retrieval.
