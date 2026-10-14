# Testing conventions

Framework: NUnit 4 with NUnit3TestAdapter, Moq for framework collaborators, Coverlet for coverage.
`Tests.props` is imported automatically for projects whose name ends in `.Tests`, and it injects a
global `using NUnit.Framework;` - test files never write that using themselves.

## Layout

- One test project per component and variant: `Y.Results.Classes.Tests`,
  `Y.Results.Classes.Extensions.Tests`, `Y.Results.Classes.Web.Tests` and their `Structs` twins.
- Test files mirror implementation files with a `.Tests` infix: `Result.Wrap.Tests.cs`,
  `Result.FromCondition.Tests.cs`, `Result.OnSuccess.Tests.cs`, `Result.Then.Async.Tests.cs`,
  `Result.Validations.Tests.cs`, `ResultExtensions.Tests.cs`, `ResultActionFilter.Tests.cs`.
- Test class mirrors the type under test and is `partial`, one chunk per aspect file:
  `partial class ResultTests`, `partial class ResultExtensionsTests`. A single-file suite may be
  non-partial (`ResultActionFilterTests`).
- Test namespaces: `Y.Results.Tests`, `Y.Results.Extensions.Tests`, `Y.Results.Web.Tests`.
- A new test file must be added to the `Structs` test `.csproj` `<Compile Include>` list, exactly like
  production files, otherwise only one variant runs it.

## Test method shape

```csharp
namespace Y.Results.Tests;

public partial class ResultTests
{
    [Test]
    public void TestFromConditionFailureCustomException()
    {
        var expectedException = new Exception();
        var result = Result.FromCondition(() => false, () => expectedException);

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }
}
```

- Naming: `Test<Subject><Scenario><ExpectedOutcome>`; `Typed` = the `Result<T>` overload,
  `Async` = async overload, `Mixed` = mixed-type input collection. Examples:
  `TestThenValueResultMapperAsyncThrowingMapper`, `TestOnFailureTypedNullAction`,
  `TestTryGetFailureMixedOneFailureDifferentType`.
- Arrange variables are named `expectedResult` / `expectedException` / `input`.
- Async cases are `public async Task Test...()`; sync cases are `public void Test...()`.
- Every public overload gets a small matrix of cases: null argument, success input, failure input,
  throwing delegate, and (where relevant) the "typed" variant. Existing suites cover 72 core,
  129 extension and 8 web tests per variant.

## Assertions

- Constraint model only: `Assert.That(actual, Is.True / Is.False / Is.Null / Is.Default / Is.Not.Null /
  Is.EqualTo(x) / Is.SameAs(x) / Is.TypeOf<T>() / Is.EqualTo("message"))`,
  `Assert.Throws<ArgumentNullException>(...)`, `Assert.DoesNotThrow(...)`.
- Reference vs value equality is variant-dependent, and tests branch accordingly:
  ```csharp
  #if CLASSES
          Assert.That(result, Is.SameAs(expectedResult));
  #elif STRUCTS
          Assert.That(result, Is.EqualTo(expectedResult));
  #endif
  ```
- `Is.Default` asserts the "no result" contract for failed `Try*` out-parameters.

## Shared fixtures and helpers

- Common expectation declared once per partial class:
  `private Exception ExpectedException { get; } = new("Expected exception.");`
- `[SetUp] public void Setup() { }` is declared as boilerplate in `ResultTests` and
  `ResultExtensionsTests`; keep it if the class already has one, do not add it to new classes that do
  not need setup.
- Repeated assertion groups become private static helpers instead of being copy-pasted, e.g.
  ```csharp
  private static void AssertConditionFailure(Result result)
  {
      Assert.That(result.IsSuccess, Is.False);
      Assert.That(result.Exception, Is.Not.Null);
      Assert.That(result.Exception, Is.TypeOf<Exception>());
      Assert.That(result.Exception!.Message, Is.EqualTo("Condition validation has failed."));
  }
  ```
- ASP.NET tests build their contexts with Moq (`Mock<IExceptionHandler>`, `ResultExecutingContext`,
  `HttpContext`, `Endpoint` metadata) rather than a test host.

## Variant pitfalls

- `Y.Results.Classes.Web.Tests` does not define `CLASSES`. A `#if CLASSES` assert written in a web test
  file is compiled out in that project and only the `STRUCTS` twin would exercise the other branch.
  Keep web asserts variant-agnostic, or add `<DefineConstants>$(DefineConstants);CLASSES</DefineConstants>`
  to that project.
- Struct equality is value equality; class equality is reference equality. Assert accordingly.

## Running

```bash
dotnet test Y.Results.sln                                  # both variants, both TFMs
dotnet test Y.Results/Y.Results.Classes.Extensions.Tests -c Debug   # one suite while iterating
```
