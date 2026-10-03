namespace Y.Results.Tests;

public partial class ResultTests
{
    private Exception ExpectedException { get; } = new("Expected exception.");

    [SetUp]
    public void Setup() { }

    [Test]
    public void TestTryGetFailureEmptyNoItems()
    {
        var success = Result.TryGetFailure(out var result);
        Assert.That(success, Is.False);
        Assert.That(result, Is.Default);
    }

    [Test]
    public void TestTryGetFailureEmptyNoFailures()
    {
        var success = Result.TryGetFailure(out var result, Result.Success(), Result.Success());
        Assert.That(success, Is.False);
        Assert.That(result, Is.Default);
    }

    [Test]
    public void TestTryGetFailureEmptyContainsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.TryGetFailure(out var result, Result.Success(), default!));
    }

    [Test]
    public void TestTryGetFailureEmptyOneFailure()
    {
        var expectedResult = Result.Failure(new Exception("expected"));
        var success = Result.TryGetFailure(out var result, Result.Success(), Result.Success(), expectedResult);
        Assert.That(success, Is.True);
#if CLASSES
        Assert.That(result, Is.SameAs(expectedResult));
#elif STRUCTS
        Assert.That(result, Is.EqualTo(expectedResult));
#endif
    }

    [Test]
    public void TestTryGetFailureEmptyMultipleFailures()
    {
        var expectedResult = Result.Failure(new Exception("expected"));
        var success = Result.TryGetFailure(out var result, Result.Success(), expectedResult, Result.Success(), Result.Failure(new Exception()));
        Assert.That(success, Is.True);
#if CLASSES
        Assert.That(result, Is.SameAs(expectedResult));
#elif STRUCTS
        Assert.That(result, Is.EqualTo(expectedResult));
#endif
    }

    [Test]
    public void TestTryGetFailureTypedNoItems()
    {
        var success = Result.TryGetFailure(out var result, Array.Empty<Result<object>>());
        Assert.That(success, Is.False);
        Assert.That(result, Is.Default);
    }

    [Test]
    public void TestTryGetFailureTypedNoFailures()
    {
        var success = Result.TryGetFailure(out var result, Result.Success(1), Result.Success(2));
        Assert.That(success, Is.False);
        Assert.That(result, Is.Default);
    }

    [Test]
    public void TestTryGetFailureTypedOneFailure()
    {
        var expectedResult = Result.Failure<int>(new Exception("expected"));
        var success = Result.TryGetFailure(out var result, Result.Success(1), Result.Success(2), expectedResult);
        Assert.That(success, Is.True);
#if CLASSES
        Assert.That(result, Is.SameAs(expectedResult));
#elif STRUCTS
        Assert.That(result, Is.EqualTo(expectedResult));
#endif
    }

    [Test]
    public void TestTryGetFailureTypedMultipleFailures()
    {
        var expectedResult = Result.Failure<int>(new Exception("expected"));
        var success = Result.TryGetFailure(out var result, Result.Success(1), expectedResult, Result.Success(2), Result.Failure<int>(new Exception()));
        Assert.That(success, Is.True);
#if CLASSES
        Assert.That(result, Is.SameAs(expectedResult));
#elif STRUCTS
        Assert.That(result, Is.EqualTo(expectedResult));
#endif
    }

    [Test]
    public void TestTryGetFailureMixedNoFailures()
    {
        var success = Result.TryGetFailure<int>(out var result, Result.Success(1), Result.Success(3));
        Assert.That(success, Is.False);
        Assert.That(result, Is.Default);
    }

    [Test]
    public void TestTryGetFailureMixedOneFailureMatchingType()
    {
        var expectedResult = Result.Failure<int>(new Exception("expected"));
        var success = Result.TryGetFailure<int>(out var result, Result.Success(1), Result.Success(3), expectedResult);
        Assert.That(success, Is.True);
#if CLASSES
        Assert.That(result, Is.SameAs(expectedResult));
#elif STRUCTS
        Assert.That(result, Is.EqualTo(expectedResult));
#endif
    }

    [Test]
    public void TestTryGetFailureMixedOneFailureDifferentType()
    {
        var expectedResult = Result.Failure<string>(new Exception("expected"));
        var success = Result.TryGetFailure(out var result, Result.Success(1), Result.Success(1), expectedResult);
        Assert.That(success, Is.True);
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedResult.Exception));
    }

    [Test]
    public void TestTryGetFailureMixedMultipleFailures()
    {
        var expectedResult = Result.Failure<int>(new Exception("expected"));
        var success = Result.TryGetFailure<int>(out var result, Result.Success(2), expectedResult, Result.Failure<int>(new Exception()));
        Assert.That(success, Is.True);
#if CLASSES
        Assert.That(result, Is.SameAs(expectedResult));
#elif STRUCTS
        Assert.That(result, Is.EqualTo(expectedResult));
#endif
    }
}