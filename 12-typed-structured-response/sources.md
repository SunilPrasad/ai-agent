# Sources

Framework commit examined: `edfe115ea06bca57ae5a123d0fac5b3fdda13603`

## Official documentation

- `https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.aiagent.runasync?view=agent-framework-dotnet-latest` — verified the generic agent run overload that returns `AgentResponse<T>`.
- `https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.agentresponse-1?view=agent-framework-dotnet-latest` — verified the generic response type used for structured output.
- `https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.agentresponse-1.result?view=agent-framework-dotnet-latest` — verified the typed `Result` property.
- `https://learn.microsoft.com/en-us/dotnet/api/microsoft.agents.ai.agentresponse.text?view=agent-framework-dotnet-latest` — verified that the ordinary response exposes concatenated response text.

## Framework implementation

- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgentStructuredOutput.cs` — verified that `RunAsync<T>` derives a JSON-schema response format, applies it through run options, invokes the normal agent path, and returns `AgentResponse<T>`.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AgentResponse{T}.cs` — verified that `Result` reads response text, handles the framework's wrapper when needed, and deserializes the first top-level JSON object with `System.Text.Json`.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AgentResponse.cs` — verified the non-generic response and its `Text` surface used for the free-form comparison.

## Official samples

- `framework/agent-framework/dotnet/samples/02-agents/Agents/Agent_Step02_StructuredOutput/Program.cs` — verified the supported C# pattern: define an application type, call `RunAsync<T>`, then retrieve `.Result`.

## Tests

- `framework/agent-framework/dotnet/tests/AgentConformance.IntegrationTests/StructuredOutputRunTests.cs` — verified conformance coverage for `RunAsync<CityInfo>` and `AgentResponse<CityInfo>.Result`.
- `framework/agent-framework/dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgent_StructuredOutput_WithRunAsyncTests.cs` — verified that a generic run sets a JSON-schema response format and deserializes the result.
