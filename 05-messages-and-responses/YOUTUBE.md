# YouTube publishing details

## Title

Microsoft Agent Framework C# — ChatMessage and AgentResponse

## Description

A string prompt is convenient, but it hides the message role. An agent response is also more than one string.

Video 05 creates an explicit Microsoft.Extensions.AI `ChatMessage` with `ChatRole.User`, sends it through Microsoft Agent Framework, then inspects `AgentResponse.Messages` and the convenient `AgentResponse.Text` property. The example shows when to keep message structure and when combined text is enough.

Prerequisites: Videos 01–04, .NET 10, and configured OpenRouter environment variables.

Next: stream an agent response as `AgentResponseUpdate` objects.

## Chapters

```text
00:00 The role hidden by a string
00:35 Why messages have structure
01:30 Message and response mental model
02:35 Walk through the C# code
05:50 Run and inspect output
06:45 Inside the conversion path
08:10 Text or Messages?
```

## Thumbnail brief

Text: **C# MESSAGES**. Navy background, cyan/violet accents, one user message envelope flowing into a response bundle.

## Accessibility alt text

Navy thumbnail reading “C# MESSAGES,” showing a user-message card becoming a bundle of response cards.
