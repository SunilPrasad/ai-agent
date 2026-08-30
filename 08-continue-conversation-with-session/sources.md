# Sources

Framework commit examined: `edfe115ea06bca57ae5a123d0fac5b3fdda13603`

- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs` — verified the overloads that accept input plus an `AgentSession`.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AgentSession.cs` — verified the per-conversation purpose and agent ownership of sessions.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs` — verified session/message preparation and history-provider notifications around successful runs.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/InMemoryChatHistoryProvider.cs` — verified that the default history provider retrieves and stores messages in session state.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgentSession.cs` — verified the concrete session's state-bag and optional conversation-identifier shape.
- `framework/agent-framework/dotnet/samples/01-get-started/03_multi_turn/Program.cs` — verified only the public two-turn same-session pattern; the chat-client implementation and tests above verify this lesson's local-history behavior.
- `framework/agent-framework/dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgent_ChatHistoryManagementTests.cs` — verified that completed turns are persisted and prior history is supplied on later runs.
