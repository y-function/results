namespace Y.Results.Extensions.Tests;

public partial class ResultExtensionsTests
{
    [Test]
    public void TestOnSuccessNullResult()
    {
        Result result = default!;
        Assert.Throws<ArgumentNullException>(() => result.OnSuccess(() => {}));
    }

    [Test]
    public void TestOnSuccessNullAction()
    {
        var input = Result.Success();
        Assert.Throws<ArgumentNullException>(() => input.OnSuccess(null!));
    }

    [Test]
    public void TestOnSuccessEmptyAction()
    {
        var input = Result.Success();
        var output = input.OnSuccess(() => {});
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
    }

    [Test]
    public void TestOnSuccessThrowingAction()
    {
        var input = Result.Success();
        var output = input.OnSuccess(() => throw new InvalidOperationException());
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
    }

    [Test]
    public void TestOnSuccessWithFailureResult()
    {
        var input = Result.Failure(new Exception());
        var output = input.OnSuccess(Assert.Fail);
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
    }

    [Test]
    public void TestOnSuccessTypedNullResult()
    {
        Result<object> result = default!;
        Assert.Throws<ArgumentNullException>(() => result.OnSuccess(_ => { }));
    }

    [Test]
    public void TestOnSuccessTypedNullAction()
    {
        var input = Result.Success(1);
        Assert.Throws<ArgumentNullException>(() => input.OnSuccess(null!));
    }

    [Test]
    public void TestOnSuccessTypedEmptyAction()
    {
        var input = Result.Success(1);
        var output = input.OnSuccess(_ => { });
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
    }

    [Test]
    public void TestOnSuccessTypedThrowingAction()
    {
        var input = Result.Success(1);
        var output = input.OnSuccess(_ => throw new InvalidOperationException());
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
    }

    [Test]
    public void TestOnSuccessTypedWithFailureResult()
    {
        var input = Result.Failure<int>(new Exception());
        var output = input.OnSuccess(_ => Assert.Fail());
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
    }
}