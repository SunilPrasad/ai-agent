# Video 06 recording guide — Stream an agent response

## Learning outcome

The viewer can use `RunStreamingAsync` with `await foreach` and explain what one `AgentResponseUpdate` represents.

Prerequisites: Videos 01–05 and basic `async`/`await`. Target length: **9:25**.

| Time | Section |
|---|---|
| 0:00–0:35 | Hook: blank screen versus growing answer |
| 0:35–1:25 | Complete response versus updates |
| 1:25–2:35 | Package/conveyor-belt diagram |
| 2:35–5:45 | Walk through `await foreach` |
| 5:45–6:50 | Run and watch output arrive |
| 6:50–8:20 | Follow the internal streaming path |
| 8:20–9:25 | When to stream, cautions, recap |

## Recording flow

Open by describing a long answer with no visible progress. State the outcome: show each available update immediately.

Use the README comparison diagram. Keep one exact distinction on screen:

```text
RunAsync          -> Task<AgentResponse>
RunStreamingAsync -> IAsyncEnumerable<AgentResponseUpdate>
```

Open `Example/Program.cs`. Compress setup and explain:

- `Console.Write` keeps chunks on one line.
- `RunStreamingAsync` returns a lazy asynchronous sequence.
- `await foreach` begins and drives the request by asynchronously requesting each next update.
- `update.Text` is the text in this update, not necessarily one token or word.
- Code after the loop runs when the stream is complete.

Run:

```powershell
dotnet run --project Example/Example.csproj
```

Pause so the viewer can see the answer grow. Point out that the update count is not stable across models or runs and includes updates whose `Text` may be empty.

For internals, show:

1. `AIAgent.RunStreamingAsync(string)` creating a user `ChatMessage`.
2. The abstract `RunCoreStreamingAsync` handoff.
3. `ChatClientAgent` calling `IChatClient.GetStreamingResponseAsync`.
4. Each `ChatResponseUpdate` becoming `AgentResponseUpdate`.

Do not teach cancellation, sessions, tool-call streaming, or aggregation here.

End with when streaming helps and when a complete response is simpler. Transition to agent sessions.

## Likely questions

- **Is one update one token?** No.
- **Does streaming make generation finish faster?** Not necessarily; it exposes partial output earlier.
- **Can `update.Text` be empty?** Yes, an update may carry other information.
- **Can I stop the loop?** Yes, but cancellation and early termination deserve production-focused handling later.
- **Do I need a session?** Not for this single-turn example.
