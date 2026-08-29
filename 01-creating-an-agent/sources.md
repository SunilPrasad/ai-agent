# Sources examined

All repository paths below are relative to the upstream Microsoft Agent Framework repository root. Framework behavior was verified against checked-out source rather than assumed from pretrained API knowledge.

## Framework implementation

- `dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs`
  - Verified the `AIAgent` abstraction, identity properties, `RunAsync` overloads, string-to-user-message conversion, and delegation to `RunCoreAsync`.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientExtensions.cs`
  - Verified both `IChatClient.AsAIAgent` overloads and that they construct `ChatClientAgent`.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs`
  - Verified constructor behavior, option cloning, default middleware installation, history-provider setup, option merging, session/message preparation, the `IChatClient.GetResponseAsync` call, and conversion to `AgentResponse`.
- `dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgentOptions.cs`
  - Verified the configuration object used for agent identity and default `ChatOptions`.
- `dotnet/src/Microsoft.Agents.AI.Abstractions/AgentResponse.cs`
  - Verified response wrapping and the `Text`, `Messages`, and `ToString` behavior.
- `dotnet/src/Microsoft.Agents.AI.Abstractions/InMemoryChatHistoryProvider.cs`
  - Verified the default local history provider used by `ChatClientAgent`.
- `dotnet/src/Microsoft.Agents.AI.OpenAI/Extensions/OpenAIChatClientExtensions.cs`
  - Verified the current OpenAI `ChatClient` adapter and its use of `AsIChatClient` and `ChatClientAgent`.

## Official samples

- `dotnet/samples/02-agents/AgentProviders/openai/Agent_With_OpenAIChatCompletion/Program.cs`
  - Verified the current OpenAI Chat Completions agent construction and `RunAsync` pattern.
- `dotnet/samples/02-agents/AgentProviders/openai/Agent_With_OpenAIChatCompletion/Agent_With_OpenAIChatCompletion.csproj`
  - Verified the source project reference required for the OpenAI client adapter.
- `dotnet/samples/05-end-to-end/AGUIClientServer/AGUIServer/Program.cs`
  - Verified construction of an `OpenAIClient` with a custom `OpenAIClientOptions.Endpoint` and conversion to `IChatClient`.
- `dotnet/samples/04-hosting/af-hosting/local_responses_workflow/Client/Program.cs`
  - Examined another official custom-endpoint example using `ApiKeyCredential`, `OpenAIClientOptions`, and Agent Framework.

## Tests

- `dotnet/tests/Microsoft.Agents.AI.UnitTests/ChatClient/ChatClientAgentTests.cs`
  - Verified construction metadata, middleware wrapping, invocation of `IChatClient.GetResponseAsync`, instruction propagation, and assistant-response handling.
- `dotnet/tests/Microsoft.Agents.AI.OpenAI.UnitTests/Extensions/OpenAIChatClientExtensionsTests.cs`
  - Verified the OpenAI chat-client adapter produces a `ChatClientAgent` with the requested instructions, name, and description.

## OpenRouter compatibility reference

- `https://openrouter.ai/docs/quickstart`
  - Verified that OpenRouter supports the OpenAI SDK compatibility path, uses `https://openrouter.ai/api/v1` as the base URL, accepts bearer API keys, and expects OpenRouter model identifiers.
