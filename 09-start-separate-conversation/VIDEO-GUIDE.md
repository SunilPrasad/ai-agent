# Video guide

## Video title

Start Separate Agent Conversations Safely in C#

## Single learning outcome

Use one `AIAgent` with two `AgentSession` instances to keep unrelated conversations isolated.

## Prerequisites

Sessions 1–8; .NET 10; OpenRouter environment variables configured.

## Target length: 8:30

| Time | Section |
|---|---|
| 0:00–0:55 | Hook: Paris must not become Oslo |
| 0:55–2:10 | What can be shared and what cannot |
| 2:10–3:15 | Two-lane diagram |
| 3:15–6:35 | Code walkthrough |
| 6:35–7:45 | Run and compare factual isolation |
| 7:45–8:30 | Production caution and recap |

## Opening hook

Show the two expected answers. Ask what would happen if both initial messages were written to one conversation.

## Recording order

1. Contrast shared agent behavior with isolated state.
2. Explain the two-session diagram.
3. Open `Example/Program.cs` and identify one agent, two session variables, and identical follow-ups.
4. Run `dotnet run --project lessons/09-start-separate-conversation/Example`.
5. Check Paris/art and Oslo/hiking remain paired.
6. Close with the application responsibility to map users or tasks to sessions securely.

## Talking points

- Reusing the agent is efficient and intended; reusing the conversation is a product decision.
- Variable names are illustrative. A real app uses an authenticated user/task mapping.
- A session separates context, but does not authenticate the user who requests it.
- The model wording may vary; the city/interest pairing is the important output.

## Likely questions

- **Should every HTTP request get a new session?** Only if it begins a new conversation; follow-ups must load the existing one.
- **Can two sessions run concurrently?** They are independent, but the app must still coordinate access to each individual session.
- **Does a new session copy instructions?** Instructions belong to the shared agent and apply to runs in both sessions.

## Recap and transition

Share the definition, isolate the state. Next, store a session so that isolation and continuity can survive a process restart.
