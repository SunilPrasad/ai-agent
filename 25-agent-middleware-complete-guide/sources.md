# Sources

Framework commit examined: `6a0773ba2180e8036d138dbb9794ae64ec2d978b`

## Official documentation

- `https://learn.microsoft.com/en-us/agent-framework/agents/middleware/` — verified the three middleware scopes, callback chain, `next` behavior, and supported cross-cutting concerns.
- `https://learn.microsoft.com/en-us/agent-framework/journey/adding-middleware` — verified the wrapper mental model, request/response interception, modification use cases, and pipeline guidance.
- `https://learn.microsoft.com/en-us/agent-framework/agents/agent-pipeline` — verified that agent middleware is outside the context and chat-client layers for a `ChatClientAgent`.
- `https://learn.microsoft.com/en-us/agent-framework/agents/middleware/exception-handling` — verified the documented C# pattern for wrapping the inner agent call with exception handling.
- `https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.aiagentbuilder.use?view=agent-framework-dotnet-latest` — verified the callback parameters and non-streaming/streaming overload semantics.
- `https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.aiagentbuilder?view=agent-framework-dotnet-latest` — verified `AIAgentBuilder` ownership and middleware-related builder surface.

## Framework implementation

- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/AIAgentBuilder.cs` — verified builder construction, registration, reverse factory application, first-registered outermost ordering, and callback overload behavior.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/AnonymousDelegatingAIAgent.cs` — verified how run callbacks receive the inner agent, how missing streaming callbacks are adapted, and how callback results flow through the delegating agent.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/AgentExtensions.cs` — verified `AsBuilder()` creation around an existing `AIAgent`.

## Official sample

- `framework/agent-framework/dotnet/samples/02-agents/Agents/Agent_Step11_Middleware/Program.cs` — verified the supported `.AsBuilder().Use(runFunc, null).Build()` pattern and before/after request transformation at agent scope.

## Tests

- `framework/agent-framework/dotnet/tests/Microsoft.Agents.AI.UnitTests/AnonymousDelegatingAIAgentTests.cs` — verified nested before/after ordering, message and options propagation, original exception propagation, cancellation-token forwarding, recovery capability, and short-circuit behavior.
- `framework/agent-framework/dotnet/tests/Microsoft.Agents.AI.UnitTests/AIAgentBuilderTests.cs` — verified middleware registration order and built pipeline construction.
