# Video guide

## Video title

Microsoft Agent Framework Context and Memory in One C# Example

## Single learning outcome

Trace one context provider before and after every agent run, then explain how fresh context, conversation history, and selected memory differ.

## Prerequisites

Sessions 1–12, especially agent sessions and function tools; .NET 10; `OPENROUTER_API_KEY`; `OPENROUTER_MODEL`.

Session 13 is not required because invalid structured output is independent of this example.

## Target length: 11:40

| Time | Section |
|---|---|
| 0:00–0:50 | Hook: the model does not know changing application state |
| 0:50–2:05 | Prepare, run, learn lifecycle diagram |
| 2:05–3:15 | Register one `AIContextProvider` |
| 3:15–4:40 | Before hook: inject dynamic context and memory |
| 4:40–6:05 | After hook: store one selected preference |
| 6:05–7:35 | Run twice and watch context change |
| 7:35–9:10 | Conversation history versus memory |
| 9:10–10:25 | New session, then copy only the profile |
| 10:25–11:05 | Trust, token, persistence, and scope boundaries |
| 11:05–11:40 | Recap and RAG transition |

## Opening hook and problem statement

Change `supportStatus` in the code and ask: “How could the model know this changed if the user never said it?” Then show the `[provider before]` line. Establish that a provider proactively builds request context; it does not wait for the model to choose a tool.

## Recording order

1. Show the README sequence diagram and name the phases: prepare, run, learn.
2. Open `Example/Program.cs` and follow its six section comments.
3. Skim OpenRouter setup because it is already familiar.
4. Show the mutable `supportStatus`, provider constructor, and `AIContextProviders` registration.
5. Explain `ProvideAIContextAsync` before `StoreAIContextAsync` so retrieval precedes storage in the mental model.
6. Run conversation A and connect each provider trace to the lifecycle.
7. Change focus to the two-session comparison. Show blank provider state in conversation B.
8. Show `SetProfile` and emphasize that it creates a new profile containing one selected value—not conversation A's messages. Conversation B now has its own first exchange.
9. Close with trust, scoping, persistence, and token boundaries without introducing a vector store.

## Exact files and code sections to show

- `README.md`: request lifecycle diagram and history-versus-memory table.
- `Example/Program.cs`, Section 2: runtime status and provider registration.
- Section 3: explicit preference input and application-visible memory.
- Section 4: changed dynamic context.
- Section 5: separate session and deliberate memory copy.
- Section 6: `ProviderSessionState`, `ProvideAIContextAsync`, and `StoreAIContextAsync`.

## Exact command

```powershell
dotnet run --project lessons/14-context-and-memory-complete-guide/Example
```

## Talking points by code section

### Registration

- `AIContextProvider` belongs to Microsoft Agent Framework.
- Registration is developer-controlled and automatic on each run.
- Unlike a tool, no model decision triggers the provider.

### Before phase

- `ProvideAIContextAsync` runs before the model request.
- It reads dynamic status again on every run.
- It loads only the current session's `TravelProfile`.
- Returned `AIContext.Instructions` is merged into the request.

### After phase

- `StoreAIContextAsync` runs after a successful invocation through the base lifecycle.
- The prefix, lowercase normalization, and allow-list make this demo deterministic.
- Store selected information, not untrusted model prose by default.

### History versus memory

- History is the ordered transcript for one session.
- This memory is one selected application fact.
- A new session begins with blank history and provider state in this implementation.
- After B's first run, it has its own exchange but still none of A's transcript.
- Copying a new profile value proves that memory can move without moving A's transcript.

### Production boundaries

- Use an authenticated user scope and durable storage for real cross-session memory.
- Validate external context before injecting it.
- Injected context consumes tokens and retrieval adds latency.
- A provider does not retrain the model or guarantee factual answers.

## Expected output to show

Pause on four deterministic signals:

1. first `[provider before]` says no preference;
2. `[provider after]` stores `train`;
3. the second conversation's first run says no preference;
4. after `SetProfile`, the provider supplies `train`; B retains only its own history.

Model prose can differ. Do not present exact response wording as guaranteed.

## Likely beginner questions

- **Why not use a tool?** Tools are model-selected and on demand; a registered provider runs proactively.
- **Is chat history memory?** History can be used as a memory source, but a transcript and a selected durable fact are different data shapes and lifecycles.
- **Does every new session remember the user?** Not automatically. This provider stores state in each session; the application must load or copy user-scoped memory.
- **Why parse a prefix?** It keeps storage deterministic and avoids adding model-based extraction to this lesson.
- **Does the provider run after failures?** The provider lifecycle is notified, but the base implementation skips `StoreAIContextAsync` when invocation failed.
- **Is the injected text automatically trusted?** No. The application is responsible for authorization and validation.

## Recap

One provider retrieves fresh information before the call and stores a selected fact after success. Sessions keep histories separate; application-owned memory can be deliberately reused across them.

## Transition

“RAG uses the same before-the-model idea. Next, we will replace this tiny profile lookup with retrieval of relevant knowledge.”
