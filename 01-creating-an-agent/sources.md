# Sources examined

Claims were verified against the checked-out Microsoft Agent Framework repository and official documentation. Repository paths below are relative to the framework repository root.

## Framework overview and capability map

- `README.md`
  - Verified the framework's purpose, supported languages, production focus, provider flexibility, middleware, workflows, observability, declarative agents, Agent Skills, hosting, and development tooling.
- `https://learn.microsoft.com/en-us/agent-framework/concepts/`
  - Verified Agents, Workflows, and Harness Agents as the framework's main concept areas.
- `https://learn.microsoft.com/en-us/agent-framework/agents/`
  - Verified the high-level agent capability areas: input/output, context and knowledge, execution and autonomy, and operations and trust.
- `https://learn.microsoft.com/en-us/agent-framework/concepts/workflows/`
  - Verified workflows as explicit execution paths made from executors, edges, events, and state, and that compatible workflows can be exposed as agents.
- `https://learn.microsoft.com/en-us/agent-framework/concepts/harness`
  - Verified the official Harness as an opinionated, batteries-included agent for long-running work.

## Agent implementation

- `dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs`
  - Verified the common agent run/session abstraction, `RunAsync` overloads, and conversion of string input into a user message.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientExtensions.cs`
  - Verified that `IChatClient.AsAIAgent(...)` constructs one `ChatClientAgent` with the supplied instructions and name.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs`
  - Verified that `ChatClientAgent` derives from `AIAgent`, prepares messages and options, calls `IChatClient.GetResponseAsync(...)`, and returns an `AgentResponse`.
- `dotnet/src/Microsoft.Agents.AI.Abstractions/AgentResponse.cs`
  - Verified the response abstraction and its `Text` property.
- `dotnet/src/Microsoft.Agents.AI.OpenAI/Extensions/OpenAIChatClientExtensions.cs`
  - Verified the OpenAI chat-client adapter path used by the example.
- `dotnet/src/Microsoft.Agents.AI.OpenAI/Microsoft.Agents.AI.OpenAI.csproj`
  - Verified that the Agent Framework OpenAI integration references the `Microsoft.Extensions.AI.OpenAI` adapter package used transitively by the lesson.

## Workflow and Harness relationships

- `dotnet/src/Microsoft.Agents.AI.Workflows/Workflow.cs`
  - Verified `Workflow` as an executable structure containing executor bindings and edges.
- `dotnet/src/Microsoft.Agents.AI.Workflows/WorkflowBuilder.cs`
  - Verified the .NET graph builder used to connect workflow executors.
- `dotnet/src/Microsoft.Agents.AI.Workflows/WorkflowHostingExtensions.cs`
  - Verified that a compatible `Workflow` can be exposed through `AIAgent` using `AsAIAgent()`.
- `dotnet/src/Microsoft.Agents.AI.Harness/HarnessAgent.cs`
  - Verified that `HarnessAgent` is a preconfigured `DelegatingAIAgent` wrapping a `ChatClientAgent`, and verified its high-level long-running-task capabilities.
- `dotnet/src/Microsoft.Agents.AI.Harness/HarnessAgentOptions.cs`
  - Verified that Harness capabilities can be configured or disabled individually.

## Official .NET sample

- `dotnet/samples/02-agents/AgentProviders/openai/Agent_With_OpenAIChatCompletion/Program.cs`
  - Verified the current pattern for creating an OpenAI chat-completion agent and calling `RunAsync`.

## Tests

- `dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgentTests.cs`
  - Verified instruction and identity propagation, chat-client invocation, and response handling.
- `dotnet/tests/Microsoft.Agents.AI.OpenAI.UnitTests/Extensions/OpenAIChatClientExtensionsTests.cs`
  - Verified the OpenAI adapter creates an agent with the requested instructions and name.

## OpenRouter compatibility

- `https://openrouter.ai/docs/quickstart`
  - Verified the OpenAI-compatible base URL, API-key authentication pattern, and OpenRouter model identifiers.
