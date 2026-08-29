# Video guide — Create your first Microsoft Agent Framework agent

## Learning outcome

By the end of the video, the viewer can create one OpenRouter-backed `AIAgent`, run it once, and explain the responsibility of every important object in the program.

## Prerequisites

- Comfortable reading basic C# and `async`/`await`
- .NET 10 SDK installed
- An OpenRouter API key and model identifier
- No previous Agent Framework knowledge required

## Target length

**9 minutes 30 seconds**

| Time | Section | Purpose |
|---|---|---|
| 0:00–0:40 | Hook | Show that the complete agent is a small console program |
| 0:40–1:25 | Problem | Explain model-client code versus an application-facing agent |
| 1:25–2:35 | Diagram | Walk from application to agent, chat client, OpenRouter, and back |
| 2:35–3:05 | Configuration | Explain the two environment variables without showing secrets |
| 3:05–6:55 | Code | Explain the program one block at a time |
| 6:55–7:55 | Run | Execute the program and read the result |
| 7:55–8:50 | Inside the framework | Explain `ChatClientAgent` and the internal request flow |
| 8:50–9:30 | Recap and next step | Repeat the five key pieces and introduce `AIAgent` |

Do not add tools, sessions, streaming, memory, RAG, MCP, workflows, or harness agents to this video. They each deserve a separate lesson.

## Before recording

1. Open `README.md` with Mermaid preview available.
2. Open `Example/Program.cs` in the editor.
3. Open a terminal in `01-creating-an-agent`.
4. Set `OPENROUTER_API_KEY` and `OPENROUTER_MODEL` before screen capture begins.
5. Clear terminal history so the API key cannot appear in the recording.
6. Run `dotnet build Example/Example.csproj` once.
7. Run the example once to confirm the selected model is available.

## Recording flow

### 0:00–0:40 — Hook

Show the final console output first.

Suggested talking point:

> We are going to create a real Microsoft Agent Framework agent in C#. By the end, every important line in this program will make sense.

Do not define every term yet. Give the viewer a reason to continue.

### 0:40–1:25 — State the problem

Explain:

- OpenRouter gives us access to models.
- The provider client knows how to send model requests.
- Our application wants an agent with a clear job and a simple run API.
- Agent Framework connects those two needs.

Use the telephone mental model: the chat client is the connection; the agent is the worker using it.

### 1:25–2:35 — Explain the diagram

Show **The complete flow** in `README.md`.

Follow one direction only at first:

1. C# calls `AIAgent.RunAsync`.
2. The concrete `ChatClientAgent` uses `IChatClient`.
3. The request travels through OpenRouter to the selected model.
4. The result returns as `AgentResponse`.

Then show the ownership diagram briefly. Emphasize that `OpenAIClient` is not inside Agent Framework and OpenRouter is an external service.

### 2:35–3:05 — Configuration

Show only the environment-variable names:

```text
OPENROUTER_API_KEY
OPENROUTER_MODEL
```

Explain that secrets stay outside source code. Do not display the environment-variable values.

### 3:05–6:55 — Walk through the code

Open `Example/Program.cs` and explain it in this order.

#### Namespaces

Point out that the namespaces reveal the boundaries:

- `OpenAI` is the provider SDK.
- `Microsoft.Extensions.AI` supplies the common chat abstraction.
- `Microsoft.Agents.AI` supplies the agent abstraction.

#### Environment variables

Explain that the null-coalescing throw gives a clear startup error. Avoid turning this into a configuration-management lesson.

#### `OpenAIClient`

Explain both constructor arguments:

- `ApiKeyCredential` authenticates the request.
- `Endpoint` changes the destination to OpenRouter.

Repeat: this is a provider client, not the agent.

#### `IChatClient`

Explain that `GetChatClient(model)` selects the model and `AsIChatClient()` creates the common interface expected by Agent Framework.

#### `AIAgent`

Explain that `AsAIAgent()` creates a `ChatClientAgent`. Read the instructions aloud and show how they define one small job.

#### `RunAsync`

Explain that this is the line that contacts the model. The earlier lines only built and connected objects.

#### `AgentResponse`

Show `response.Text`. Mention that richer response content exists, but it is outside this video's single concept.

### 6:55–7:55 — Run the example

Run:

```powershell
dotnet run --project Example/Example.csproj
```

Read the result, then connect it back to the diagram. Mention that exact wording varies because the response is generated.

If the run fails, do not debug credentials during the recording. Pause and fix the environment or model identifier off camera.

### 7:55–8:50 — What happened internally?

Return to the flow diagram and explain:

1. `AsAIAgent()` created `ChatClientAgent`.
2. `RunAsync` converted the string into a user message.
3. The agent combined the user message with its instructions.
4. It called `IChatClient`.
5. It returned an `AgentResponse`.

Keep this at mental-model depth. Do not open framework source during the first video.

### 8:50–9:30 — Recap and transition

Recap with five short phrases:

1. OpenRouter hosts the model connection.
2. `OpenAIClient` talks to the compatible endpoint.
3. `IChatClient` is the common boundary.
4. `AIAgent` is what the application uses.
5. `RunAsync` performs the work.

Close by introducing Video 02:

> We created an `AIAgent`, but why is it an abstraction instead of just a concrete class? That is the one question we will answer next.

## Likely beginner questions

### Is `OpenAIClient` part of Microsoft Agent Framework?

No. It comes from the OpenAI .NET SDK. Agent Framework receives the adapted `IChatClient`.

### Are we calling OpenAI directly?

No. The custom endpoint points the compatible SDK at OpenRouter.

### Why not call `IChatClient` directly?

That can be enough for a tiny chat request. `AIAgent` gives the application an agent abstraction that can later participate in the rest of Agent Framework.

### Does creating the agent spend tokens?

No. The model request starts when `RunAsync` is called.

### Why can the output be different each time?

The model generates the response. We are verifying the code path, not a fixed sentence.
