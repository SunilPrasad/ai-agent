# Video guide

## Video title

Microsoft Agent Framework Function Tools in One C# Example

## Single learning outcome

Trace the complete function-tool loop and recognize its common forms: typed parameters, results, selection, expected failure, and agent delegation.

## Prerequisites

Sessions 1–10; .NET 10; `OPENROUTER_API_KEY` and `OPENROUTER_MODEL`; an OpenRouter model that supports function calling.

## Target length: 11:50

| Time | Section |
|---|---|
| 0:00–0:45 | Hook: a model cannot execute your C# by itself |
| 0:45–2:00 | Complete tool-loop diagram and ownership |
| 2:00–3:15 | Create tools and generate parameter schemas |
| 3:15–4:10 | Register the available tools |
| 4:10–5:55 | Run one tool; trace parameters and returned result |
| 5:55–7:05 | Let the model select a relevant tool |
| 7:05–8:15 | Represent an expected tool-domain failure |
| 8:15–10:00 | Convert a specialist agent into a tool |
| 10:00–11:05 | Run output and security boundary |
| 11:05–11:50 | Recap and structured-output transition |

## Opening hook and problem statement

Say: “The model can ask for `ConvertCurrency`, but only this .NET process can execute it.” Show the final console output with a `[tool]` line followed by the natural-language answer. Establish the difference between a requested call, local execution, and the final answer.

## Recording order

1. Show the README sequence diagram and trace one complete round trip.
2. Open `Example/Program.cs` and use its numbered section comments as the recording path.
3. Skim Section 1 because transport setup is already known.
4. Explain Sections 2–4: functions, specialist, and registration.
5. Jump to `ConvertCurrency` and connect each parameter to the model-produced JSON arguments.
6. Show `CurrencyResult` and explain why the model, not the method, writes the final prose.
7. Run the five prompts and connect each ordinary C# `[tool]` line to the relevant code section. Explain that this example sees only the specialist's returned text, not an inner-agent trace.
8. Show `Success=false` and distinguish expected domain failure from an unexpected exception.
9. Trace `PackingSpecialist.AsAIFunction()` as the same outer tool loop with an agent behind the delegate.

## Exact command

```powershell
dotnet run --project lessons/11-tools-complete-guide/Example
```

## Important talking points by code section

### Sections 1–2

- OpenRouter transports model requests; it does not run local functions.
- `AIFunctionFactory` and `AIFunction` belong to Microsoft.Extensions.AI.
- Method and parameter descriptions become model-visible metadata.

### Sections 3–4

- `AsAIFunction` belongs to Microsoft Agent Framework.
- One main agent registers ordinary functions and the specialist in the same tool collection.
- Registration grants availability, not guaranteed selection.

### Sections 5–7

- `RunAsync` hides a multi-call model/tool loop.
- The console trace proves which ordinary C# delegate executed and which values reached it.
- The model is expected to choose from descriptions and schemas; selection is not authorization.

### Section 8

- Unsupported routes are normal domain outcomes, represented by `Success=false`.
- Preserve a safe cause without leaking internals.
- Unexpected exceptions need production logging and policy, but are not expanded here.

### Section 9

- The outer agent sends a query string to the inner agent tool.
- The inner agent uses its own instructions and a fresh session per call in this example.
- Use an agent tool for genuine specialist reasoning, not simple arithmetic.

## Likely beginner questions

- **Does the model execute the method?** No. It requests a function name and arguments; local .NET code executes it.
- **Are parameter values safe because a schema exists?** No. Treat them as untrusted input and validate/authorize in C#.
- **Why can one `RunAsync` call the model twice?** The first response can request a tool; a later response uses its result to answer.
- **Will the model always choose the expected tool?** No. Names, descriptions, schemas, instructions, and model capability influence selection.
- **Why return a record?** It makes success, values, and expected domain-failure reasons explicit before the model explains them.
- **Does the packing specialist inherit the outer transcript?** No. Relevant context must be included in its query.

## Concise recap

The model chooses and proposes. The local function validates and executes. The result returns to the model. `AsAIFunction` applies that same loop to a specialist agent.

## Transition

The tools returned structured records internally. Next, request a typed structured result as the application's final response.
