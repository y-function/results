---
name: dotnet-results-conventions
description: Coding conventions for the Y.Results .NET solution (Result / Result<T> pattern, built twice as CLASSES and STRUCTS). Use when adding, refactoring, or reviewing C# here - result factories, Wrap/WhenAll/TryGetFailure, Then/On/Validate extension methods, the ASP.NET action filter, partial files, or NUnit tests - so new code matches this repository's naming, dual-variant file linking, failure-handling, and test layout.
license: MIT
compatibility: .NET SDK 10 (global.json), projects target net8.0 and net10.0. Needs only the dotnet CLI.
metadata:
  scope: repository
  area: dotnet, csharp, result-pattern
  verified-against: commit 81dc2b5
---

# Y.Results repository conventions

Apply these rules to every change in this solution. The details live in `references/`
(relative to this skill directory); read them before structural work.

## Non-negotiables

1. **Every source file is compiled twice.** The `Y.Results.Structs*` projects do not have their own
   copies - they link the `Y.Results.Classes*` files through `<Compile Include>` + `<Link>` in the
   `.csproj` and compile them with `STRUCTS` instead of `CLASSES`. A new or renamed `.cs` file must be
   added to the matching `Structs*` `.csproj` as well, or the struct variant silently omits it.
   Test files follow the same rule. See `references/architecture.md`.
2. **Every partial file declares both variants at the top.**
   ```csharp
   #if CLASSES
   public partial class Result
   #elif STRUCTS
   public readonly partial record struct Result
   #endif
   ```
   Variant-specific *bodies* use `#if CLASSES` / `#elif STRUCTS` only - never a bare `#if CLASSES`
   without an `#elif STRUCTS` branch (the struct build would compile nothing).
3. **Results are created only through factories** (`Success`, `Failure`, `FromCondition`, `Wrap`,
   `WhenAll`). Core constructors are `internal` / `protected internal` and are reachable from the
   satellite projects only via `InternalsVisibleTo`. Do not add public constructors.
4. **Guards come first.** `ArgumentNullException.ThrowIfNull(...)` for delegate arguments and
   `Result.AssertNotDefault(result)` for `Result`/`Result<T>` arguments (internal helper, throws
   `ArgumentNullException` named by `[CallerArgumentExpression]`).
5. **Never lose a stored exception.** Converting between typed and untyped results must carry
   `.Exception!` over (`Failure<TOut>(result.Exception!)`, `CastFailure<TIn, TOut>`, `WhenAll`, `Then`).
   A `catch` either becomes a `Failure` or rethrows. Two deliberate, documented exceptions to this:
   `OnSuccess`/`OnFailure` discard the callback exception, and `ThrowIfFailed`/`GetValueOrThrow`
   rethrow the stored one. See `references/failure-handling.md`.
6. **Layer boundaries are fixed.** Core has no ASP.NET reference; only `*.Web` projects may use
   `FrameworkReference Include="Microsoft.AspNetCore.App"`. Fluent APIs live in `ResultExtensions`
   in the satellite `.Extensions` project, so the core type is never modified to add one.
7. **Tests mirror implementation.** `<Aspect>.Tests.cs` files hold `partial class <Type>Tests` in a
   `.Tests` project, and are linked into the struct twin. New public API gets tests in both variants.
   See `references/testing.md`.
8. **Verify both variants.** Run before finishing:
   ```bash
   dotnet build Y.Results.sln          # expect 0 warnings, 0 errors
   dotnet test Y.Results.sln           # 12 test assemblies: 2 variants x 3 layers x 2 TFMs
   ```

## Decision guide

| Task | Do this |
|---|---|
| New fluent operation on a result | Add a `public static` method to `ResultExtensions` in a `Result.<Aspect>.cs` partial file in `Y.Results.Classes.Extensions` (split sync/async files), then link it in `Y.Results.Structs.Extensions.csproj` |
| New static/factory on `Result` | Add to the matching `Result.<Aspect>.cs` partial in `Y.Results.Classes` and link it in `Y.Results.Structs.csproj` |
| Needs a value-returning failure | `Result.Failure<T>(exception)` (stores `default` value), or `CastFailure<TIn, TOut>()` to retype an existing failure |
| Needs a variadic result collection parameter | `params ReadOnlySpan<Result>` + `foreach (ref readonly var result in results)` |
| Behavior differs per variant | `#if CLASSES ... #elif STRUCTS ... #endif` in the same file (see `AssertNotDefault`, `CastFailure`, `Result.Generic.cs`) |
| Caller must be able to throw | Add an escape hatch extension (`ThrowIfFailed`, `GetValueOrThrow`) rather than making a factory throw |
| ASP.NET behaviour change | Only in `Y.Results.Classes.Web` (`ResultActionFilter`, `ReturnsNoContentOnSuccessAttribute`); opt-in via endpoint metadata, never by touching core types |
| Unsure which API already exists | Check `references/api-surface.md` first - most composition already exists (`Validate` is built from `Then`) |

## References

| File | Contents |
|---|---|
| `references/api-surface.md` | Verified inventory of every type, method and their visibility |
| `references/architecture.md` | The 12 projects, namespaces, constants, linked-file compilation, build props |
| `references/naming.md` | Project, file, type, member, generic-parameter and test naming |
| `references/code-style.md` | Observed C# style, member ordering, conditional-compilation rules |
| `references/failure-handling.md` | Guards, exception preservation, deliberate swallow/rethrow cases |
| `references/testing.md` | NUnit conventions, test layout, assertions, variant-sensitive asserts |
| `references/principles.md` | SOLID/KISS/DRY as actually implemented, plus known repo deviations |
| `references/checklist.md` | Pre-commit self-review |

## Workflow

1. Read `references/api-surface.md`; extend an existing partial instead of adding a new aspect file.
2. Implement in the `Classes` variant source files; add the `<Compile Include>` entry to the
   `Structs` twin project.
3. Write guards, then the operation, then convert failures - never a nested conditional block.
4. Add tests mirroring the file name, in the `Classes*.Tests` project (the struct twin links them).
5. `dotnet build Y.Results.sln` then `dotnet test Y.Results.sln`; keep the build warning-free.
6. Self-review against `references/checklist.md`.
