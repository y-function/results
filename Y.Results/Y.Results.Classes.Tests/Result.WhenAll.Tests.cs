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
    public void TestWhenAllContainsNull()
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
}