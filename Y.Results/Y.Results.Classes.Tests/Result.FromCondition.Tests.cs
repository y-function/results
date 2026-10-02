namespace Y.Results.Tests;

public partial class ResultTests
{
    [Test]
    public void TestFromConditionNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.FromCondition(null!));
    }

    [Test]
    public void TestFromConditionSuccess()
    {
        var result = Result.FromCondition(() => true);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromConditionFailure()
    {
        var result = Result.FromCondition(() => false);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf<Exception>());
        Assert.That(result.Exception!.Message, Is.EqualTo("Condition validation has failed."));
    }

    [Test]
    public void TestFromConditionFailureCustomException()
    {
        var expectedException = new Exception();
        var result = Result.FromCondition(() => false, () => expectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromConditionFailureOnThrow()
    {
        var expectedException = new Exception();
        var result = Result.FromCondition(() => throw expectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromConditionFailureOnThrowCustomException()
    {
        var expectedException = new Exception();
        var result = Result.FromCondition(() => throw expectedException, () => ExpectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }
}