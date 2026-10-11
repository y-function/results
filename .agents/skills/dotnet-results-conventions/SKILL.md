---
name: dotnet-results-conventions
description: Coding conventions for the Y.Results .NET solution (Result/Result<T> pattern). Use when adding, refactoring, or reviewing C# code here - result APIs, extension methods, ASP.NET integration, partial files, or unit tests - so new code matches the repository's naming, style, failure-handling, and project layout.
license: MIT
compatibility: Any repository containing the Y.Results solution (net8+). No external tooling required.
metadata:
  scope: repository
  area: dotnet, csharp, result-pattern
  source: SKILLS.md
---

# Y.Results repository conventions

Apply these rules to every code change in this solution. Read the referenced files
before making larger or structural changes; they contain the full detail and the
examples from the current codebase.

## Non-negotiables

1. **One responsibility per project and per file.** Core result logic stays in
   `Y.Results`, fluent extensions in `Y.Results.Extensions`, ASP.NET integration in
   `Y.Results.Web`, tests in `Y.Results.Tests`. Never mix web, core, and tests in one assembly.
2. **Fail fast on invalid input.** `ArgumentNullException.ThrowIfNull(...)` for required
   arguments; `AssertNotDefault`-style validation for result-like values.
3. **Never mask an exception.** A failure result must preserve the original exception.
   A `catch` either converts to a failure result or rethrows, never swallows.
4. **Reuse construction rules.** Route result creation through `Success` / `Failure`
   factories and shared validation helpers instead of duplicating checks inline.
5. **Keep the API surface disciplined.** Add an overload only when it is truly needed;
   if an overload set gets hard to reason about, refactor to a smaller, clearer helper.
6. **Test every new branch.** Every success/failure path and edge case gets a test whose
   name describes the behavior and the expected outcome.

## Quick decision guide

| Situation | Do |
|---|---|
| New public operation | Verb-first PascalCase name (`Then`, `OnSuccess`, `OnFailure`, `TryGetFailure`, `FromCondition`); creation goes through a factory method |
| Type gets too large | Split into `partial` files named by logical aspect (`Result.cs`, `Result.Ctors.cs`, `Result.Methods.cs`, `Result.Wrap.cs`) |
| Framework dependency appears | Put it in the integration project (`Y.Results.Web`), never in core domain types |
| Invalid argument | Guard clause at the top of the method, early `throw` |
| Domain-level failure | Return a structured failure result rather than throwing |
| Multi-target implementation variants | `#if CLASSES` / `#elif STRUCTS` only when the API shape stays common |

## References

- `references/naming.md` - projects, folders, files, types, methods, parameters, properties, constants
- `references/code-style.md` - formatting, method bodies, member ordering, conditional compilation
- `references/failure-handling.md` - guard clauses, exception preservation, conversion rules, examples
- `references/architecture.md` - project layout, separation of core and integration, test placement
- `references/principles.md` - SOLID, KISS, DRY, YAGNI as observed in this solution
- `references/checklist.md` - pre-commit checklist for changes and new features

## Workflow

1. Identify which project owns the change (core, extensions, web, tests).
2. Sketch the smallest public API that expresses it; check whether an existing
   factory or helper already covers it.
3. Implement with early guards, compact bodies, and preserved exceptions.
4. Add tests for each new branch, named after the behavior under test.
5. Self-review against `references/checklist.md` before finishing.
