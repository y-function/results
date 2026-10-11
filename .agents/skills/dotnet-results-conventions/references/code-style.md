# C# and code style conventions

- Follow standard .NET/C# conventions with readability-first formatting.
- Use expression-bodied members sparingly; prefer clarity when short logic is easier to read.
- Keep method bodies compact and straightforward.
- Prefer `throw`/guard clauses early rather than nested conditionals.
- Favor explicit type arguments and explicit null checking when semantics matter.
- Limit the number of extension overloads when they become difficult to reason about.
- Favor readable, explicit flow over cleverness.

## Member ordering within a type

Keep a consistent order:

1. constructors / factory methods
2. public API
3. helper methods
4. private / internal helpers

## Conditional compilation

- Use `#if CLASSES` / `#elif STRUCTS` only for intentional multi-target implementation
  variants that share a common API shape.

## Maintainability

- Keep public APIs understandable and obvious; avoid unnecessary overload sprawl.
- If a feature or overload set becomes difficult to understand, refactor toward a
  smaller abstraction or a clearer helper method.
- Prefer composition over hidden behavior when bridging external frameworks to result types.
- Prefer small, explicit helpers over large nested blocks.
