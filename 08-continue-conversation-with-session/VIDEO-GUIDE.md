# Video guide

## Video title

Continue a Conversation with `AgentSession` in C#

## Single learning outcome

Use one session for multiple related turns without manually rebuilding chat history.

## Prerequisites

Sessions 1–7; .NET 10; OpenRouter environment variables configured.

## Target length: 8:30

| Time | Section |
|---|---|
| 0:00–0:50 | Hook: an incomplete follow-up |
| 0:50–2:00 | Same-session rule |
| 2:00–3:15 | Sequence diagram |
| 3:15–6:40 | Code walkthrough |
| 6:40–7:50 | Run and inspect continuity |
| 7:50–8:30 | Recap and transition |

## Opening hook

Show “Make the morning activity free.” alone and ask what morning and which city it refers to. The missing meaning lives in the prior turn.

## Recording order

1. Show the two prompts before the code.
2. Walk through the Mermaid sequence and emphasize the same `AgentSession` object on both calls.
3. Open `Example/Program.cs`; skim familiar OpenRouter setup.
4. Highlight the single `CreateSessionAsync` call and both `RunAsync` calls.
5. Run `dotnet run --project lessons/08-continue-conversation-with-session/Example`.
6. Point out that the second answer modifies the first plan without manual history code.

## Talking points

- The prompt is new; the conversation is not.
- The session is supplied explicitly, making continuity visible in application code.
- `ChatClientAgent` and its history provider perform the transcript work for this setup.
- Model wording is nondeterministic; correct continuity is the observable behavior.

## Likely questions

- **Does the session itself call the model?** No; the agent runs the model and uses the session as its state boundary.
- **Can I send a full `ChatMessage` collection?** Yes, but this lesson isolates session-managed continuation.
- **How long can a conversation grow?** Model context limits still apply; reduction is a later topic.

## Recap and transition

Reuse one session for one conversation. Next, create two sessions to isolate two conversations.
