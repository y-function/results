# Principles as actually implemented - and where the repo deviates

Only claims that are demonstrable from the source are listed. Do not restate a principle in a code
comment unless the code shows it.

## SOLID

- **Single Responsibility:** `Result`/`Result<T>` own success, failure and the stored exception only
  (`Result.cs` has three members). Composition lives in extension methods in a separate assembly;
  HTTP translation lives in `.Web`.
- **Open/Closed:** fluent behaviour is added by new `ResultExtensions` partial files in the satellite
  project - the core type is never edited to add a chain operation. `Validate` composes `Then` +
  `FromCondition` instead of adding a core primitive.
- **Liskov Substitution:** *variant-specific.* In the CLASSES build `Result<T> : Result`, so a generic
  result is substitutable for a plain one (`TryGetFailure` accepts `Result<T>` items after `Failure<string>`
  in the mixed test). In the STRUCTS build `Result<T>` is an independent `record struct` and the
  relationship is replaced by `public static implicit operator Result(Result<T>)`, which is what keeps
  that same test code compiling in the struct build (`TestTryGetFailureMixedOneFailureDifferentType`
  passes `Result<int>` and `Result<string>` into `params ReadOnlySpan<Result>` in both variants).
  Write code that works under both: never depend on `Result<T>` being assignable to `Result` without
  an implicit conversion.
- **Interface Segregation:** the shared seam `IResult` (`Exception`, `IsSuccess`, `Value`) is
  `internal` and exists only so `ResultActionFilter` can pattern-match `ObjectResult { Value: IResult }`
  without referencing a concrete type. It is not part of the public API; do not widen it.
- **Dependency Inversion:** the filter depends on the framework abstraction `IExceptionHandler`, not on
  a concrete error-response type; core result semantics never reference ASP.NET.

## KISS / YAGNI

- Two states (`IsSuccess` + `Exception`) and explicit branching, no error codes, no result hierarchy,
  no `Ok`/`NotFound`/`ValidationError` subtypes.
- `WhenAll` ships untyped, list, 2-arity and 3-arity only; higher arity is composed by the caller.
- No DI registration helper exists yet; the filter is registered by the consumer. Adding one is a real
  feature, not a free convenience - keep the surface minimal.

## DRY

- One `Failure`/`Success` pair used by every other factory.
- `TryGetFailure` is the single failure-scan implementation reused by `WhenAll(params)`.
- `AssertNotDefault` is the single validation helper reused by every extension method.
- `CastFailure` is the single typed-failure conversion reused by the `Then` overloads.
- `FromCondition` typed overload with `Func<Exception>` forwards to the `Func<T, Exception>` overload.
- `ValidateNotNull` forwards to `Validate`.
- Test-side DRY: `AssertConditionFailure` helper, shared `ExpectedException` property, and one test
  source compiled into both variants.

## Deliberate trade-offs (understood, not accidents)

- Overload width of `Then` (10 sync + 7 async) is the cost of a fluent API; the repo compensates with
  per-overload tests and `#region`-free, one-aspect-per-file organization. If a new overload makes the
  set harder to reason about, prefer composing existing overloads (as `Validate` does).
- `OnSuccess`/`OnFailure` swallow callback exceptions - documented in their XML docs with a pointer to
  `Then`. This is an observation API, not a composition API.
- Nested ternaries in `WhenAll<T1,T2>` / `WhenAll<T1,T2,T3>` are accepted because each level has the
  identical shape; do not extend that style to asymmetric logic.
- `GenerateDocumentationFile` is `false` while public API still carries XML docs.

## Known deviations to avoid copying

- `Y.Results.Structs.Extensions` sets `RootNamespace` to `Y.Results.Extensions` while
  `Y.Results.Classes.Extensions` sets `Y.Results`; the files declare `namespace Y.Results;`, so the
  extension API currently lands in `Y.Results` in both variants. Declare namespaces explicitly.
- `Y.Results.Classes.Web.Tests` has no `CLASSES` constant while its struct twin has `STRUCTS`.
- File `ReturnsNoContentOnSuccess.cs` declares `ReturnsNoContentOnSuccessAttribute`.
- Dead build output folders (`Y.Results/Benchmark`, `Y.Results/Y.Results`, `Y.Results/Y.Results.Tests`)
  remain on disk with no project files; the current layout is `Y.Results.<Variant>[.<Layer>][.Tests]`.
- `Package.props` is never imported by the local build; versioning only materializes in CI.
