# Sources

Framework commit examined: `edfe115ea06bca57ae5a123d0fac5b3fdda13603`

- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AgentSession.cs` — verified session contents, agent ownership, serialization support, compatibility warning, and security guidance.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs` — verified `SerializeSessionAsync` and `DeserializeSessionAsync` signatures and documented persistence/security contract.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs` — verified concrete session type checking and chat-client serialization/deserialization implementation.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgentSession.cs` — verified JSON shape validation and concrete session reconstruction during deserialization.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/InMemoryChatHistoryProvider.cs` — verified that this example's default history is stored in the session state that is serialized.
- `framework/agent-framework/dotnet/samples/02-agents/Agents/Agent_Step03_PersistedConversations/Program.cs` — verified the official create → run → serialize → deserialize → resume pattern.
- `framework/agent-framework/dotnet/samples/02-agents/Agents/Agent_Step04_3rdPartyChatHistoryStorage/Program.cs` — verified that state used by a history provider can round-trip through `AgentSession.StateBag`.
- `framework/agent-framework/dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/MessageInjectingChatClientTests.cs` — verified chat-client session serialization and restored session behavior are exercised by tests.
