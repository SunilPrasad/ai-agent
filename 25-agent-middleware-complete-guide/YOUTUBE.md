# YouTube publishing metadata

## Final title

Microsoft Agent Framework Middleware: The Complete Run Pipeline

## Description

Wrap a Microsoft Agent Framework `AIAgent` with C# middleware and follow the complete execution path. The production-shaped outer delegate logs safe request metadata, adds one fixed developer-owned instruction, forwards session/options/cancellation, observes success, and rethrows cancellation or failure. A tiny inner demo layer creates a reproducible downstream exception.

You will see how `AsBuilder().Use(...).Build()` creates an outer agent around the inner implementation, why request modification is a security and behavior boundary, and where agent-run middleware sits relative to context providers and the chat client.

Prerequisites: Sessions 1–14, .NET 10, `OPENROUTER_API_KEY`, and `OPENROUTER_MODEL`. The RAG sessions are independent. The next lesson compares one agent with an explicit workflow.

## Chapters

```text
00:00 One concern around every agent call
00:45 Wrapper mental model and pipeline
01:55 Build the inner and outer agents
03:00 Before: safe logging and timing
04:10 Change one request safely
05:35 Continue the inner pipeline
06:45 After: observe success
07:40 Preserve cancellation and failure
09:15 Run both paths
10:15 Middleware layers, streaming, and risks
10:55 Recap and workflow transition
```

## Thumbnail brief

A minimal dark navy thumbnail in the established series style. Large text: “MIDDLEWARE PIPELINE”. Include a clearly visible `C#` badge. Show one central agent/model circle wrapped by a glowing outer ring, with a request arrow entering through a logging/checkpoint icon and a response arrow leaving through a success/error observation icon. Cyan and violet accents, high contrast, generous margins, no dense code, logos, fake UI, secrets, or provider branding.

## Accessibility alt text

The words “MIDDLEWARE PIPELINE” above a C# badge, with an outer wrapper around an agent request and response path.
