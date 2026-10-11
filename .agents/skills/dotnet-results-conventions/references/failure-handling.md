# Failure handling

## Rules

1. **Fail fast** for invalid input and null arguments.
2. Use `ArgumentNullException.ThrowIfNull(...)` for required arguments.
3. Validate default values for result-like objects using an internal `AssertNotDefault` helper pattern.
4. **Preserve the original exception** when a failure result is returned; do not mask it.
5. Prefer returning structured failure results over throwing for domain-level operations
   that are part of the Result abstraction.
6. Keep exception translation explicit and narrow: when a method catches an exception, it
   should either convert it to a failure result or rethrow when the API contract requires it.
   Never swallow.

## Examples from the current codebase

- `Result.Failure(Exception exception)` validates `exception` before constructing a failure.
- `FromCondition` catches exceptions and converts them into `Failure` values.
- `Then` methods catch delegate exceptions and return converted failure results.

## Shared validation (DRY)

- Reuse constructors, factory methods, and validation helpers instead of duplicating logic.
- Centralize rules such as default validation and failure creation.
- Prefer small utility methods over repeated inline patterns.
- For repeated type variants, use generic implementations or shared overloads when they
  improve clarity without making the API too complex.
- `Success` and `Failure` factory overloads centralize result construction.
- `AssertNotDefault` avoids repeated null/default checks in extension methods.
- `FromCondition` overloads share core logic for both typed and untyped results.
