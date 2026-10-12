# Public API surface (verified inventory)

Check this before adding anything - most composition already exists.

## `Y.Results.Classes` / `Y.Results.Structs` (namespace `Y.Results`)

| File | Members | Visibility |
|---|---|---|
| `IResult.cs` | `IResult` (`Exception`, `IsSuccess`, `Value`) | `internal` interface - used by `ResultActionFilter` via `InternalsVisibleTo` |
| `Result.cs` | `Result` type declaration, `Exception`, `IsSuccess`, `object? IResult.Value => null`, `Result(bool isSuccess, Exception? exception = null)` | type `public`, ctor `internal` |
| `Result.Generic.cs` | `Result<T>` declaration, `T Value`, `object? IResult.Value => Value`; CLASSES ctor `Result(bool, T, Exception?)`; STRUCTS `implicit operator Result(Result<T>)` | CLASSES ctor `protected internal`, STRUCTS ctor `internal` |
| `Result.Ctors.cs` | `Success()`, `Success<T>(T)`, `Failure(Exception)`, `Failure<T>(Exception)`, `FromCondition(Func<bool>, Func<Exception>?)`, `FromCondition<T>(T, Func<T,bool>, Func<T,Exception>?)`, `FromCondition<T>(T, Func<T,bool>, Func<Exception>?)` | `public static`; `EvalExceptionOrDefault` is `private static` |
| `Result.Wrap.cs` | `Wrap(Action)`, `Wrap(Func<Task>)`, `Wrap<T>(Func<T>)`, `Wrap<T>(Func<Task<T>>)` - all `public static`, all XML-documented | converts thrown exceptions into `Failure` |
| `Result.WhenAll.cs` | `WhenAll(params ReadOnlySpan<Result>)`, `WhenAll<T>(params ReadOnlySpan<Result<T>>) -> Result<IReadOnlyList<T>>`, `WhenAll<T1,T2>`, `WhenAll<T1,T2,T3>` -> `Result<Tuple<...>>` | `public static` |
| `Result.Methods.cs` | `TryGetFailure(out Result, params ReadOnlySpan<Result>)`, `TryGetFailure<T>(out Result<T>, params ReadOnlySpan<Result<T>>)`, `AssertNotDefault(Result, [CallerArgumentExpression])`, `AssertNotDefault<T>(Result<T>, ...)` (STRUCTS only) | `public static` / `internal static` |

Private constant: `private const string ConditionValidationHasFailedErrorMessage = "Condition validation has failed.";`
(`Result.Ctors.cs`) - the message given to a failure produced by a predicate that returned `false`
without an `onFailure` factory.

## `*.Extensions` projects (namespace `Y.Results`, type `public static partial class ResultExtensions`)

| File | Members |
|---|---|
| `ResultExtensions.cs` | `private static Result<TOut> CastFailure<TIn, TOut>(this Result<TIn>)` - retype a failure, preserving its exception (`as Result<TOut>` in CLASSES, `Unsafe.As` in STRUCTS) |
| `Result.On.cs` | `OnSuccess(Result, Action)`, `OnSuccess<T>(Result<T>, Action<T>)`, `OnFailure(Result, Action<Exception>)`, `OnFailure<T>(Result<T>, Action<Exception>)` - transparent filters, callback exceptions swallowed |
| `Result.Then.cs` | 10 sync `Then` overloads: chaining (`next`), `Action`, `Func<TOut>`, `Func<Result>`, `Func<Result<TOut>>`, `Func<TIn,TOut>`, `Func<TIn,Result<TOut>>`, `Func<TIn,Result>` |
| `Result.Then.Async.cs` | 7 `Task`-returning `Then` overloads (`Func<Task>`, `Func<TIn,Task>`, `Func<Task<Result>>`, `Func<TIn,Task<Result>>`, `Func<Task<Result<TOut>>>`, `Func<TIn,Task<TOut>>`, `Func<TIn,Task<Result<TOut>>>`) |
| `Result.Validations.cs` | `ThrowIfFailed()`, `GetValueOrThrow<T>()`, `Validate(Result, Func<bool>, Func<Exception>?)`, `Validate<T>(Result<T>, Func<T,bool>, Func<T,Exception>?)`, `Validate<T>(Result<T>, Func<T,bool>, Func<Exception>?)`, `ValidateNotNull<T>(Func<Exception>?)` |

`Validate` is implemented as composition of existing APIs, not new primitives:

```csharp
public static Result Validate(this Result result, Func<bool> predicate, Func<Exception>? onFailure = null)
{
    ArgumentNullException.ThrowIfNull(predicate);
    return result.Then(() => Result.FromCondition(predicate, onFailure)).Then(result);
}
```

Prefer composing existing overloads over adding a parallel primitive.

## `Y.Results.Classes.Web` / `Y.Results.Structs.Web` (namespace `Y.Results.Web`)

| File | Members |
|---|---|
| `ResultActionFilter.cs` | `public class ResultActionFilter(IExceptionHandler exceptionHandler) : IAsyncResultFilter` - unwraps `ObjectResult { Value: IResult }`; on failure delegates to the registered `IExceptionHandler`; on success rewrites the result (`FileContentResult` passthrough, `OkObjectResult`+null -> `OkResult`, `NoContentResult` only when `ReturnsNoContentOnSuccessAttribute` endpoint metadata is present). Private helper `CreateSuccessResult(...)` |
| `ReturnsNoContentOnSuccess.cs` | `public class ReturnsNoContentOnSuccessAttribute : Attribute;` - opt-in endpoint metadata |

There is no `AddResults()`-style DI helper yet; consumers register the filter themselves. If you add
one, put it in this project and link it in the struct twin.

## Conventions to reuse

- Variadic result parameters: `params ReadOnlySpan<Result>` / `params ReadOnlySpan<Result<T>>`.
- "Try" APIs: `bool Try*(out T, ...)` returning `false` and `default` output.
- Factory names are the same for the untyped and generic result (`Success`, `Failure`, `FromCondition`,
  `Wrap`, `WhenAll`), differing only by generic parameters.
