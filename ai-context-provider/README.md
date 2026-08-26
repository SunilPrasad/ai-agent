# AI Context Provider

This sample demonstrates why Microsoft Agent Framework has AI context providers.

An AI context provider supplies application data immediately before the model is invoked. The caller asks a normal question and does not need to manually append that data to every prompt.

## Why not put the data in the instructions?

Instructions and context have different responsibilities:

- **Instructions** define stable behavior, such as “You are a support assistant” and “Say when you do not know.”
- **Context providers** supply data that can change between invocations, such as current business data, the signed-in user's profile, permissions, or retrieved documents.

The support hours are hardcoded in this small sample only so it can focus on the provider mechanism. In a real application, `SupportContextProvider` would load the latest hours from a database or API whenever the agent runs. This avoids rebuilding the agent or mixing changing business data into its permanent behavioral instructions.

```text
user question
     +
SupportContextProvider context
     |
     v
   agent -> chat model -> grounded answer
```

In this sample, `SupportContextProvider` injects the support opening hours. Asking `When is support available?` can therefore be answered even though those hours are not included in the user's question or the agent's permanent instructions.

Context providers are useful for information that comes from your application, such as:

- retrieved documents for RAG;
- user profile information;
- permissions or tenant settings;
- current application state.

## Run

Set your OpenAI API key in PowerShell:

```powershell
$env:OPENAI_API_KEY = "your-key"
```

Run the default question:

```powershell
dotnet run
```

Or provide another question:

```powershell
dotnet run -- "Is support open on Saturday?"
```

The console prints a `CONTEXT PROVIDER` message so you can see when the provider runs.
