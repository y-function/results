namespace Y.Results.Tests;

public partial class ResultTests
{
    [Test]
    public void TestSuccessEmpty()
    {
        var result = Result.Success();
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFailureEmpty()
    {
        var expectedException = new Exception();
        var result = Result.Failure(expectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestSuccessWithValue()
    {
        var v = new object();
        var result = Result.Success(v);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(v));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFailureWithValue()
    {
        var expectedException = new Exception();
        var result = Result.Failure<object>(expectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

}