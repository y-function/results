using Moq;

namespace Y.Results.Extensions.Tests;

public partial class ResultExtensionsTests
{
    
    [Test]
    public void TestOnFailureNullResult()
    {
        Result result = default!;
        Assert.Throws<ArgumentNullException>(() => result.OnFailure(_ => {}));
    }

    [Test]
    public void TestOnFailureNullAction()
    {
        var input = Result.Failure(ExpectedException);
        Assert.Throws<ArgumentNullException>(() => input.OnFailure(null!));
    }

    [Test]
    public void TestOnFailureSomeAction()
    {
        var actionMock = new Mock<Action<Exception>>();
        var input = Result.Failure(ExpectedException);
        var output = input.OnFailure(actionMock.Object);
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
        actionMock.Verify(a => a.Invoke(ExpectedException), Times.Once);
    }

    [Test]
    public void TestOnFailureThrowingAction()
    {
        var input = Result.Failure(ExpectedException);
        var output = input.OnFailure(_ => throw new InvalidOperationException());
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
    }

    [Test]
    public void TestOnFailureWithSuccessResult()
    {
        var input = Result.Success();
        var output = input.OnFailure(_ => Assert.Fail());
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
    }

    [Test]
    public void TestOnFailureTypedNullResult()
    {
        Result<object> result = default!;
        Assert.Throws<ArgumentNullException>(() => result.OnFailure(_ => { }));
    }

    [Test]
    public void TestOnFailureTypedNullAction()
    {
        var input = Result.Failure<object>(ExpectedException);
        Assert.Throws<ArgumentNullException>(() => input.OnFailure(null!));
    }

    [Test]
    public void TestOnFailureTypedEmptyAction()
    {
        var actionMock = new Mock<Action<Exception>>();
        var input = Result.Failure<object>(ExpectedException);
        var output = input.OnFailure(actionMock.Object);
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
        actionMock.Verify(a => a.Invoke(ExpectedException), Times.Once);
    }

    [Test]
    public void TestOnFailureTypedThrowingAction()
    {
        var input = Result.Failure<object>(ExpectedException);
        var output = input.OnFailure(_ => throw new InvalidOperationException());
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
    }

    [Test]
    public void TestOnFailureTypedWithSuccessResult()
    {
        var input = Result.Success(1);
        var output = input.OnFailure(_ => Assert.Fail());
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
    }
}