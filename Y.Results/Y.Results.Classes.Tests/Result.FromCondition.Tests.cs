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

    [Test]
    public void TestFromConditionValueNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.FromCondition(new object(), null!, () => ExpectedException));
    }

    [Test]
    public void TestFromConditionValueSuccess()
    {
        var o = new object();
        var result = Result.FromCondition(
            o,
            v =>
            {
                Assert.That(v, Is.SameAs(o));
                return true;
            },
            () =>
            {
                Assert.Fail();
                return ExpectedException;
            });
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(o));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromConditionValueFailure()
    {
        Func<Exception>? onFailure = null;
        var result = Result.FromCondition(new object(), _ => false, onFailure);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf<Exception>());
        Assert.That(result.Exception!.Message, Is.EqualTo("Condition validation has failed."));
    }

    [Test]
    public void TestFromConditionValueFailureCustomException()
    {
        var expectedException = new Exception();
        var result = Result.FromCondition(new object(), _ => false, () => expectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromConditionValueFailureOnThrow()
    {
        var expectedException = new Exception();
        var result = Result.FromCondition(new object(), _ => throw expectedException, (Func<Exception>?)null);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromConditionValueFailureOnThrowCustomException()
    {
        var expectedException = new Exception();
        var result = Result.FromCondition(new object(), _ => throw expectedException, () => ExpectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromConditionValueFailureOnThrowInOnFailure()
    {
        var expectedException = new Exception();
        var result = Result.FromCondition(new object(), _ => false, () => throw expectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromConditionValueTypedNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.FromCondition(new object(), null!, _ => ExpectedException));
    }

    [Test]
    public void TestFromConditionValueTypedSuccess()
    {
        var o = new object();
        var result = Result.FromCondition(
            o,
            v =>
            {
                Assert.That(v, Is.SameAs(o));
                return true;
            },
            _ =>
            {
                Assert.Fail();
                return ExpectedException;
            });
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(o));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromConditionValueTypedFailure()
    {
        Func<object, Exception>? onFailure = null;
        var result = Result.FromCondition(new object(), _ => false, onFailure);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf<Exception>());
        Assert.That(result.Exception!.Message, Is.EqualTo("Condition validation has failed."));
    }

    [Test]
    public void TestFromConditionValueTypedFailureCustomException()
    {
        var o = new object();
        var expectedException = new Exception();
        var result = Result.FromCondition(o, _ => false, v =>
        {
            Assert.That(v, Is.SameAs(o));
            return expectedException;
        });
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromConditionValueTypedFailureOnThrow()
    {
        var expectedException = new Exception();
        var result = Result.FromCondition(new object(), _ => throw expectedException, (Func<object, Exception>?)null);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromConditionValueTypedFailureOnThrowInOnFailure()
    {
        var expectedException = new Exception();
        var result = Result.FromCondition(new object(), _ => false, _ => throw expectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }
}