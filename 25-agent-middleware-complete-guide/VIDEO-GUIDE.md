# Video guide

## Video title

Microsoft Agent Framework Middleware: The Complete Run Pipeline

## Single learning outcome

Trace an outer agent-run middleware delegate on the way into and out of the remaining pipeline, including logging, a fixed request change, and preservation of a deterministic inner failure.

## Prerequisites

Sessions 1–14; .NET 10; `OPENROUTER_API_KEY`; `OPENROUTER_MODEL`.

The RAG sessions numbered 15–24 are not required because middleware wraps the agent independently of its knowledge source.

## Target length: 11:25

| Time | Section |
|---|---|
| 0:00–0:45 | Hook: one concern around every agent call |
| 0:45–1:55 | Wrapper mental model and pipeline diagram |
| 1:55–3:00 | Build the inner and outer agents |
| 3:00–4:10 | Before: request ID, timer, and safe logging |
| 4:10–5:35 | Change: add one fixed system instruction |
| 5:35–6:45 | Continue: forward session, options, and cancellation |
| 6:45–7:40 | After: log success and return the same response |
| 7:40–9:15 | Failure: catch, distinguish cancellation, and rethrow |
| 9:15–10:15 | Run both paths and read the trace |
| 10:15–10:55 | Middleware layers, streaming, and risks |
| 10:55–11:25 | Recap and workflow transition |

## Opening hook and problem statement

Show the success trace and ask: “Where would you put this timing code if ten different agents need it?” Establish that middleware is a reusable wrapper around a run, not another prompt and not a model-selected tool.

## Recording order

1. Show the README pipeline diagram and trace the inward and outward arrows.
2. Open `Example/Program.cs` and follow Sections 1–6.
3. Skim provider setup; focus on `innerAgent`, the outer middleware, the tiny failure-only inner middleware, and `Build`.
4. Walk the middleware top-to-bottom: materialize, log, transform into a new message list, call `nextAgent`, log after, return.
5. Explain the cancellation catch before the general catch.
6. Run the success prompt and connect each line to its code.
7. Run the deterministic marker and show outer change → inner throw → outer catch → same type/message at the application.
8. Close by locating context-provider, function, and chat-client middleware as different scopes without implementing them.

## Exact files and code sections to show

- `README.md`: “Where middleware sits” diagram.
- `Example/Program.cs`, Section 1: inner agent.
- Section 2: two `Use(...)` registrations and `Build()`.
- Sections 3–4: successful and deterministic failure calls.
- Section 5: complete outer middleware delegate.
- Section 6: tutorial-only inner failure middleware.

## Exact command

```powershell
dotnet run --project lessons/25-agent-middleware-complete-guide/Example
```

## Talking points by code section

### Build the wrapper

- `AIAgentBuilder` belongs to Microsoft Agent Framework.
- `Build` returns an `AIAgent`, so application calling code does not change.
- Both callbacks are batch run middleware; production incremental streaming needs a streaming-aware path.

### Before the inner call

- Materialize the enumerable once.
- Log metadata rather than prompts or credentials.
- A fixed system instruction is developer policy, not user text.
- Build a new collection so the original input remains unchanged.

### Continue and return

- `nextAgent.RunAsync` continues to inner layers.
- Forward the same session, options, and cancellation token.
- The successful path returns the same inner response.
- Not calling the inner agent would short-circuit execution.

### Failure path

- The inner demo middleware is reproducible tutorial scaffolding.
- The outer `try` covers its own logic and the awaited next layer.
- Caller cancellation receives a separate log category.
- Bare `throw;` preserves the active exception and its stack.
- Recovery or fallback must be an explicit application policy.

## Expected output to show

For success, point to before → change → after → application. For failure, point to outer before → change → inner demo throw → outer error → application catch. The console proves matching type/message; bare-throw stack preservation is a C# semantic. Request IDs and timings vary.

## Likely beginner questions

- **Is there an `IMiddleware` interface?** Not for this delegate example. `Use` accepts the Agent Framework callback shape.
- **What is `nextAgent`?** The next inner `AIAgent` in the built wrapper chain.
- **Can middleware block a run?** Yes, by returning or throwing without calling the inner agent. Do that only as an intentional policy.
- **Why create new messages?** It avoids mutating caller-owned inputs and makes the transformation explicit.
- **Can I log prompts?** Technically yes, but they may contain personal data, proprietary content, or secrets. Default to safe metadata.
- **Why not return a friendly response in the catch?** That could hide a real failure. This lesson preserves it; deliberate fallback is a separate policy.
- **Does this handle streaming?** Only through the framework's batch-to-update adaptation. Implement a streaming delegate for genuine incremental middleware behavior.

## Recap

Agent middleware wraps the whole run. It can inspect or change inputs, call the inner agent, observe success or failure, and return or rethrow with deliberate semantics.

## Transition

“Middleware controls how an agent runs. Next, we decide when one agent is no longer enough and an explicit workflow is the clearer design.”
