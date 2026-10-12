# Pre-change self-review

## Both variants (highest risk - check first)

- [ ] New/renamed `.cs` file is listed in the `Structs` twin `.csproj` with `<Compile Include>` + `<Link>`.
- [ ] New test file is listed in the `Structs` twin *test* `.csproj` the same way.
- [ ] Every partial type declaration has both `#if CLASSES` and `#elif STRUCTS` branches.
- [ ] No new file was created just to hold a variant difference; the difference is an `#if`/`#elif` block.
- [ ] Code that differs per variant does not rely on `Result<T> : Result` (true only in CLASSES) or on
      reference-null checks (only `== default` works in STRUCTS).
- [ ] `dotnet build Y.Results.sln` passes for `net8.0` **and** `net10.0` with zero warnings.

## Placement

- [ ] Core logic in `Y.Results.Classes` (+ `Structs`), fluent API in `*.Extensions`, ASP.NET behaviour in
      `*.Web`, tests in `*.Tests`.
- [ ] No ASP.NET/`Microsoft.AspNetCore.*` reference introduced outside a `*.Web` project.
- [ ] No new public constructor; creation goes through `Success` / `Failure` / `FromCondition` / `Wrap` / `WhenAll`.
- [ ] An internal helper that satellite projects need is exposed via `InternalsVisibleTo`, not made public.
- [ ] Existing API was checked in `api-surface.md`; composition preferred over a new primitive.

## Naming

- [ ] File follows `<Subject>.<Aspect>.cs`, async in its own `<Aspect>.Async.cs`.
- [ ] Namespace declared explicitly (`Y.Results`, `Y.Results.Web`, `Y.Results.Tests`,
      `Y.Results.Extensions.Tests`, `Y.Results.Web.Tests`).
- [ ] Type PascalCase; verb-first method reusing an existing verb (`Then`, `On*`, `Wrap`, `WhenAll`,
      `Try*`, `Validate*`, `ThrowIf*`, `*OrThrow`); `Assert*` for internal validators.
- [ ] Generics: `T` for the value, `TIn`/`TOut` for a transformation, `T1..T3` for arity overloads.
- [ ] Parameters/locals camelCase and role-naming (`unsafeOperation`, `onFailure`, `predicate`, `next`).
- [ ] `private const` PascalCase for fixed messages.
- [ ] Test class `partial class <Type>Tests`; test method `Test<Subject><Scenario><ExpectedOutcome>`.

## Failure semantics

- [ ] `ArgumentNullException.ThrowIfNull(...)` for every caller-supplied delegate argument.
- [ ] `Result.AssertNotDefault(result)` for every `Result` / `Result<T>` argument.
- [ ] Guard clauses precede the operation; single early `if (!result.IsSuccess) return result;`.
- [ ] A `catch` converts to `Failure(...)` carrying the caught exception, or rethrows - never silent.
- [ ] Type conversions preserve the stored exception (`Failure<TOut>(result.Exception!)`, `CastFailure`).
- [ ] Swallowing a callback exception happens only in an `On*` observer, and is documented in XML docs.
- [ ] A throwing escape hatch is named `ThrowIf*` / `*OrThrow` and lives in the extensions project.

## Tests

- [ ] Case matrix per overload: null argument, success input, failure input, throwing delegate, typed variant.
- [ ] `Assert.That(actual, Is.*)` constraint style; `Assert.Throws<T>` for guard clauses.
- [ ] Reference-vs-value equality handled with `#if CLASSES` / `#elif STRUCTS`.
- [ ] Repeated assertion groups extracted into a private static helper.
- [ ] `dotnet test Y.Results.sln` green in both variants and both TFMs.

## Simplicity

- [ ] No overload added that composition could already express.
- [ ] Nested ternaries only for symmetric branching, at most two levels.
- [ ] `params ReadOnlySpan<...>` + `foreach (ref readonly ...)` used for variadic result inputs.
- [ ] No abstraction added beyond the success/failure workflow (no new result subtypes, no error codes).
