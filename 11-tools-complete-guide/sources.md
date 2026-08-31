# Sources

Framework commit examined: `edfe115ea06bca57ae5a123d0fac5b3fdda13603`

## Official documentation

- `https://learn.microsoft.com/en-us/agent-framework/agents/tools/function-tools` — verified the supported C# pattern using `AIFunctionFactory.Create`, `[Description]`, and the `tools` argument on a chat-client agent.
- `https://learn.microsoft.com/en-us/agent-framework/agents/tools/` — verified Microsoft Agent Framework's tool categories and the documented `AsAIFunction()` agent-as-tool pattern.
- `https://learn.microsoft.com/en-us/agent-framework/journey/agents-as-tools` — verified the outer-agent/inner-agent mental model and that inner context is isolated unless passed through tool arguments.
- `https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.aiagentextensions.asaifunction?view=agent-framework-dotnet-latest` — verified the optional session contract: without a supplied session, each inner-agent function call gets a new session.
- `https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.ai.aifunctionfactory?view=net-10.0-pp` — verified that `AIFunctionFactory` wraps .NET methods and derives input and result JSON schemas.

## Framework implementation

- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/AgentExtensions.cs` — verified that `AsAIFunction()` creates a query-taking `AIFunction`, runs the inner agent, returns its response text, and forwards its optional session argument.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs` — verified the public run/session boundary used when an agent function supplies no reusable session.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientExtensions.cs` — verified that the Agent Framework chat-client pipeline installs/configures `FunctionInvokingChatClient` and supplies registered tools.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs` — verified how agent tools are copied into request chat options, participate in the run, and how a missing session is resolved for the chat-client agent path.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgentOptions.cs` — verified tool and function-invocation configuration ownership.

## Official samples

- `framework/agent-framework/dotnet/samples/01-get-started/02_add_tools/Program.cs` — verified the minimal C# method → `AIFunctionFactory.Create` → agent tools → `RunAsync` pattern.
- `framework/agent-framework/dotnet/samples/02-agents/Agents/Agent_Step09_AsFunctionTool/Program.cs` — verified a specialist agent exposed to a main agent through `AsAIFunction()`.

## Tests

- `framework/agent-framework/dotnet/tests/Microsoft.Agents.AI.UnitTests/AgentExtensionsTests.cs` — verified inferred agent-tool metadata, query forwarding, response-text return, cancellation, and options.
- `framework/agent-framework/dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientExtensionsTests.cs` — verified `FunctionInvokingChatClient` configuration in the agent chat-client pipeline.
