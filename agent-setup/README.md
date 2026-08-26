# Simple Agent Setup

This is the smallest sample in this repository. It creates a Microsoft Agent Framework agent, sends it one question, and prints the model's response.

```text
question -> agent -> chat model -> answer
```

## Run

Set your OpenAI API key in PowerShell:

```powershell
$env:OPENAI_API_KEY = "your-key"
```

Run the default question:

```powershell
dotnet run
```

Or provide a question:

```powershell
dotnet run -- "Explain embeddings in one paragraph"
```

The sample does not create an `AgentSession` because it performs only one request and does not need multi-turn conversation state.
