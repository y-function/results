using Moq;

namespace Y.Results.Extensions.Tests;

public partial class ResultExtensionsTests
{
    [Test]
    public void TestThenResultNullResult()
    {
        Result result = null!;
        Result next = Result.Success();
        Assert.Throws<ArgumentNullException>(() => result.Then(next));
    }

    [Test]
    public void TestThenResultNullNext()
    {
        var input = Result.Success();
        Assert.Throws<ArgumentNullException>(() => input.Then((Result)null!));
    }

    [Test]
    public void TestThenResultForFailure()
    {
        var input = Result.Failure(ExpectedException);
        var next = Result.Success();
        var output = input.Then(next);
        Assert.That(output, Is.SameAs(input));
    }

    [Test]
    public void TestThenResultVoidToTypedForSuccess()
    {
        var input = Result.Success();
        var next = Result.Success();
        var output = input.Then(next);
        Assert.That(output, Is.SameAs(next));
    }

    [Test]
    public void TestThenResultVoidToTypedNullResult()
    {
        Result result = null!;
        var next = Result.Success(1);
        Assert.Throws<ArgumentNullException>(() => result.Then(next));
    }

    [Test]
    public void TestThenResultVoidToTypedNullNext()
    {
        var input = Result.Success();
        Assert.Throws<ArgumentNullException>(() => input.Then((Result<int>)null!));
    }

    [Test]
    public void TestThenResultVoidToTypedForFailure()
    {
        var input = Result.Failure(ExpectedException);
        var next = Result.Success(1);
        var output = input.Then(next);
        Assert.That(output, Is.Not.Null);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestThenResultForSuccess()
    {
        var input = Result.Success();
        var next = Result.Success(2);
        var output = input.Then(next);
        Assert.That(output, Is.SameAs(next));
    }
    
    [Test]
    public void TestThenActionNullResult()
    {
        Result result = null!;
        Assert.Throws<ArgumentNullException>(() => result.Then(() => { }));
    }

    [Test]
    public void TestThenActionNullAction()
    {
        var input = Result.Success();
        Assert.Throws<ArgumentNullException>(() => input.Then((Action)null!));
    }

    [Test]
    public void TestThenActionWithSuccessResult()
    {
        var actionMock = new Mock<Action>();
        var input = Result.Success();
        var output = input.Then(actionMock.Object);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output, Is.Not.SameAs(input));
        actionMock.Verify(a => a.Invoke(), Times.Once);
    }

    [Test]
    public void TestThenActionThrowingAction()
    {
        var input = Result.Success();
        var output = input.Then((Action)(() => throw ExpectedException));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestThenActionWithFailureResult()
    {
        var actionMock = new Mock<Action>();
        var input = Result.Failure(ExpectedException);
        var output = input.Then(actionMock.Object);
        Assert.That(output, Is.SameAs(input));
        actionMock.Verify(a => a.Invoke(), Times.Never);
    }
    
    [Test]
    public void TestThenResultTypedNullResult()
    {
        var result = (Result<int>)null!;
        var next = Result.Success(1);
        Assert.Throws<ArgumentNullException>(() => result.Then(next));
    }

    [Test]
    public void TestThenResultTypedNullNext()
    {
        var input = Result.Success(1);
        Assert.Throws<ArgumentNullException>(() => input.Then((Result<int>)null!));
    }

    [Test]
    public void TestThenResultTypedForFailure()
    {
        var input = Result.Failure<int>(ExpectedException);
        var next = Result.Success(1);
        var output = input.Then(next);
        Assert.That(output, Is.SameAs(input));
    }

    [Test]
    public void TestThenResultTypedForSuccess()
    {
        var input = Result.Success(1);
        var next = Result.Success(2);
        var output = input.Then(next);
        Assert.That(output, Is.SameAs(next));
    }
    

    [Test]
    public void TestThenFuncNullResult()
    {
        Result result = null!;
        Assert.Throws<ArgumentNullException>(() => result.Then(() => 1));
    }

    [Test]
    public void TestThenFuncNullFunc()
    {
        var input = Result.Success();
        Assert.Throws<ArgumentNullException>(() => input.Then((Func<int>)null!));
    }

    [Test]
    public void TestThenFuncWithSuccessResult()
    {
        var funcMock = new Mock<Func<int>>();
        funcMock.Setup(f => f.Invoke()).Returns(42);
        var input = Result.Success();
        var output = input.Then(funcMock.Object);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.EqualTo(42));
        funcMock.Verify(f => f.Invoke(), Times.Once);
    }

