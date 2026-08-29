# Video 02 recording guide — What `AIAgent` represents

## Learning outcome

By the end, the viewer can explain why application code accepts `AIAgent` even though this example creates a `ChatClientAgent` at runtime.

## Prerequisites

- Video 01: create and run a basic agent.
- Basic C# inheritance, method parameters, and `async`/`await`.
- `OPENROUTER_API_KEY` and `OPENROUTER_MODEL` already configured off-screen.

## Target length

9 minutes 20 seconds. Hard stop: 12 minutes.

| Time | Section | What to show |
|---|---|---|
| 0:00–0:35 | Hook | `PrintAnswerAsync(AIAgent agent, ...)` |
| 0:35–1:20 | The coupling problem | The hypothetical `ChatClientAgent` parameter |
| 1:20–2:25 | Define `AIAgent` | One-sentence idea and `Stream` analogy |
| 2:25–3:25 | Base and runtime types | README class diagram |
| 3:25–4:10 | Ownership boundary | README ownership diagram |
| 4:10–6:55 | Walk through the code | `Example/Program.cs` in five small sections |
| 6:55–7:40 | Run it | Command and three output lines |
| 7:40–8:40 | Framework internals | README sequence diagram and `RunCoreAsync` handoff |
| 8:40–9:20 | Recap and next step | Four-sentence recap; bridge to Video 03 |

## 0:00–0:35 — Hook

Show the helper signature first:

```csharp
static async Task PrintAnswerAsync(AIAgent agent, string question)
```

Talking point: “This method can run our agent without knowing about OpenRouter, the OpenAI SDK, or even `ChatClientAgent`. The reason is one framework type: `AIAgent`.”

## 0:35–1:20 — State the problem

Temporarily show this hypothetical signature in the README, not by editing the program:

```csharp
static Task PrintAnswerAsync(ChatClientAgent agent, string question)
```

Explain that it ties application code to one agent implementation. Do not claim that concrete types are bad; say they are too specific when a method needs only common agent behavior.

## 1:20–2:25 — Define the concept

Use the one-sentence idea from the README.

Say explicitly:

- `AIAgent` is an abstract C# base class, not an interface.
- It represents the operations common to Agent Framework agents.
- It is not the model, provider service, or chat client.

Use `Stream` / `FileStream` as the familiar .NET analogy. Keep the analogy under 30 seconds.

## 2:25–3:25 — Explain the type diagram

Show the README “Base class and runtime object” diagram.

Point to `AIAgent`, then `ChatClientAgent`. Mention `DelegatingAIAgent`, `HarnessAgent`, and `A2AAgent` only as proof that the base class covers different implementations. Do not explain harnesses, delegation, or A2A yet.

Key sentence: “Sharing a base class does not make their internals identical; it gives callers one stable set of operations.”

## 3:25–4:10 — Explain ownership

Show the README ownership diagram from left to right. Spend most time on the application-to-`AIAgent` boundary.

State that the OpenAI SDK and OpenRouter setup is reused only to make the example runnable. Promise to untangle that side in Video 03.

## 4:10–6:55 — Walk through `Example/Program.cs`

Open `Example/Program.cs`.

### Configuration

Show lines that read `OPENROUTER_API_KEY` and `OPENROUTER_MODEL`. Explain that secrets stay outside source and recording. Do not show their values.

### Chat-client setup

Collapse this into one explanation: it creates an OpenAI-compatible client, points it to OpenRouter, chooses a model, and exposes `IChatClient`. Avoid teaching this boundary today.

### The central line

Pause on:

```csharp
AIAgent agent = chatClient.AsAIAgent(...);
```

Explain left and right separately:

- Left: the base type our code chooses to depend on.
- Right: `AsAIAgent` creates a concrete `ChatClientAgent`.

### Declared and runtime types

Show the two `Console.WriteLine` calls. Explain that the declaration establishes the compile-time type, `nameof(AIAgent)` merely prints that source symbol's name, and `GetType()` inspects the runtime object.

### Application boundary

Show the call and `PrintAnswerAsync`. Point out every concrete/provider type that the helper does not need to know. Explain `AgentResponse.Text`.

## 6:55–7:40 — Run the example

From the lesson directory, run:

```powershell
dotnet run --project Example/Example.csproj
```

Expected shape:

```text
Declared type in source: AIAgent
Runtime type: ChatClientAgent
DotNetTeacher: <one short model-generated sentence>
```

Say that the first two lines prove the type relationship; the final wording can vary because it comes from a model.

## 7:40–8:40 — Show the internal handoff

Return to the README sequence diagram.

Explain only this path:

1. `RunAsync(string)` creates a user `ChatMessage`.
2. The base class delegates through abstract `RunCoreAsync`.
3. The runtime `ChatClientAgent` implementation calls its `IChatClient`.
4. The caller receives `AgentResponse`.

If showing source, open these exact members:

- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI.Abstractions/AIAgent.cs` — `RunAsync(string)` and the collection overload.
- `framework/agent-framework/dotnet/src/Microsoft.Agents.AI/ChatClient/ChatClientAgent.cs` — `RunCoreAsync`.

Do not expand into sessions, middleware, tools, or streaming.

## 8:40–9:20 — Recap and transition

Recap:

1. `AIAgent` is the common abstract base class.
2. Our object is a `ChatClientAgent` at runtime.
3. The helper depends only on common agent behavior.
4. Base `RunAsync` calls `RunCoreAsync`, and virtual dispatch selects the `ChatClientAgent` override.

Transition: “Now that the application-facing agent boundary is clear, Video 03 will explain what sits behind `ChatClientAgent`: `IChatClient`, the OpenAI SDK, and OpenRouter.”

## Likely beginner questions

**Is `AIAgent` an interface?**  
No. In C# it is an abstract class.

**Did assigning it to `AIAgent` convert the object?**  
No. The runtime object remains a `ChatClientAgent`.

**Can I write `new AIAgent()`?**  
No. Abstract classes must be implemented by a concrete derived class.

**Does every `AIAgent` use `IChatClient`?**  
No. This lesson's `ChatClientAgent` does. Other derived agents can perform their core work differently.

**Why not explain sessions and streaming now?**  
They are part of the base abstraction, but each deserves a focused later video.
