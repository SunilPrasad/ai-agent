Great, I've got the full content. This is a well-written guide from Session 14 of an AI agent course. Let me break it down for you clearly.

---

## What This Is About

This guide teaches how to give an AI agent **memory and live context** in a C# application using Microsoft's Agent Framework. The core challenge it solves: *a language model only knows what you put in its current request* — it has no automatic memory of previous conversations or access to changing application data.

---

## The Central Concept: `AIContextProvider`

An `AIContextProvider` lets application code add fresh information before every agent request, and save a small selected piece of state after a successful request.

Think of it as a **middleware hook** that wraps every model call with a "before" and "after" step:

- **Before:** inject current facts (e.g. "support is closed", "user prefers train")
- **After:** inspect the conversation and save any useful fact to session state

---

## The Four Types of Information

The guide distinguishes four kinds of information an agent can work with:

| Information | Source | Lifetime |
|---|---|---|
| Base instructions | `ChatOptions.Instructions` | Every run of this agent |
| Dynamic context | `getSupportStatus()` | Read again for each run |
| Conversation history | Chat-history provider | Messages in that session |
| Selected memory | `TravelProfile` in session state | One value stored per session |

The key insight is that **history and memory are not the same thing** — history is the full transcript, memory is one specific fact the app chose to remember.

---

## The Demo: A Travel Assistant

The example runs a travel assistant through 4 calls across 2 conversations (A and B):



| Run | What happens |
|---|---|
| A1 | User says "remember transport=train". Provider stores it. Support is open. |
| A2 | User asks for travel advice. Provider injects stored preference + new "support closed" status. |
| B1 | New conversation B, blank state. Model says no preference is stored. |
| B2 | App manually copies A's `train` preference into B. Model now knows it. |

This proves that **memory doesn't transfer automatically** between sessions — you have to deliberately move it.

---

## The Lifecycle (Simplified)

```
App calls RunAsync(message, session)
    → History loaded for this session
    → Provider "before" hook: reads live support status + saved preference
    → Model receives: base instructions + history + provider context + new message
    → Model responds
    → Provider "after" hook: parses response, saves "train" if found
    → RunAsync returns
```

The after hook completes before the awaited `RunAsync` returns, so saved state is available on the very next line of application code.

---

## Key Design Decisions Explained

**Why not use base instructions for live data?**
Permanent base instructions are a poor home for changing facts. A function tool solves a different problem because the model chooses whether to call it. A registered context provider is proactive: application code runs it around every relevant invocation.

**How does the "after" hook save preferences?**
The parser deliberately looks for a specific prefix like `Remember preference: transport=`, normalizes the value to lowercase, and only accepts `train`, `plane`, or `car`. The app does not trust model prose or make another model call to choose memories.

**When should you use a context provider vs. a function tool?**
Use a context provider when application-controlled information should be assembled proactively — for example current account status, an authorized profile, selected memories, or retrieved knowledge. Use a function tool when the model should decide whether and when to perform an on-demand lookup or action.

---

## The Takeaway

The pattern here is called **"Prepare → Run → Learn"**:

1. **Prepare** — read live app data and any saved state for this session
2. **Run** — inject it into the model request
3. **Learn** — after success, extract and save only one specific, validated fact

This is a clean, controlled alternative to giving the model free-rein over its own memory. The app stays in charge of *what* gets remembered and *when*.

The guide says Session 15 extends this same idea into RAG (Retrieval-Augmented Generation) — retrieving relevant knowledge chunks before the model call instead of a single preference value.
