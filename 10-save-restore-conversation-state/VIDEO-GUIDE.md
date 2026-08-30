# Video guide

## Video title

Save and Restore AgentSession State in C#

## Single learning outcome

Persist an agent-produced session snapshot and restore it for a later turn.

## Prerequisites

Sessions 1–9; .NET 10; OpenRouter environment variables configured.

## Target length: 9:30

| Time | Section |
|---|---|
| 0:00–0:55 | Hook: process memory is temporary |
| 0:55–2:15 | Save-game mental model and responsibilities |
| 2:15–3:30 | Serialization flow diagram |
| 3:30–7:25 | Code walkthrough |
| 7:25–8:25 | Run, inspect path, verify recall |
| 8:25–9:05 | Security boundary |
| 9:05–9:30 | Recap and tools transition |

## Opening hook

Close an imaginary console window and ask where its conversation object went. Explain that continuity across processes needs durable bytes, not a reference to a .NET object.

## Recording order

1. Show the live session → JSON → file → restored session diagram.
2. State the ownership split: framework converts; application stores.
3. Open `Example/Program.cs` and skim familiar configuration.
4. Show first run, `SerializeSessionAsync`, file write, file read, `DeserializeSessionAsync`, and the follow-up.
5. Explain why `RootElement.Clone()` is used.
6. Run `dotnet run --project lessons/10-save-restore-conversation-state/Example`.
7. Show the printed file path but do not display private session contents in a recording with real user data.

## Talking points

- Call the agent's serialization API because the agent owns its session format.
- `JsonElement` is the framework/storage handoff boundary.
- The local file is a demonstration, not a production recommendation.
- Use the restored object in the final call to prove the flow.
- Session snapshots are sensitive and untrusted storage can tamper with future context.

## Likely questions

- **Why not serialize the session object directly?** Its concrete shape and behaviors belong to the agent implementation; use the agent's supported API.
- **Must storage be JSON files?** No. Store the returned JSON in an appropriately secured database, blob, or other durable store.
- **Can another agent restore it?** It need not be the same object instance, but do not assume compatibility; use a compatible agent implementation and configuration.
- **Does saving include the API key?** The example never puts the key in session state, but snapshots can still contain sensitive conversation data.

## Recap and transition

Serialize with the agent, store securely in the application, restore with the agent, then continue. Next, let the model request one C# function tool.
