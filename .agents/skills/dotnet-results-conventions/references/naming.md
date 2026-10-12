# Naming conventions (as actually used)

## Projects and assemblies

- Pattern: `Y.Results.<Variant>[.<Layer>][.Tests]` - variant is `Classes` or `Structs`, layer is
  `Extensions` or `Web`. Example: `Y.Results.Structs.Extensions.Tests`.
- The suffix `.Tests` is load-bearing: `Directory.Build.props` uses it to detect test projects and
  import `Tests.props`. Never name a test project without it.

## Namespaces (declared explicitly in every file)

| Code | Namespace |
|---|---|
| Core + extension implementations | `Y.Results` |
| ASP.NET integration | `Y.Results.Web` |
| Core tests | `Y.Results.Tests` |
| Extension tests | `Y.Results.Extensions.Tests` |
| Web tests | `Y.Results.Web.Tests` |

Declare the namespace in the file; do not depend on `RootNamespace` (it is inconsistent between the
`Classes` and `Structs` extension projects).

## Files

Pattern: `<Subject>.<Aspect>.cs` - one logical aspect per partial file, subject named after the type
being implemented *or extended*.

- Core: `Result.cs`, `Result.Generic.cs`, `Result.Ctors.cs`, `Result.Methods.cs`, `Result.Wrap.cs`,
  `Result.WhenAll.cs`, `IResult.cs`
- Extensions (type is `ResultExtensions`, subject is `Result`): `ResultExtensions.cs` (shared private
  helpers), `Result.On.cs`, `Result.Then.cs`, `Result.Then.Async.cs`, `Result.Validations.cs`
- Web: `ResultActionFilter.cs`, `ReturnsNoContentOnSuccess.cs`
- Tests mirror the implementation file with `.Tests`: `Result.Wrap.Tests.cs`, `Result.OnSuccess.Tests.cs`,
  `Result.Then.Async.Tests.cs`, `ResultExtensions.Tests.cs`

Rules to follow for new files:

- Async variants of an aspect go in a separate `<Aspect>.Async.cs` file, not mixed into the sync file.
- A new aspect of an existing type gets its own partial file (`Result.<NewAspect>.cs`) rather than
  growing an existing one.
- Remember rule 1 of the skill: add the file to the `Structs` twin `.csproj`.

Known deviation, do not copy: `ReturnsNoContentOnSuccess.cs` declares `ReturnsNoContentOnSuccessAttribute`
- the file name drops the `Attribute` suffix. New attribute files should match their type name.

## Types

- PascalCase, noun-based: `Result`, `Result<T>`, `IResult`, `ResultExtensions`, `ResultActionFilter`,
  `ReturnsNoContentOnSuccessAttribute`.
- Declaration is repeated per partial file under `#if CLASSES` / `#elif STRUCTS`:
  `public partial class Result` vs `public readonly partial record struct Result`.
- Generic parameters: single `T` for the result value; `TIn`/`TOut` when a transformation changes the
  value type; `T1`,`T2`,`T3` for `WhenAll` arity overloads.
- Abstraction is internal on purpose: `IResult` is `internal` so framework adapters can match any
  result without exposing the seam.

## Methods

- PascalCase, verb-first, and the same verb is reused for untyped and generic results.
- Existing verbs, in the order they appear in a type: `Success`, `Failure`, `FromCondition`, `Wrap`,
  `WhenAll`, `TryGetFailure`, `AssertNotDefault`, `EvalExceptionOrDefault`, `CastFailure`, `OnSuccess`,
  `OnFailure`, `Then`, `Validate`, `ValidateNotNull`, `ThrowIfFailed`, `GetValueOrThrow`.
- Naming patterns to reuse:
  - factory with no failure supplied -> `Success` / `Failure`
  - converting an unsafe operation into a result -> `Wrap`
  - combining several results -> `WhenAll`
  - attempt that returns `bool` + `out` -> `Try*`
  - fluent chaining -> `Then`, observation only -> `On*`, re-asserting a result -> `Validate*`
  - rethrowing escape hatch -> `ThrowIf*` / `*OrThrow`
- Internal validation helpers are named `Assert*`.

## Parameters, locals, constants

- camelCase parameters that name the role, not the type: `unsafeOperation`, `onFailure`, `predicate`,
  `next`, `action`, `exception`, `results`, `value`.
- Locals: `values`, `e` (catch clause), `v`, `httpResult`, `fileResult`, `hasNoContentOverride`,
  `expectedResult`, `expectedException`.
- `private const string` in PascalCase for fixed messages (`ConditionValidationHasFailedErrorMessage`).
- Avoid `obj`, `tmp`, `data`.

## Properties

- PascalCase, domain-driven: `IsSuccess`, `Exception`, `Value`, `IResult.Value` (explicit interface
  implementation when the typed value is not available).
- Simple expressions; no backing-field logic.

## Tests

- Test class = `partial class <Type>Tests` (`ResultTests`, `ResultExtensionsTests`), split across one
  file per aspect; single-file tests may be non-partial (`ResultActionFilterTests`).
- Test method: `Test<Subject><Scenario><ExpectedOutcome>`, e.g. `TestThenResultFuncAsyncWithFailureResult`,
  `TestOnFailureTypedNullResult`, `TestFromConditionFailureOnThrow`, `TestTryGetFailureMixedOneFailureDifferentType`.
  `Typed` marks the `Result<T>` overload, `Mixed` a mixed-type input collection.
