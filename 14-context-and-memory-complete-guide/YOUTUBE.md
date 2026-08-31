# YouTube publishing metadata

## Final title

Microsoft Agent Framework Context and Memory in One C# Example

## Description

Learn how a Microsoft Agent Framework `AIContextProvider` adds fresh application information before every model request and stores one selected preference after a successful response.

The commented C# example changes runtime support status, traces the provider lifecycle, and starts a second `AgentSession` to show the difference between conversation history and application-owned memory. It then creates a new `TravelProfile` containing only the selected preference in that conversation—without replaying the original transcript.

Prerequisites: Sessions 1–12, .NET 10, `OPENROUTER_API_KEY`, and `OPENROUTER_MODEL`. Session 13 is independent and is not required. The next lesson starts the RAG mini-series.

## Chapters

```text
00:00 The model cannot see changing application state
00:50 Prepare, run, learn
01:50 Build the OpenRouter model client
02:40 Register one context provider
03:35 Inject dynamic context and memory
04:50 Parse and store one selected preference
06:20 Watch context change between runs
07:35 Conversation history versus memory
08:55 New session, copied profile
10:25 Trust, tokens, persistence, and scope
11:05 Recap and RAG transition
```

## Thumbnail brief

A minimal dark navy thumbnail in the established series style. Large text: “CONTEXT + MEMORY”. Include a clearly visible `C#` badge. Show a central agent/request circle receiving a bright live-context lightning bolt from the left and a small memory card from the right. Cyan and violet accents, high contrast, generous margins, and no dense code, logos, provider UI, or extra wording.

## Accessibility alt text

The words “CONTEXT + MEMORY” above a C# badge, with live context and a memory card flowing into one agent request.
