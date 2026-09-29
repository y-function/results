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

    [Test]
    public void TestFromActionNull()
    {
        var result = Result.FromAction((Action)null!);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void TestFromActionSuccess()
    {
        var result = Result.FromAction(() => { });
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromActionFailure()
    {
        var expectedException = new Exception();

        void Action() => throw expectedException;

        var result = Result.FromAction(Action);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromActionNullAsync()
    {
        var result = Result.FromAction((Func<Task>)null!).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void TestFromActionSuccessAsync()
    {
        Task Action() => Task.CompletedTask;
        var result = Result.FromAction(Action).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromActionFailureAsync()
    {
        var expectedException = new Exception();
        Task Action() => Task.FromException(expectedException);
        var result = Result.FromAction(Action).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromFunctionNull()
    {
        var result = Result.FromFunction((Func<object>)null!);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void TestFromFunctionSuccess()
    {
        var v = new object();
        var result = Result.FromFunction(() => v);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(v));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromFunctionFailure()
    {
        var expectedException = new Exception();
        object Func() => throw expectedException;
        var result = Result.FromFunction(Func);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromFunctionNullAsync()
    {
        var result = Result.FromFunction((Func<Task<object>>)null!).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void TestFromFunctionSuccessAsync()
    {
        var v = new object();
        var result = Result.FromFunction(() => Task.FromResult(v)).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(v));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromFunctionFailureAsync()
    {
        var expectedException = new Exception();

        Task<object> Func() => Task.FromException<object>(expectedException);

        var result = Result.FromFunction(Func).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromConditionNull()
    {
        var result = Result.FromCondition(new object(), null!);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf<ArgumentNullException>());
    }
    
    [Test]
    public void TestFromConditionSuccess()
    {
        var v = new object();
        var result = Result.FromCondition(v, _ => true);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(v));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromConditionFailure()
    {
        var v = new object();
        var result = Result.FromCondition(v, _ => false);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception, Is.TypeOf<Exception>());
        Assert.That(result.Exception!.Message, Is.EqualTo("Condition validation has failed."));
    }

    [Test]
    public void TestFromConditionFailureCustomException()
    {
        var v = new object();
        var expectedException = new Exception();
        var result = Result.FromCondition(v, _ => false, () => expectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromConditionFailureOnThrow()
    {
        var v = new object();
        var expectedException = new Exception();
        var result = Result.FromCondition(v, _ => throw expectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }
}