    [Test]
    public void TestThenFuncThrowingFunc()
    {
        var input = Result.Success();
        var output = input.Then((Func<int>)(() => throw ExpectedException));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestThenFuncWithFailureResult()
    {
        var funcMock = new Mock<Func<int>>();
        var input = Result.Failure(ExpectedException);
        var output = input.Then(funcMock.Object);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
        funcMock.Verify(f => f.Invoke(), Times.Never);
    }

    [Test]
    public void TestThenResultFuncNullResult()
    {
        Result result = null!;
        Assert.Throws<ArgumentNullException>(() => result.Then(Result.Success));
    }

    [Test]
    public void TestThenResultFuncNullFunc()
    {
        var input = Result.Success();
        Assert.Throws<ArgumentNullException>(() => input.Then((Func<Result>)null!));
    }

    [Test]
    public void TestThenResultFuncWithSuccessResult()
    {
        var outputResult = Result.Success();
        var funcMock = new Mock<Func<Result>>();
        funcMock.Setup(f => f.Invoke()).Returns(outputResult);
        var input = Result.Success();
        var output = input.Then(funcMock.Object);
        Assert.That(output, Is.SameAs(outputResult));
        funcMock.Verify(f => f.Invoke(), Times.Once);
    }

    [Test]
    public void TestThenResultFuncThrowingFuncThrows()
    {
        var input = Result.Success();
        var outputException = new InvalidOperationException();
        var output = input.Then((Func<Result>)(() => throw outputException));
        Assert.That(output, Is.Not.SameAs(input));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(outputException));
    }

    [Test]
    public void TestThenResultFuncWithFailureResult()
    {
        var funcMock = new Mock<Func<Result>>();
        var input = Result.Failure(ExpectedException);
        var output = input.Then(funcMock.Object);
        Assert.That(output, Is.SameAs(input));
        funcMock.Verify(f => f.Invoke(), Times.Never);
    }

    [Test]
    public void TestThenResultFuncGenericNullResult()
    {
        Result result = null!;
        Assert.Throws<ArgumentNullException>(() => result.Then(() => Result.Success(1)));
    }

    [Test]
    public void TestThenResultFuncGenericNullFunc()
    {
        var input = Result.Success();
        Assert.Throws<ArgumentNullException>(() => input.Then((Func<Result<int>>)null!));
    }

