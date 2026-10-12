# Failure handling (verified semantics)

## 1. Guards

- Required delegate arguments: `ArgumentNullException.ThrowIfNull(predicate | action | unsafeOperation | exception)`.
- Result arguments: `Result.AssertNotDefault(result)` - `internal static`, defined in
  `Result.Methods.cs`, with `[CallerArgumentExpression(nameof(result))] string? paramName` so the
  thrown `ArgumentNullException` reports the caller's argument name.
  - CLASSES variant: `if (result == null)` (reference null).
  - STRUCTS variant: `if (result == default)`; because `Result<T>` does not derive from `Result` there,
    a separate `internal static void AssertNotDefault<T>(Result<T> ...)` exists under `#if STRUCTS`.
  - Extension methods call it as `Result.AssertNotDefault(result)` - legal only because the core project
    grants `InternalsVisibleTo` to `$(AssemblyName).Extensions` and `$(AssemblyName).Web`.
- Every extension method that takes a result validates it before branching, including the "null result"
  negative tests (`TestOnSuccessNullResult`, `TestThenFuncNullResult`, `TestTryGetFailureEmptyContainsNull`).

## 2. Construction rules

- `Failure(Exception)` and `Failure<T>(Exception)` validate the exception first, then construct.
- `Failure<T>` stores `default!` as the value: a typed failure has no value by contract.
- Constructors are `internal` / `protected internal` - creation is only possible through the factories,
  from inside the assembly or from a satellite project listed in `InternalsVisibleTo`.

## 3. Exception conversion (the normal path)

Any overload that invokes caller-supplied code converts throwing into a failure result:

| API | Shape |
|---|---|
| `Wrap(Action)` / `Wrap(Func<Task>)` / `Wrap<T>` / `Wrap<T>(Func<Task<T>>)` | `try { op(); return Success(...); } catch (Exception e) { return Failure<T>(e); }` |
| `FromCondition(...)` | `predicate()` throws -> `Failure(e)`; `predicate()` false -> `Failure(EvalExceptionOrDefault(onFailure))` |
| `Then(... Func<Result> / Func<Result<TOut>> / Func<TIn, Result<TOut>> ...)` (sync and async) | `try { return action(...); } catch (Exception e) { return Failure<TOut>(e); }` |

`EvalExceptionOrDefault(onFailure)` synthesizes the default failure exception when the caller supplied
no `onFailure` factory: `new Exception(ConditionValidationHasFailedErrorMessage)` =
`"Condition validation has failed."`. Tests assert this message (`TestFromConditionFailure`,
`AssertConditionFailure` helper).

## 4. Preserving the stored exception across type changes

Never create a fresh exception when converting a failure:

- `Failure<TOut>(result.Exception!)` - untyped failure to typed failure.
- `CastFailure<TIn, TOut>()` (`private` in `ResultExtensions.cs`) - retypes a failure; when `TIn == TOut`
  it reinterprets the same value (`as Result<TOut>` in CLASSES, `Unsafe.As<...>` in STRUCTS), otherwise
  builds `Result.Failure<TOut>(result.Exception!)`.
- `WhenAll<T>` returns `Failure<IReadOnlyList<T>>(result.Exception!)` for the first failed input;
  `WhenAll<T1,T2>` / `WhenAll<T1,T2,T3>` return the first failing input's exception.
- `Validate` short-circuits on an existing failure (the predicate is never invoked for a failed
  result) and re-attaches the original instance with `.Then(result)`, so a successful validation returns
  the original result and a failed validation returns the newly created failure.

## 5. Deliberate, documented deviations

- `OnSuccess` / `OnFailure` (all four overloads) execute the callback through `Result.Wrap(...)` and
  **discard** the resulting failure, returning the original result unchanged. Their XML docs say the
  action result is swallowed and point to `Then` when the caller wants it returned. Do not "fix" this;
  choose `Then` when the failure must propagate.
- `ThrowIfFailed()` (`throw result.Exception!`) and `GetValueOrThrow<T>()` are the intentional escape
  hatches that turn a stored failure back into a thrown exception. They rethrow the *stored* exception,
  they do not wrap it.
- `ResultActionFilter` does not translate failures itself: on `!result.IsSuccess` it hands
  `result.Exception!` to the framework's registered `IExceptionHandler` (`TryHandleAsync`) so the
  application's own error handling owns the response.

## 6. Rules for new code

1. Guard first, then branch, then convert - never a nested `if` ladder.
2. Domain-level failure -> return `Failure`, do not throw.
3. Invalid programming input (null delegate, default result) -> throw `ArgumentNullException`.
4. A `catch` must either become a `Failure` carrying the caught exception, or rethrow. Never swallow
   silently in an API that returns a result.
5. Do not wrap or replace a stored exception when changing the result's value type.
6. If an API must throw, name it `ThrowIf*` or `*OrThrow` and implement it as an extension in the
   `.Extensions` project.
