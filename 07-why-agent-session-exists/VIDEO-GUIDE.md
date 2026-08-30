# Video guide

## Video title

Why `AgentSession` Exists in Microsoft Agent Framework

## Single learning outcome

Explain why the reusable `AIAgent` and one conversation's `AgentSession` are separate objects.

## Prerequisites

Sessions 1–6; .NET 10; `OPENROUTER_API_KEY` and `OPENROUTER_MODEL` configured.

## Target length: 8:00

| Time | Section |
|---|---|
| 0:00–0:45 | Hook: one agent, many conversations |
| 0:45–2:00 | Agent definition versus conversation state |
| 2:00–3:15 | Diagram and ownership boundaries |
| 3:15–6:30 | Code walkthrough |
| 6:30–7:20 | Run and inspect output |
| 7:20–8:00 | Recap and next lesson |

## Opening hook and problem

Ask: “If one `TripPlanner` serves a thousand users, where should each user's conversation live?” Explain that putting all state on one shared agent would mix conversations.

## Recording order

1. Show the README's employee-and-case-file analogy.
2. Trace application → agent + session → chat client → OpenRouter in the Mermaid diagram.
3. Open `Example/Program.cs` and briefly revisit configuration/client creation.
4. Pause on `AIAgent agent` versus `AgentSession session`.
5. Show that `RunAsync` receives both prompt and session.
6. Run `dotnet run --project lessons/07-why-agent-session-exists/Example` and identify the concrete session type.

## Talking points

- `AIAgent` comes from Microsoft Agent Framework and describes reusable behavior.
- `AgentSession` also comes from the framework, but belongs to one conversation.
- The agent creates its own compatible session; application code should not construct an implementation-specific session.
- The OpenAI SDK is only the OpenRouter-compatible transport in this example.
- A session may include history, identifiers, provider state, or other per-conversation data.

## Likely questions

- **Does creating a session call the model?** For this chat-client agent, it creates a local session object; the model call occurs at `RunAsync`.
- **Is a session permanent?** In this chat-client example it is local until the application serializes and stores it. Other implementations may keep a reference to service-stored conversation state.
- **Can I reuse it with another agent?** Do not assume so; agents may attach different behaviors and expect different session types.

## Recap

One reusable agent can serve many conversations because each conversation has its own session.

## Transition

Next, reuse one session and watch a follow-up prompt inherit the first turn's context.
