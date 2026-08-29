# YouTube publishing details

## Title

Microsoft Agent Framework for .NET — What Is AIAgent?

## Description

Why does Microsoft Agent Framework code use `AIAgent` when the object running behind it is a `ChatClientAgent`?

In this beginner-friendly .NET lesson, we use one small console application to make that relationship visible. You will see the variable type, the runtime type, and a helper method that can run an agent without depending on OpenRouter, the OpenAI SDK, or a concrete agent implementation.

You will learn:

- What the abstract `AIAgent` base class represents.
- How it relates to `ChatClientAgent`.
- Why application code accepts the common base type.
- How `RunAsync` reaches the concrete implementation.
- Which library or service owns each type in the example.

Prerequisites: basic C# inheritance and Video 01 of this series. The runnable example uses .NET 10, Microsoft Agent Framework, and an OpenRouter API key stored in `OPENROUTER_API_KEY`; the model name comes from `OPENROUTER_MODEL`.

Next: Video 03 separates `AIAgent`, `IChatClient`, the OpenAI .NET SDK, and OpenRouter so every layer has a clear job.

## Chapters

```text
00:00 One method, any agent
00:35 The concrete-type problem
01:20 What AIAgent represents
02:25 Base type versus runtime type
03:25 Who owns each type
04:10 Code walkthrough
06:55 Run the example
07:40 What happens inside the framework
08:40 Recap and next video
```

## Thumbnail brief

Use the established series style: a deep navy background, bright white main text, cyan and violet accents, and one simple developer-oriented diagram. Large text: **AIAgent EXPLAINED**. Show one central abstract base card branching to three small, distinct implementation shapes. Avoid logos, robots, code blocks, provider names, and extra labels.

## Accessibility alt text

Dark navy YouTube thumbnail reading “AIAgent EXPLAINED,” with one central abstract block connected to three smaller implementation blocks.
