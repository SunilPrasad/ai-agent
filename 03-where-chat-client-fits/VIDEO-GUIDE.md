# Video 03 recording guide — Where the chat client fits

## Learning outcome

The viewer can name the job and owner of `ChatClientAgent`, `IChatClient`, the OpenAI SDK chat client, and OpenRouter.

Prerequisites: Videos 01–02 and basic C# interfaces. Target length: **9:15**.

| Time | Section |
|---|---|
| 0:00–0:40 | Hook: four similar names |
| 0:40–1:30 | The confusion this boundary solves |
| 1:30–3:05 | Walk through the ownership diagram |
| 3:05–6:35 | Build the four variables in code |
| 6:35–7:25 | Run and inspect runtime types |
| 7:25–8:30 | Follow one request through the layers |
| 8:30–9:15 | Recap and Video 04 transition |

## Recording flow

At 0:00, show the four type names in `Program.cs`. Ask: “Which one is the agent, which one calls the network, and which one is only an interface?”

At 0:40, explain that unclear ownership causes configuration and dependencies to leak into the wrong parts of an application.

At 1:30, show the README ownership diagram. Move left to right and give each box one sentence. Emphasize that `AsIChatClient()` is the adapter and OpenRouter is outside all .NET libraries.

At 3:05, open `Example/Program.cs` and explain:

1. Environment variables provide configuration.
2. `OpenAIClient` stores endpoint and credential configuration.
3. `GetChatClient(model)` creates `OpenAI.Chat.ChatClient`.
4. `AsIChatClient()` exposes the common Microsoft.Extensions.AI interface.
5. `AsAIAgent()` creates Agent Framework's `ChatClientAgent`.

At 6:35, run:

```powershell
dotnet run --project Example/Example.csproj
```

Expected shape:

```text
1. SDK client: ChatClient
2. Common client: <adapter runtime type>
3. Agent: ChatClientAgent
4. Answer: <model output>
```

Say that adapter implementation names are not an API to memorize.

At 7:25, show the sequence diagram and follow `RunAsync` to `IChatClient.GetResponseAsync`, the SDK, OpenRouter, and back as `AgentResponse`.

At 8:30, recap the four responsibilities. Transition: “Next we leave this plumbing unchanged and change only the instructions.”

## Likely questions

- **Is `IChatClient` part of Agent Framework?** No. It comes from Microsoft.Extensions.AI, which Agent Framework uses.
- **Is `OpenAIClient` the agent?** No. It is the provider SDK root client.
- **Does using the OpenAI SDK mean calling OpenAI's service?** Not here. The endpoint is explicitly configured as OpenRouter.
- **Why not call `IChatClient` directly?** You can for simple chat calls. `ChatClientAgent` adds the common agent run/session surface and agent pipeline.
