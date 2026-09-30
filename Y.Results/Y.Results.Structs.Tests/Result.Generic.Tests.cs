namespace Y.Results.Structs.Tests;

public partial class ResultTests
{
    [Test]
    public void TestImplicitConversionFromSuccess()
    {
        var v = new object();
        var expectedResult = Result.Success(v);
        Result result = expectedResult;
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestImplicitConversionFromFailure()
    {
        var expectedException = new Exception();
        var expectedResult = Result.Failure<object>(expectedException);
        Result result = expectedResult;
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestImplicitConversionFromValueTypeSuccess()
    {
        var expectedResult = Result.Success(42);
        Result result = expectedResult;
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestImplicitConversionFromDefault()
    {
        Result result = default(Result<int>);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestDefaultResultIsFailure()
    {
        Result result = default;
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestDefaultGenericResultIsFailure()
    {
        Result<object> result = default;
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.Null);
    }
}
