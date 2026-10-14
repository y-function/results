# C# style as actually written in this solution

## File skeleton

```csharp
using System.Runtime.CompilerServices;   // only when the file needs it

namespace Y.Results;                     // file-scoped, declared explicitly

#if CLASSES
public partial class Result
#elif STRUCTS
public readonly partial record struct Result
#endif
{
    ...
}
```

- File-scoped namespaces everywhere.
- Extension classes: `public static partial class ResultExtensions`, no `#if` needed because they are
  variant-agnostic unless a body needs it (`CastFailure`).
- `using` directives only at the top, only what is used (`System.Runtime.CompilerServices` for
  `[CallerArgumentExpression]` / `[MethodImpl]`, the ASP.NET namespaces in the Web project).

## Member ordering inside a type

1. `#if`/`#elif` variant declaration
2. constants (`private const`) and properties / fields
3. constructors and factory methods
4. public static API (grouped by aspect, sync before async)
5. `internal` helpers shared with satellite projects
6. `private` helpers last

Examples: `Result.Ctors.cs` = const -> factories -> `private static EvalExceptionOrDefault`;
`Result.Methods.cs` = public `TryGetFailure` -> `internal AssertNotDefault`;
`ResultActionFilter.cs` = interface method -> private `HandleFailureAsync` -> private static
`CreateSuccessResult`.

## Expression bodies vs blocks

Expression bodies (`=>`) are the default for single-expression members: `Success`, `WhenAll` untyped,
`CastFailure`, `ValidateNotNull`, the `Validate` overload that forwards, one-line types
(`public class ReturnsNoContentOnSuccessAttribute : Attribute;`).

Use a block when there are guards, a `try`/`catch`, or multiple statements - all factories with
validation, every `Wrap` overload, every `Then` overload that invokes a delegate.

## Control flow

- Guard clauses first, one per line:
  ```csharp
  Result.AssertNotDefault(result);
  ArgumentNullException.ThrowIfNull(action);
  ```
- Early return of the unchanged result for the "not my case" path:
  ```csharp
  if (!result.IsSuccess)
      return result;
  ```
- Single-expression branching for symmetric choices: `result.IsSuccess ? next : result`.
- `try { ... } catch (Exception e) { return Failure(e); }` is the standard shape for any overload that
  invokes a caller-supplied delegate that may throw.
- Nested ternaries are used only for symmetric multi-arity branching (`WhenAll<T1,T2>`, `WhenAll<T1,T2,T3>`)
  where each level has the same shape. Beyond two levels, write a loop or a helper instead.
- Variadic loops use `params ReadOnlySpan<...>` + `foreach (ref readonly var result in results)` to
  avoid copying struct results.

## Type and null handling

- Nullable reference types are enabled; public API expresses optionality with `Exception?` and
  `Func<Exception>?`, and `default!` / `!` is used where an invariant guarantees non-null
  (`result.Exception!` on a failure, `default!` for the out-parameter of a failed `TryGetFailure`).
- Null-check required delegate arguments explicitly; do not rely on the runtime to throw.
- Explicit type arguments at failure-conversion sites so the target type is obvious:
  `Failure<IReadOnlyList<T>>(result.Exception!)`, `Failure<Tuple<T1, T2>>(...)`.
- Optional delegate parameters get `= null` defaults; where overload resolution would be ambiguous the
  repo adds a deliberately non-defaulted overload (`Validate<T>(Result<T>, Func<T,bool>, Func<Exception>?)`)
  and forwards to the primary one.
- `[MethodImpl(MethodImplOptions.AggressiveInlining)]` on hot internal validation helpers.

## Conditional compilation rules

- Type declarations: always `#if CLASSES` + `#elif STRUCTS`, both branches present.
- Member bodies that differ: `#if CLASSES ... #elif STRUCTS ... #endif`
  (`CastFailure`, `AssertNotDefault`, `Result.Generic.cs` ctor vs implicit operator).
- A struct-only member is wrapped in `#if STRUCTS ... #endif` (that is the one accepted case without an
  `#elif CLASSES`) - see `AssertNotDefault<T>`.
- Never introduce a third constant or a new file just to hold a variant difference.

## Documentation

- Public factory/filter behavior is documented with `///` XML comments using `<paramref/>`,
  `<see cref="..."/>`, `<value>` and inline `<code>` blocks (see `Result.Wrap.cs`, `Result.On.cs`,
  `Result.Validations.cs`). Document *which* exceptions are converted and which are swallowed.
- `GenerateDocumentationFile` is `false` in `Directory.Build.props`, so docs are for readers and tool
  tooltips, not for a generated `.xml`.

## Analyzer posture

`AnalysisMode` Default, `EnforceCodeStyleInBuild` true, `CodeAnalysisTreatWarningsAsErrors` false. There
is no `.editorconfig`; the current build is warning-free, and new code must keep it that way.