    [Test]
    public void TestThenResultFuncGenericWithSuccessResult()
    {
        var funcMock = new Mock<Func<Result<int>>>();
        funcMock.Setup(f => f.Invoke()).Returns(Result.Success(42));
        var input = Result.Success();
        var output = input.Then(funcMock.Object);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.EqualTo(42));
        funcMock.Verify(f => f.Invoke(), Times.Once);
    }

    [Test]
    public void TestThenResultFuncGenericReturnsFailure()
    {
        var failureResult = Result.Failure<int>(ExpectedException);
        var input = Result.Success();
        var output = input.Then(() => failureResult);
        Assert.That(output, Is.SameAs(failureResult));
    }

    [Test]
    public void TestThenResultFuncGenericThrowingFunc()
    {
        var input = Result.Success();
        var output = input.Then((Func<Result<int>>)(() => throw ExpectedException));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestThenResultFuncGenericWithFailureResult()
    {
        var funcMock = new Mock<Func<Result<int>>>();
        var input = Result.Failure(ExpectedException);
        var output = input.Then(funcMock.Object);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
        funcMock.Verify(f => f.Invoke(), Times.Never);
    }

    [Test]
    public void TestThenValueMapperNullResult()
    {
        Result<string> result = null!;
        Assert.Throws<ArgumentNullException>(() => result.Then(x => x.Length));
    }

    [Test]
    public void TestThenValueMapperNullMapper()
    {
        var input = Result.Success("abc");
        Assert.Throws<ArgumentNullException>(() => input.Then((Func<string?, int>)null!));
    }

    [Test]
    public void TestThenValueMapperWithSuccessResult()
    {
        var mapperMock = new Mock<Func<string?, int>>();
        mapperMock.Setup(m => m.Invoke("abc")).Returns(3);
        var input = Result.Success("abc");
        var output = input.Then(mapperMock.Object);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.EqualTo(3));
        mapperMock.Verify(m => m.Invoke("abc"), Times.Once);
    }

    [Test]
    public void TestThenValueMapperThrowingMapper()
    {
        var input = Result.Success("abc");
        var output = input.Then((Func<string?, int>)(_ => throw ExpectedException));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestThenValueMapperWithFailureResult()
    {
        var mapperMock = new Mock<Func<string?, int>>();
        var input = Result.Failure<string>(ExpectedException);
        var output = input.Then(mapperMock.Object);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
        mapperMock.Verify(m => m.Invoke(It.IsAny<string?>()), Times.Never);
    }

    [Test]
    public void TestThenValueResultMapperNullResult()
    {
        Result<string> result = null!;
        Assert.Throws<ArgumentNullException>(() => result.Then(x => Result.Success(x.Length)));
    }

    [Test]
    public void TestThenValueResultMapperNullMapper()
    {
        var input = Result.Success("abc");
        Assert.Throws<ArgumentNullException>(() => input.Then((Func<string?, Result<int>>)null!));
    }

    [Test]
    public void TestThenValueResultMapperWithSuccessResult()
    {
        var mapperMock = new Mock<Func<string?, Result<int>>>();
        mapperMock.Setup(m => m.Invoke("abc")).Returns(Result.Success(3));
        var input = Result.Success("abc");
        var output = input.Then(mapperMock.Object);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.EqualTo(3));
        mapperMock.Verify(m => m.Invoke("abc"), Times.Once);
    }

    [Test]
    public void TestThenValueResultMapperReturnsFailure()
    {
        var failureResult = Result.Failure<int>(ExpectedException);
        var input = Result.Success("abc");
        var output = input.Then(_ => failureResult);
        Assert.That(output, Is.SameAs(failureResult));
    }

    [Test]
    public void TestThenValueResultMapperThrowingMapper()
    {
        var input = Result.Success("abc");
        var output = input.Then((Func<string?, Result<int>>)(_ => throw ExpectedException));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestThenValueResultMapperWithFailureResult()
    {
        var mapperMock = new Mock<Func<string?, Result<int>>>();
        var input = Result.Failure<string>(ExpectedException);
        var output = input.Then(mapperMock.Object);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
        mapperMock.Verify(m => m.Invoke(It.IsAny<string?>()), Times.Never);
    }

    [Test]
    public void TestThenValueResultFuncNullResult()
    {
        Result<string> result = null!;
        Assert.Throws<ArgumentNullException>(() => result.Then(_ => Result.Success()));
    }

    [Test]
    public void TestThenValueResultFuncNullMapper()
    {
        var input = Result.Success("abc");
        Assert.Throws<ArgumentNullException>(() => input.Then((Func<string?, Result>)null!));
    }

    [Test]
    public void TestThenValueResultFuncWithSuccessResult()
    {
        var outputResult = Result.Success();
        var mapperMock = new Mock<Func<string?, Result>>();
        mapperMock.Setup(m => m.Invoke("abc")).Returns(outputResult);
        var input = Result.Success("abc");
        var output = input.Then(mapperMock.Object);
        Assert.That(output, Is.SameAs(outputResult));
        mapperMock.Verify(m => m.Invoke("abc"), Times.Once);
    }

    [Test]
    public void TestThenValueResultFuncThrowingMapper()
    {
        var input = Result.Success("abc");
        var output = input.Then((Func<string?, Result>)(_ => throw ExpectedException));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public void TestThenValueResultFuncWithFailureResult()
    {
        var mapperMock = new Mock<Func<string?, Result>>();
        var input = Result.Failure<string>(ExpectedException);
        var output = input.Then(mapperMock.Object);
        Assert.That(output, Is.SameAs(input));
        mapperMock.Verify(m => m.Invoke(It.IsAny<string?>()), Times.Never);
    }
}
