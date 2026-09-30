namespace Y.Results.Structs.Tests;

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
    public void TestSuccessWithValueType()
    {
        const int v = 42;
        var result = Result.Success(v);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo(v));
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
    public void TestFailureWithValueType()
    {
        var expectedException = new Exception();
        var result = Result.Failure<int>(expectedException);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.EqualTo(0));
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromActionNull()
    {
        Assert.That(() => Result.FromAction((Action)null!), Throws.TypeOf<ArgumentNullException>());
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
        Assert.That(
            () => Result.FromAction((Func<Task>)null!).GetAwaiter().GetResult(),
            Throws.TypeOf<ArgumentNullException>());
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
    public void TestFromActionFuncNull()
    {
        Assert.That(() => Result.FromAction((Func<object>)null!), Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void TestFromActionFuncSuccess()
    {
        var v = new object();
        var result = Result.FromAction(() => v);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(v));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromActionFuncFailure()
    {
        var expectedException = new Exception();
        object Func() => throw expectedException;
        var result = Result.FromAction(Func);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromActionFuncNullAsync()
    {
        Assert.That(
            () => Result.FromAction((Func<Task<object>>)null!).GetAwaiter().GetResult(),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void TestFromActionFuncSuccessAsync()
    {
        var v = new object();
        var result = Result.FromAction(() => Task.FromResult(v)).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(v));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromActionFuncFailureAsync()
    {
        var expectedException = new Exception();

        Task<object> Func() => Task.FromException<object>(expectedException);

        var result = Result.FromAction(Func).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromConditionNullPredicate()
    {
        Assert.That(
            () => Result.FromCondition(new object(), null!),
            Throws.TypeOf<ArgumentNullException>());
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
    public void TestFromConditionFailureCustomErrorMessage()
    {
        var v = new object();
        var result = Result.FromCondition(v, _ => false, "custom error");
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.Not.Null);
        Assert.That(result.Exception!.Message, Is.EqualTo("custom error"));
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
