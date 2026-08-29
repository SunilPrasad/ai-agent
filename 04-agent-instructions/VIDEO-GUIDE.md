# Video 04 recording guide — Give an agent instructions

## Learning outcome

The viewer can separate stable agent instructions from the current user message and explain how instructions reach `IChatClient`.

Prerequisites: Videos 01–03. Target length: **8:50**.

| Time | Section |
|---|---|
| 0:00–0:35 | Hook: same question, different answers |
| 0:35–1:25 | Stable rule versus current request |
| 1:25–2:35 | Job-description mental model |
| 2:35–5:45 | Walk through the two-agent code |
| 5:45–6:45 | Run and compare output |
| 6:45–7:55 | Trace instructions inside the framework |
| 7:55–8:50 | Use, caution, recap, next video |

## Recording flow

Open with the expected output. Ask why one model and one question produce different shapes.

Show the README diagram. Define instructions before using the term repeatedly: stable guidance applied to each run. Contrast it with the user message, which carries today's task.

Open `Example/Program.cs`. Compress the OpenRouter setup to one reminder from Video 03. Spend time on:

- The two `AsAIAgent` calls.
- The exact instruction difference.
- The shared `Question` constant.
- The two `RunAsync` calls.

Run:

```powershell
dotnet run --project Example/Example.csproj
```

Point out response shape, not exact wording. Mention that model output can vary and instructions are guidance rather than enforcement.

For internals, show:

- `ChatClientAgent` constructor placing `instructions` in `ChatOptions.Instructions`.
- `RunCoreAsync` passing effective `ChatOptions` to `IChatClient.GetResponseAsync`.

Do not teach per-run overrides, prompt engineering, tools, or safety systems in this video.

Recap: stable instructions, current message, options reach client, validate important output. Transition to explicit messages and responses.

## Likely questions

- **Are instructions the same as the user prompt?** No. They serve different roles and lifetimes.
- **Are instructions guaranteed?** No. They guide model behavior.
- **Why create two agents?** Only to make one variable—the instructions—easy to compare.
- **Should secrets go in instructions?** No.
