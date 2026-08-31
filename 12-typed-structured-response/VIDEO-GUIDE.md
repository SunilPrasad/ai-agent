# Video guide

## Video title

Microsoft Agent Framework Structured Responses: One Typed C# Result

## Single learning outcome

Explain why free-form model text is a brittle application contract and replace it with one `TripPlan` returned through `RunAsync<TripPlan>`.

## Prerequisites

Sessions 1–11; .NET 10; `OPENROUTER_API_KEY` and `OPENROUTER_MODEL`; an OpenRouter model that supports structured output.

## Target length: 9:45

| Time | Section |
|---|---|
| 0:00–0:45 | Hook: useful prose, unusable contract |
| 0:45–1:45 | Blank-page versus form mental model |
| 1:45–2:45 | Run the free-form path |
| 2:45–4:15 | Define `TripPlan` and ownership boundaries |
| 4:15–5:45 | Request `AgentResponse<TripPlan>` |
| 5:45–6:55 | Trace schema, JSON, and `.Result` internally |
| 6:55–8:05 | Use typed values in ordinary C# logic |
| 8:05–9:00 | Limits: provider support and validation |
| 9:00–9:45 | Recap and invalid-output transition |

## Opening hook and problem statement

Show a polished free-form city plan, then point to `Application received a String.` Say: “A person can understand this, but where is the property my code can compare?” The problem is not poor prose; it is the lack of a stable application contract.

## Recording order

1. Show the README flow diagram and compare the two branches.
2. Open `Example/Program.cs`; skim Section 1 because transport setup is already familiar.
3. Run Section 2 and emphasize that `Text` is one string even when it contains headings.
4. Explain `TripPlan` before showing the generic call: the application defines the contract.
5. Show the one central line, `agent.RunAsync<TripPlan>(Request)`.
6. Contrast `typedResponse.Text` with `typedResponse.Result`.
7. Trace property reads, the decimal comparison, count, and loop.
8. Close on the limits: schema support is model-dependent; typed shape is not factual or business validation; invalid output is the next lesson.

## Exact files and code sections to show

- `README.md`: “Free-form versus typed flow” diagram.
- `Example/Program.cs`, Section 2: `AgentResponse` and `.Text`.
- `Example/Program.cs`, `TripPlan`: ordinary C# contract, property names, and types.
- `Example/Program.cs`, Section 4: `RunAsync<TripPlan>` and `.Result`.
- `Example/Program.cs`, Section 5: normal typed property access and budget decision.

## Exact command

```powershell
dotnet run --project lessons/12-typed-structured-response/Example
```

Before recording, set `OPENROUTER_API_KEY` and choose an `OPENROUTER_MODEL` with structured-output support.

## Plain-language talking points

### Free-form branch

- `RunAsync(Request)` is good when a person is the consumer.
- Nice formatting does not create C# properties.
- Parsing generated headings couples code to presentation that may vary.

### The contract

- `TripPlan` belongs to this application; it does not inherit from Agent Framework.
- Property types state the desired JSON shape.
- Descriptive property names make units and meaning explicit in this small contract.
- `required` can enforce property presence during deserialization, but it cannot validate truth, sensible values, or the number of list items.

### Typed branch

- `T` is the response contract.
- Agent Framework requests a JSON-schema response format derived from `T`.
- `Text` exposes returned JSON; `.Result` deserializes it as `TripPlan`.
- The application can now compare a decimal and count a list without parsing prose.

### Boundaries and limits

- Agent Framework supplies the typed-run helper and response wrapper.
- Microsoft.Extensions.AI represents the response format and chat abstraction.
- OpenRouter routes to the selected model; that model must support the requested feature.
- Structured shape does not prove that a budget is accurate or that a rule such as exactly three activities was obeyed.

## Expected output to show

Show all three labeled blocks. Point out that the first block varies as prose, the second is JSON, and the third proves that C# received typed properties. Do not promise the exact generated activities or budget.

## Likely beginner questions

- **Why not ask for JSON in the prompt?** A prompt alone leaves the shape as a prose instruction. `RunAsync<T>` supplies an explicit response format and gives the framework a type to deserialize.
- **Is `TripPlan` a framework model?** No. It is an ordinary application-owned C# class.
- **Does `required` reject a missing model field?** Do not rely on it as your full runtime validator. Validate business-critical data after deserialization.
- **Is `.Result` a blocking task call?** No. Here it is the typed result property on an already awaited `AgentResponse<TripPlan>`.
- **Can every OpenRouter model do this?** No. Select a model that supports structured output.

## Recap

Free-form output is designed for reading. `RunAsync<TripPlan>` states a data contract, and `.Result` turns the returned JSON into normal C# properties that application logic can use.

## Transition to the next video

“We now have the happy path. Next, we will make the boundary honest when output is malformed, missing, or unacceptable.”
