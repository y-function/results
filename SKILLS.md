# Repository coding conventions and standards

This document captures the conventions currently used in the .NET solution and is intended to guide future code changes in a consistent way.

## 1. Naming conventions

### Projects and folders
- Use clear, domain-oriented names organized by responsibility.
- Prefer names aligned to the root namespace pattern:
  - `Y.Results`
  - `Y.Results.Web`
  - `Y.Results.Extensions`
  - `Y.Results.Tests`
- Keep project folders aligned with namespace boundaries and feature responsibilities.
- Prefer a single responsibility per project/folder rather than mixing web, core, and tests into one assembly.

### Files
- Use one primary type or responsibility per file when practical.
- File names should reflect the main abstraction or feature they contain.
- For `partial` types, name files according to the logical aspect they represent, e.g.:
  - `Result.cs`
  - `Result.Ctors.cs`
  - `Result.Methods.cs`
  - `Result.Wrap.cs`
  - `ResultActionFilter.cs`

### Types
- Use PascalCase for all type names.
- Prefer noun-based or domain-based names for model/result types: `Result`, `Result<T>`, `ResultActionFilter`.
- Keep types focused and cohesive: do not combine unrelated concerns inside a single type.
- Use `partial` types when a feature is logically split across files but still belongs to one type.

### Methods
- Use PascalCase method names and verb-first names when action-oriented.
- Prefer explicit operation names such as:
  - `Success`
  - `Failure`
  - `Then`
  - `OnSuccess`
  - `OnFailure`
  - `TryGetFailure`
  - `FromCondition`
- Use factory methods to centralize creation rules and validation.

### Parameters and local variables
- Use camelCase for parameters and local variables.
- Prefer clear names that express intent, not abbreviations.
- Example: `expectedResult`, `exceptionHandler`, `onFailure`, `result`, `value`.
- Avoid vague names like `obj`, `tmp`, or `data` unless the scope is very small and obvious.

### Properties and fields
- Use PascalCase property names.
- Keep properties explicit and domain-driven: `IsSuccess`, `Exception`, `Value`.
- Prefer simple property expressions over rich backing-field logic unless needed.

### Constants
- Use `const` for static, fixed values and name them in PascalCase.
- Example: `ConditionValidationHasFailedErrorMessage`.

## 2. C# and code style conventions

- Follow standard .NET/C# conventions and readability-first formatting.
- Use expression-bodied members sparingly; prefer clarity when short logic is easier to read.
- Keep method bodies compact and straightforward.
- Prefer `throw`/guard clauses early rather than nested conditionals.
- Favor explicit type arguments and explicit null checking when semantics matter.
- Preserve a consistent ordering of members within types:
  - constructors/factories
  - public API
  - helper methods
  - private/internal helpers
- Use `#if CLASSES` / `#elif STRUCTS` only for intentional multi-target implementation variants that share a common API shape.

## 3. Failure handling

- Fail fast for invalid input and null arguments.
- Use `ArgumentNullException.ThrowIfNull(...)` for required arguments.
- Validate default values for result-like objects using an internal `AssertNotDefault` helper pattern.
- Preserve the original exception when a failure result is returned; do not mask it.
- Prefer returning structured failure results over throwing for domain-level operations that are part of the Result abstraction.
- Keep exception translation explicit and narrow: when a method catches an exception, it should either convert it to a failure result or rethrow when the API contract requires it.

Examples from the current codebase:
- `Result.Failure(Exception exception)` validates `exception` before constructing a failure.
- `FromCondition` catches exceptions and converts them into `Failure` values.
- `Then` methods catch delegate exceptions and return converted failure results.

## 4. DRY and reusable abstractions

- Reuse constructors, factory methods, and validation helpers instead of duplicating logic.
- Centralize rules like default validation and failure creation.
- Prefer small utility methods over repeated inline patterns.
- For repeated type variants, use generic implementations or shared overloads when they improve clarity without making the API too complex.

Examples:
- `Success` and `Failure` factory overloads centralize result construction.
- `AssertNotDefault` avoids repeated null/default checks in extension methods.
- `FromCondition` overloads share core logic for both typed and untyped results.

## 5. File organization and architecture

- Separate core domain logic from framework-specific integrations.
- Keep framework dependencies isolated in dedicated areas such as `Y.Results.Web`.
- Keep tests in dedicated test projects with namespace alignment to the component under test.
- Prefer logic grouping by capability rather than by incidental implementation details.

Current structure is a good example:
- `Y.Results`: core result abstraction and behavior
- `Y.Results.Extensions`: fluent extension methods
- `Y.Results.Web`: ASP.NET integration and filters
- `Y.Results.Tests`: unit tests for core behavior

## 6. Maintainability guidance

- Keep public APIs understandable and obvious; avoid unnecessary overload sprawl.
- Favor readable, explicit flow over cleverness.
- If a feature or overload set becomes difficult to understand, refactor toward a smaller abstraction or clearer helper method.
- Limit the number of extension overloads when they become difficult to reason about.
- Prefer composition over hidden behavior when bridging external frameworks to result types.
- Use test names that describe the behavior under test and expected result.

## 7. SOLID, KISS, and other principles observed

### SOLID
- Single Responsibility: core types own a single domain concept (result success/failure and error state); web adaptations are isolated in a separate project.
- Open/Closed: result APIs are easy to extend with additional operations without modifying core behavior in a brittle way.
- Liskov Substitution: generic `Result<T>` and base `Result` relationships are designed to be polymorphic in a predictable way.
- Interface Segregation: the abstraction remains focused; consumers and framework adapters use the minimal needed surface.
- Dependency Inversion: framework-specific handling is abstracted behind higher-level result semantics rather than hard-coded behavior inside domain logic.

### KISS
- The code favors straightforward success/failure states and explicit branching over elaborate abstractions.
- The implementation remains easy to follow, even with multiple generic overloads.

### DRY
- Centralized validation and factory creation reduces repetition across the library.
- Reused helper methods avoid repeated result-checking logic in APIs and tests.

### YAGNI / simplicity-first
- The library keeps a narrow scope: represent success/failure and composition around it.
- Avoid adding unnecessary abstractions beyond the core result workflow.

## 8. Practical guidance for future changes

- Preserve the existing conventions when extending the library.
- Add tests for every new success/failure branch or edge case.
- Keep implementation and test naming consistent with the repository conventions.
- Prefer small, explicit helpers over large nested blocks.
- When adding overloads, check whether they are truly needed; if they become confusing, prefer a clearer minimal API.
- Keep framework integration separate from the core result model.

## 9. Summary

The solution generally follows a clean .NET library pattern:
- domain-first naming
- small focused types
- clear result semantics
- fail-fast validation
- explicit exception handling
- separation of core and integration concerns
- maintainable file organization

This is a strong foundation for future evolution, provided the overload surface remains disciplined and refactoring stays guided by readability and simplicity.
