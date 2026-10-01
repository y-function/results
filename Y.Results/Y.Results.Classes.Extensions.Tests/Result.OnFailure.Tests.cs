using Moq;

namespace Y.Results.Extensions.Tests;

public partial class ResultExtensionsTests
{
    private Exception ExpectedException { get; } = new Exception("Expected exception.");
    
    [Test]
    public void TestOnFailureNullResult()
    {
        var result = (Result)null!;
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
        Assert.That(output, Is.SameAs(input));
        actionMock.Verify(a => a.Invoke(ExpectedException), Times.Once);
    }

    [Test]
    public void TestOnFailureThrowingAction()
    {
        var input = Result.Failure(ExpectedException);
        var output = input.OnFailure(_ => throw new InvalidOperationException());
        Assert.That(output, Is.SameAs(input));
    }

    [Test]
    public void TestOnFailureWithSuccessResult()
    {
        var input = Result.Success();
        var output = input.OnFailure(_ => Assert.Fail());
        Assert.That(output, Is.SameAs(input));
    }

    [Test]
    public void TestOnFailureTypedNullResult()
    {
        var result = (Result<object>)null!;
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
        Assert.That(output, Is.SameAs(input));
        actionMock.Verify(a => a.Invoke(ExpectedException), Times.Once);
    }

    [Test]
    public void TestOnFailureTypedThrowingAction()
    {
        var input = Result.Failure<object>(ExpectedException);
        var output = input.OnFailure(_ => throw new InvalidOperationException());
        Assert.That(output, Is.SameAs(input));
    }

    [Test]
    public void TestOnFailureTypedWithSuccessResult()
    {
        var input = Result.Success(1);
        var output = input.OnFailure(_ => Assert.Fail());
        Assert.That(output, Is.SameAs(input));
    }
}