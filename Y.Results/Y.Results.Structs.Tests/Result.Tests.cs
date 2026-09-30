namespace Y.Results.Structs.Tests;

public partial class ResultTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestTryGetFailedResultEmptyNoItems()
    {
        var success = Result.TryGetFailedResult(out var result);
        Assert.That(success, Is.False);
        Assert.That(result, Is.EqualTo(default(Result)));
    }

    [Test]
    public void TestTryGetFailedResultEmptyNoFailures()
    {
        var success = Result.TryGetFailedResult(out var result, Result.Success(), Result.Success());
        Assert.That(success, Is.False);
        Assert.That(result, Is.EqualTo(default(Result)));
    }

    [Test]
    public void TestTryGetFailedResultEmptyOneFailure()
    {
        var expectedResult = Result.Failure(new Exception("expected"));
        var success = Result.TryGetFailedResult(out var result, Result.Success(), Result.Success(), expectedResult);
        Assert.That(success, Is.True);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedResult.Exception));
    }

    [Test]
    public void TestTryGetFailedResultEmptyMultipleFailures()
    {
        var expectedResult = Result.Failure(new Exception("expected"));
        var success = Result.TryGetFailedResult(
            out var result,
            Result.Success(),
            expectedResult,
            Result.Success(),
            Result.Failure(new Exception()));
        Assert.That(success, Is.True);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedResult.Exception));
    }

    [Test]
    public void TestTryGetFailedResultTypedNoItems()
    {
        var success = Result.TryGetFailedResult(out var result, Array.Empty<Result<object>>());
        Assert.That(success, Is.False);
        Assert.That(result, Is.EqualTo(default(Result<object>)));
    }

    [Test]
    public void TestTryGetFailedResultTypedNoFailures()
    {
        var success = Result.TryGetFailedResult(out var result, Result.Success(1), Result.Success(2));
        Assert.That(success, Is.False);
        Assert.That(result, Is.EqualTo(default(Result<int>)));
    }

    [Test]
    public void TestTryGetFailedResultTypedOneFailure()
    {
        var expectedResult = Result.Failure<int>(new Exception("expected"));
        var success = Result.TryGetFailedResult(out var result, Result.Success(1), Result.Success(2), expectedResult);
        Assert.That(success, Is.True);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.EqualTo(0));
        Assert.That(result.Exception, Is.SameAs(expectedResult.Exception));
    }

    [Test]
    public void TestTryGetFailedResultTypedMultipleFailures()
    {
        var expectedResult = Result.Failure<int>(new Exception("expected"));
        var success = Result.TryGetFailedResult(
            out var result,
            Result.Success(1),
            expectedResult,
            Result.Success(2),
            Result.Failure<int>(new Exception()));
        Assert.That(success, Is.True);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedResult.Exception));
    }

    [Test]
    public void TestTryGetFailedResultMixedNoFailures()
    {
        var success = Result.TryGetFailedResult<int>(out var result, Result.Success(1), Result.Success());
        Assert.That(success, Is.False);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.EqualTo(0));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestTryGetFailedResultMixedOneFailureMatchingType()
    {
        var expectedResult = Result.Failure(new Exception("expected"));
        var success =
            Result.TryGetFailedResult<int>(out var result, Result.Success(1), Result.Success(), expectedResult);
        Assert.That(success, Is.True);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.EqualTo(0));
        Assert.That(result.Exception, Is.SameAs(expectedResult.Exception));
    }

    [Test]
    public void TestTryGetFailedResultMixedOneFailureDifferentType()
    {
        var expectedResult = Result.Failure<string>(new Exception("expected"));
        var success =
            Result.TryGetFailedResult<int>(out var result, Result.Success(1), Result.Success(), expectedResult);
        Assert.That(success, Is.True);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.EqualTo(0));
        Assert.That(result.Exception, Is.SameAs(expectedResult.Exception));
    }

    [Test]
    public void TestTryGetFailedResultMixedMultipleFailures()
    {
        var expectedResult = Result.Failure(new Exception("expected"));
        var success = Result.TryGetFailedResult<int>(
            out var result,
            Result.Success(),
            expectedResult,
            Result.Failure(new Exception()));
        Assert.That(success, Is.True);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedResult.Exception));
    }

    [Test]
    public void TestTryGetFailedResultMixedFailureWithoutException()
    {
        var expectedResult = Result.Failure(null!);
        var success = Result.TryGetFailedResult<object>(out var result, Result.Success(), expectedResult);
        Assert.That(success, Is.True);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception!.Message, Is.EqualTo("Operation failed."));
    }
}