# Video 05 recording guide — Messages and responses

## Learning outcome

The viewer can create a user `ChatMessage` and choose between `AgentResponse.Messages` and `AgentResponse.Text`.

Prerequisites: Videos 01–04. Target length: **9:05**.

| Time | Section |
|---|---|
| 0:00–0:35 | Hook: the hidden role in a string prompt |
| 0:35–1:30 | Why messages have structure |
| 1:30–2:35 | Envelope and reply-bundle diagram |
| 2:35–5:50 | Walk through explicit input and output |
| 5:50–6:45 | Run and inspect output |
| 6:45–8:10 | Framework conversion path |
| 8:10–9:05 | Choosing Text or Messages; recap |

## Recording flow

Begin by showing `RunAsync("...")` from earlier videos, then show `new ChatMessage(ChatRole.User, ...)`. Explain that the string overload hides a useful default.

Use the README diagram to define:

- One message: role plus content.
- One response: a list of messages plus metadata.
- `Text`: a convenience view, not the whole response model.

Open `Example/Program.cs`. Compress provider and instruction setup. Explain every line from `ChatMessage request` through the final output.

Run:

```powershell
dotnet run --project Example/Example.csproj
```

Point out the sent `User` role, returned assistant message, and repeated combined text. Say that simple responses usually contain one message, but code should not make that a universal assumption.

For internals, show `AIAgent.RunAsync(ChatMessage)` delegating to the collection overload, then `ChatClientAgent.RunCoreAsync` creating `AgentResponse` from `ChatResponse`.

End with the decision:

- Need display text only? Use `Text`.
- Need roles or structure? Use `Messages`.

Transition: “Next the response arrives piece by piece instead of as one completed object.”

## Likely questions

- **Where is `ChatMessage` defined?** Microsoft.Extensions.AI.
- **Why isn't it an Agent Framework type?** Agent Framework shares the Microsoft.Extensions.AI message model with `IChatClient`.
- **Can a response contain more than one message?** Yes.
- **Does `Text` include non-text content?** No.
- **Should I trust the returned text?** No; validate it for the application's use.
