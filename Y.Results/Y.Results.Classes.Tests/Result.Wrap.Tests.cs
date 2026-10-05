namespace Y.Results.Tests;

public partial class ResultTests
{
    [Test]
    public void TestWrapNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Wrap((Action)null!));
    }

    [Test]
    public void TestWrapSuccess()
    {
        var result = Result.Wrap(() => { });
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestWrapFailure()
    {
        var expectedException = new Exception();

        void Action() => throw expectedException;

        var result = Result.Wrap(Action);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestWrapNullAsync()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Wrap((Func<Task>)null!).GetAwaiter().GetResult());
    }

    [Test]
    public void TestWrapSuccessAsync()
    {
        Task Action() => Task.CompletedTask;
        var result = Result.Wrap(Action).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestWrapFailureAsync()
    {
        var expectedException = new Exception();
        Task Action() => Task.FromException(expectedException);
        var result = Result.Wrap(Action).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestWrapTypedNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Wrap((Func<object>)null!));
    }

    [Test]
    public void TestWrapTypedSuccess()
    {
        var v = new object();
        var result = Result.Wrap(() => v);
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(v));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestWrapTypedFailure()
    {
        var expectedException = new Exception();
        object Func() => throw expectedException;
        var result = Result.Wrap(Func);
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }

    [Test]
    public void TestWrapTypedNullAsync()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Wrap((Func<Task<object>>)null!).GetAwaiter().GetResult());
    }

    [Test]
    public void TestWrapTypedSuccessAsync()
    {
        var v = new object();
        var result = Result.Wrap(() => Task.FromResult(v)).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.SameAs(v));
        Assert.That(result.Exception, Is.Null);
    }

    [Test]
    public void TestWrapTypedFailureAsync()
    {
        var expectedException = new Exception();

        Task<object> Func() => Task.FromException<object>(expectedException);

        var result = Result.Wrap(Func).GetAwaiter().GetResult();
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Value, Is.Default);
        Assert.That(result.Exception, Is.SameAs(expectedException));
    }
}