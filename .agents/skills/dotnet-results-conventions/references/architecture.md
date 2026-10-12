# Project layout and build architecture

Verified against the solution at commit `81dc2b5`. All projects sit in the `Y.Results/` folder and
are grouped in the solution folders `Classes`, `Structs`, `Tests`, `Packages`.

## Two variants of the same code

The library ships twice: a **class-based** variant and a **struct-based** variant. They do not have
separate sources. The `Structs` projects link the `Classes` files and compile them with a different
preprocessor constant.

```xml
<!-- Y.Results/Y.Results.Structs/Y.Results.Structs.csproj -->
<PropertyGroup>
  <RootNamespace>Y.Results</RootNamespace>
  <DefineConstants>$(DefineConstants);STRUCTS</DefineConstants>
</PropertyGroup>
<ItemGroup>
  <Compile Include="..\Y.Results.Classes\Result.Wrap.cs">
    <Link>Result.Wrap.cs</Link>
  </Compile>
  <!-- ... every other core file ... -->
</ItemGroup>
```

Consequences:

- A new `.cs` file in a `Classes*` project is invisible to the `Structs*` build until it is added to
  that project's `<Compile Include>` list with a matching `<Link>`.
- Never create a struct-only source file; variant differences are expressed with
  `#if CLASSES` / `#elif STRUCTS` inside the shared file.
- The same applies to tests: `Y.Results.Structs.Tests`, `Y.Results.Structs.Extensions.Tests`,
  `Y.Results.Structs.Web.Tests` link the test files of their `Classes` twins.

## Projects

| Project | RootNamespace | Constant | Notes |
|---|---|---|---|
| `Y.Results.Classes` | `Y.Results` | `CLASSES` | Core: `Result`, `Result<T>`, `IResult` |
| `Y.Results.Classes.Extensions` | `Y.Results` | `CLASSES` | `ResultExtensions` partials, references core |
| `Y.Results.Classes.Web` | `Y.Results.Web` | `CLASSES` | ASP.NET integration, `FrameworkReference Microsoft.AspNetCore.App` |
| `Y.Results.Classes.Tests` | `Y.Results.Tests` | `CLASSES` | Core tests |
| `Y.Results.Classes.Extensions.Tests` | `Y.Results.Extensions.Tests` | `CLASSES` | Extension tests |
| `Y.Results.Classes.Web.Tests` | `Y.Results.Web.Tests` | *(none)* | Filter tests (Moq). Has no `DefineConstants` - see pitfall below |
| `Y.Results.Structs` | `Y.Results` | `STRUCTS` | Links all 7 core source files |
| `Y.Results.Structs.Extensions` | `Y.Results.Extensions` | `STRUCTS` | Links all 5 extension files |
| `Y.Results.Structs.Web` | `Y.Results.Web` | `STRUCTS` | Links both web files |
| `Y.Results.Structs.Tests` | `Y.Results.Tests` | `STRUCTS` | Links all 5 core test files |
| `Y.Results.Structs.Extensions.Tests` | `Y.Results.Extensions.Tests` | `STRUCTS` | Links all 6 extension test files |
| `Y.Results.Structs.Web.Tests` | `Y.Results.Web.Tests` | `STRUCTS` | Links the filter test file |

The namespaces the compiler actually sees are declared in the files: `Y.Results` (core and
extensions), `Y.Results.Web`, `Y.Results.Tests`, `Y.Results.Extensions.Tests`, `Y.Results.Web.Tests`.

### Pitfalls found in the current `.csproj` set

- `Y.Results.Classes.Extensions` sets `RootNamespace` to `Y.Results` while
  `Y.Results.Structs.Extensions` sets it to `Y.Results.Extensions`. Harmless today because every file
  declares `namespace Y.Results;` explicitly, but do not rely on `RootNamespace` for the namespace of
  new files - declare it.
- `Y.Results.Classes.Web.Tests` defines no `CLASSES` constant. A `#if CLASSES` written in a web test
  file compiles to nothing in that project. Keep web tests variant-agnostic, or add the constant.

## Shared MSBuild configuration

`Directory.Build.props` (applies to all projects):

- `TargetFrameworks`: `net8.0;net10.0` - every project must compile for both.
- `Nullable` enable, `LangVersion` latest, `ImplicitUsings` enable.
- `AnalysisMode` Default, `EnforceCodeStyleInBuild` true, `CodeAnalysisTreatWarningsAsErrors` false,
  `GenerateDocumentationFile` false (XML docs are still written on public API).
- Detects test projects by name suffix (`.Tests`, `.Mocks`, `.UnitTests`) and imports `Tests.props`.
- Adds `InternalsVisibleTo("$(AssemblyName).Tests")` to every non-test project.
- Adds `Microsoft.CodeAnalysis.NetAnalyzers`.

Core projects additionally add `InternalsVisibleTo` for `$(AssemblyName).Extensions` and
`$(AssemblyName).Web`, which is how `ResultExtensions` and `ResultActionFilter` can call
`Result.AssertNotDefault` and read the internal `IResult`. Keep this mechanism for internal helpers;
do not widen visibility to the outside.

`Tests.props` (imported for test projects): NUnit 4, NUnit3TestAdapter, Moq, coverlet.collector and
coverlet.msbuild, Microsoft.NET.Test.Sdk, and a global `Using NUnit.Framework` - test files therefore
contain no `using NUnit.Framework;`.

`Package.props` defines version properties from `$(MasterVersion)`, supplied by CI. It is *not*
imported by `Directory.Build.props`; packaging runs through the reusable workflow
`y-function/workflows/.github/workflows/dotnet-package.yml` (`.github/workflows/dotnet.yml`).

## Stale folders - do not touch

`Y.Results/Benchmark`, `Y.Results/Y.Results` and `Y.Results/Y.Results.Tests` exist on disk but contain
only `bin`/`obj` leftovers from an earlier layout. They have no `.csproj` and are not in
`Y.Results.sln`. Never edit, reference, or "fix" them. (`[Bb]in/` and `[Oo]bj/` are git-ignored.)

## Verification commands

```bash
dotnet build Y.Results.sln     # currently: 12 projects, 0 warnings, 0 errors
dotnet test Y.Results.sln      # core 72, extensions 129, web 8 tests per variant per TFM
```
