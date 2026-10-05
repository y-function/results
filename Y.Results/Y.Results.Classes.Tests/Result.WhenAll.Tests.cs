using NUnit.Framework.Legacy;

namespace Y.Results.Tests;

public partial class ResultTests
{
    [Test]
    public void TestWhenAllNullArgTreatedAsEmptyArray()
    {
        var output = Result.WhenAll(null);
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.True);
    }

    [Test]
    public void TestWhenAllNoArgs()
    {
        var output = Result.WhenAll();
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.True);
    }

    [Test]
    public void TestWhenAllOneArgSuccess()
    {
        var output = Result.WhenAll(Result.Success());
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.True);
    }

    [Test]
    public void TestWhenAllOneArgFailure()
    {
        var output = Result.WhenAll(Result.Failure(ExpectedException));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestWhenAllContainsFailure()
    {
        var output = Result.WhenAll(Result.Success(), Result.Success(), Result.Success(), Result.Failure(ExpectedException));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestWhenAllContainsDefault()
    {
        Assert.Throws<ArgumentNullException>(() => Result.WhenAll(Result.Success(), Result.Success(), Result.Success(), default!));
    }

    [Test]
    public void TestWhenAllTypedNullArgTreatedAsEmptyArray()
    {
        var output = Result.WhenAll<object>(null);
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.Empty);
    }

    [Test]
    public void TestWhenAllTypedNoArgsTreatedAsEmptyArray()
    {
        var output = Result.WhenAll<object>();
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.Empty);
    }

    [Test]
    public void TestWhenAllTypedOneArgSuccess()
    {
        var expectedValue = 1;
        var output = Result.WhenAll(Result.Success(expectedValue));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.Not.Empty);
        CollectionAssert.AreEquivalent(output.Value, new[] { expectedValue });
    }

    [Test]
    public void TestWhenAllTypedOneArgFailure()
    {
        var output = Result.WhenAll(Result.Failure<int>(ExpectedException));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestWhenAllTypedContainsFailure()
    {
        var output = Result.WhenAll(Result.Success(), Result.Success(), Result.Success(), Result.Failure(ExpectedException));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestWhenAllTypedContainsDefault()
    {
        Assert.Throws<ArgumentNullException>(() => Result.WhenAll(Result.Success(), Result.Success(), default!));
    }

    [Test]
    public void TestWhenAllTupleContainsFailure()
    {
        var output = Result.WhenAll(Result.Success(1), Result.Failure<string>(ExpectedException));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestWhenAllTupleTwoArgsSuccess()
    {
        var output = Result.WhenAll(Result.Success(1), Result.Success("two"));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.Not.Null);
        Assert.That(output.Value.Item1, Is.EqualTo(1));
        Assert.That(output.Value.Item2, Is.EqualTo("two"));
    }

    [Test]
    public void TestWhenAllTupleTwoArgsNullOrDefaultThrows()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Result.WhenAll((Result<int>)default!, Result.Success("two")));
        Assert.Throws<ArgumentNullException>(() =>
            Result.WhenAll(Result.Success(1), (Result<string>)default!));
    }

    [Test]
    public void TestWhenAllTupleTwoArgsFailureSecond()
    {
        var output = Result.WhenAll(Result.Success(1), Result.Failure<string>(ExpectedException));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestWhenAllTupleTwoArgsFailureFirst()
    {
        var output = Result.WhenAll(Result.Failure<int>(ExpectedException), Result.Success("two"));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestWhenAllTupleThreeArgsSuccess()
    {
        var output = Result.WhenAll(Result.Success(1), Result.Success("two"), Result.Success(3.5d));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.Not.Null);
        Assert.That(output.Value.Item1, Is.EqualTo(1));
        Assert.That(output.Value.Item2, Is.EqualTo("two"));
        Assert.That(output.Value.Item3, Is.EqualTo(3.5d));
    }

    [Test]
    public void TestWhenAllTupleThreeArgsNullOrDefaultThrows()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Result.WhenAll((Result<int>)default!, Result.Success("two"), Result.Success(3.5d)));
        Assert.Throws<ArgumentNullException>(() =>
            Result.WhenAll(Result.Success(1), (Result<string>)default!, Result.Success(3.5d)));
        Assert.Throws<ArgumentNullException>(() =>
            Result.WhenAll(Result.Success(1), Result.Success("two"), (Result<double>)default!));
    }

    [Test]
    public void TestWhenAllTupleThreeArgsFailureThird()
    {
        var output = Result.WhenAll(Result.Success(1), Result.Success("two"), Result.Failure<double>(ExpectedException));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestWhenAllTupleThreeArgsFailureSecond()
    {
        var output = Result.WhenAll(Result.Success(1), Result.Failure<string>(ExpectedException), Result.Success(3.5d));
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }
}