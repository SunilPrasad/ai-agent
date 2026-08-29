# Video guide — Meet Microsoft Agent Framework and create your first agent

## Learning outcome

By the end of the video, the viewer can explain what Microsoft Agent Framework is, distinguish an Agent from a Workflow and a Harness Agent at a high level, and create one OpenRouter-backed `AIAgent` while understanding every important code block.

## Prerequisites

- Comfortable reading basic C# and `async`/`await`
- .NET 10 SDK installed
- An OpenRouter API key and model identifier
- No previous Agent Framework knowledge required

## Target length

**9 minutes 50 seconds**

| Time | Section | Purpose |
|---|---|---|
| 0:00–0:35 | Hook | Show the final result and promise the framework map first |
| 0:35–1:50 | What the framework is | Separate the framework, model provider, and model SDK |
| 1:50–2:55 | Capability map | Introduce Agents, Workflows, Harness Agents, and the future roadmap |
| 2:55–3:30 | Today's boundary | Locate the simple `ChatClientAgent` and name what is deliberately excluded |
| 3:30–4:05 | Object ownership | Explain the provider SDK, `IChatClient`, Agent Framework, and OpenRouter boundary |
| 4:05–7:15 | Code walkthrough | Explain every important code block in `Program.cs` |
| 7:15–8:00 | Run the example | Execute it and connect the output back to the diagram |
| 8:00–9:05 | Internal flow | Separate construction from the `RunAsync` request path |
| 9:05–9:50 | Recap and next lesson | Repeat the framework map and introduce `AIAgent` |

The capability overview is a map, not a set of mini-tutorials. Do not explain tools, memory, RAG, MCP, workflows, multi-agent communication, or Harness internals beyond one-sentence definitions.

## Before recording

1. Open `README.md` with Mermaid preview enabled.
2. Open `Example/Program.cs`.
3. Open a terminal in `01-creating-an-agent`.
4. Set `OPENROUTER_API_KEY` and `OPENROUTER_MODEL` before screen capture.
5. Clear terminal history so credentials cannot appear.
6. Run `dotnet build Example/Example.csproj`.
7. Run the example once to confirm the model is available.

## Recording flow

### 0:00–0:35 — Hook

Show the final program and one successful output.

Suggested opening:

> This small C# program creates a real Microsoft Agent Framework agent. Before we explain the code, let us build the map: what the framework is, what it can do, and exactly where this tiny example fits.

### 0:35–1:50 — What Microsoft Agent Framework is

Show the opening of `README.md`.

Explain three boundaries:

1. The model generates text or decisions.
2. The model SDK communicates with the provider endpoint.
3. Microsoft Agent Framework structures the application around that model access.

Use the phrase **framework around the model, not the model itself**.

Then explain why the framework becomes useful as an application adds state, tools, knowledge, orchestration, and production concerns.

### 1:50–2:55 — Capability map

Show **The framework map** diagram.

Define each item in one sentence:

- Agent: one callable AI capability through `AIAgent`.
- Workflow: an explicit path of code or agent steps.
- Harness Agent: a specialized, batteries-included `AIAgent` for longer interactive work.

Do not read every roadmap row. Mention only that later videos cover tools and MCP, memory and RAG, multi-agent orchestration, observability, and the official Harness Agent.

Do not explain how any of those capabilities work yet.

### 2:55–3:30 — Set today's boundary

Show **Where today's example fits**.

Say clearly:

> Today is one model connection, one instruction, one message, and one response. It is the first building block—not an autonomous system and not the whole framework.

Point to `ChatClientAgent` under the Agent branch of the diagram.

### 3:30–4:05 — Explain ownership

Show **The objects in this example**.

Follow the construction path:

```text
OpenAI SDK → IChatClient → ChatClientAgent/AIAgent
```

Keep OpenRouter visibly outside the framework boundary. Explain that one runtime `ChatClientAgent` object is referenced through the `AIAgent` base type.

### 4:05–7:15 — Walk through the code

Open `Example/Program.cs` and explain these blocks.

#### Namespaces

Use them to repeat ownership, not to teach basic `using` syntax.

#### Environment variables

Explain why secrets and the model identifier are configuration. Do not show their values.

#### `OpenAIClient`

Explain that this is the OpenAI SDK's root client and that the custom endpoint directs compatible requests to OpenRouter. Creating it is local.

#### `IChatClient`

Explain that `GetChatClient(model)` creates the model-specific provider client, then `AsIChatClient()` exposes the common Microsoft.Extensions.AI interface.

#### `AIAgent`

Explain that `AsAIAgent()` returns a concrete `ChatClientAgent`. The `AIAgent` variable shows the application-facing abstraction. Read the short instructions and name aloud.

#### `RunAsync`

Mark this as the point where external I/O begins. Explain user message, instructions, chat-client call, and asynchronous waiting.

#### `AgentResponse.Text`

Explain that richer content exists but is deliberately excluded from Video 01. Remind viewers that model output varies and is untrusted.

### 7:15–8:00 — Run the example

Run:

```powershell
dotnet run --project Example/Example.csproj
```

Read the output and trace it backwards through the ownership diagram. Do not debug provider configuration on camera; pause and fix it off camera if necessary.

### 8:00–9:05 — Explain the internal flow

Show **Construction flow versus request flow**.

Emphasize:

- Constructors and adapters configure local objects.
- `RunAsync` converts text into a user message.
- `ChatClientAgent` adds instructions and calls `IChatClient`.
- The framework returns `AgentResponse`.

Do not open source files in this introductory recording. `sources.md` lets interested viewers inspect them afterward.

### 9:05–9:50 — Recap and transition

Ask the viewer to remember:

1. Agent Framework is around the model, not the model.
2. Agents, Workflows, and Harness Agents solve different problems.
3. Today's runtime object is one `ChatClientAgent` referenced as `AIAgent`.
4. `IChatClient` is the provider-neutral boundary.
5. `RunAsync` starts the request.

Close with:

> We used the `AIAgent` type, but we have not yet explained why it is the common abstraction. That single question is Video 02.

## Likely beginner questions

### Is Microsoft Agent Framework another AI model?

No. It is an application framework that works around model clients and agent services.

### Is `OpenAIClient` part of Agent Framework?

No. It comes from the OpenAI .NET SDK. The adapter exposes its model-specific chat client as `IChatClient`.

### Are we calling OpenAI directly?

No. The custom endpoint directs the compatible SDK request to OpenRouter.

### Is `AIAgent` a second object wrapping `ChatClientAgent`?

No. The one runtime object is a `ChatClientAgent`; `AIAgent` is its base type and the variable's declared type.

### Is this agent autonomous?

No. It performs one call. Tools, loops, planning, memory, and Harness behavior are later topics.

### Why not call `IChatClient` directly?

That can be enough for a tiny stateless request. `AIAgent` gives application code the common Agent Framework run and session abstraction and a path to further capabilities.
