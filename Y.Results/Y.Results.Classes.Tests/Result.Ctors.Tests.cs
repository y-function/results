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
        Assert.Throws<ArgumentNullException>(() => Result.Wrap((Action)null!));
    }

    [Test]
    public void TestFromActionSuccess()
    {
        var result = Result.Wrap(() => { });
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromActionFailure()
    {
        var expectedException = new Exception();

        void Action() => throw expectedException;

        var result = Result.Wrap(Action);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromActionNullAsync()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Wrap((Func<Task>)null!).GetAwaiter().GetResult());
    }

    [Test]
    public void TestFromActionSuccessAsync()
    {
        Task Action() => Task.CompletedTask;
        var result = Result.Wrap(Action).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromActionFailureAsync()
    {
        var expectedException = new Exception();
        Task Action() => Task.FromException(expectedException);
        var result = Result.Wrap(Action).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromFunctionNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Wrap((Func<object>)null!));
    }

    [Test]
    public void TestFromFunctionSuccess()
    {
        var v = new object();
        var result = Result.Wrap(() => v);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(v));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromFunctionFailure()
    {
        var expectedException = new Exception();
        object Func() => throw expectedException;
        var result = Result.Wrap(Func);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestFromFunctionNullAsync()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Wrap((Func<Task<object>>)null!).GetAwaiter().GetResult());
    }

    [Test]
    public void TestFromFunctionSuccessAsync()
    {
        var v = new object();
        var result = Result.Wrap(() => Task.FromResult(v)).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(v));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestFromFunctionFailureAsync()
    {
        var expectedException = new Exception();

        Task<object> Func() => Task.FromException<object>(expectedException);

        var result = Result.Wrap(Func).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

}