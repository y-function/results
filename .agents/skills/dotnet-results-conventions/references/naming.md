# Naming conventions

## Projects and folders

- Use clear, domain-oriented names organized by responsibility.
- Align names with the root namespace pattern:
  - `Y.Results`
  - `Y.Results.Web`
  - `Y.Results.Extensions`
  - `Y.Results.Tests`
- Keep project folders aligned with namespace boundaries and feature responsibilities.
- One responsibility per project/folder; do not mix web, core, and tests into one assembly.

## Files

- One primary type or responsibility per file when practical.
- File names reflect the main abstraction or feature they contain.
- For `partial` types, name files by the logical aspect they represent:
  - `Result.cs`
  - `Result.Ctors.cs`
  - `Result.Methods.cs`
  - `Result.Wrap.cs`
  - `ResultActionFilter.cs`

## Types

- PascalCase for all type names.
- Prefer noun-based or domain-based names for model/result types: `Result`, `Result<T>`, `ResultActionFilter`.
- Keep types focused and cohesive; do not combine unrelated concerns in one type.
- Use `partial` types when a feature is logically split across files but still belongs to one type.

## Methods

- PascalCase, verb-first when action-oriented.
- Prefer explicit operation names:
  - `Success`
  - `Failure`
  - `Then`
  - `OnSuccess`
  - `OnFailure`
  - `TryGetFailure`
  - `FromCondition`
- Use factory methods to centralize creation rules and validation.

## Parameters and local variables

- camelCase.
- Names express intent, not abbreviations: `expectedResult`, `exceptionHandler`, `onFailure`, `result`, `value`.
- Avoid vague names like `obj`, `tmp`, `data` unless the scope is very small and obvious.

## Properties and fields

- PascalCase property names.
- Keep properties explicit and domain-driven: `IsSuccess`, `Exception`, `Value`.
- Prefer simple property expressions over rich backing-field logic unless needed.

## Constants

- Use `const` for static, fixed values and name them in PascalCase.
- Example: `ConditionValidationHasFailedErrorMessage`.

## Tests

- Test names describe the behavior under test and the expected result.
- Test namespaces align with the component under test.
