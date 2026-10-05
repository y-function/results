using Moq;

namespace Y.Results.Extensions.Tests;

public partial class ResultExtensionsTests
{
    [Test]
    public void TestThenActionAsyncNullResult()
    {
        Result result = default!;
        Assert.ThrowsAsync<ArgumentNullException>(() => result.Then(() => Task.CompletedTask));
    }

    [Test]
    public void TestThenActionAsyncNullAction()
    {
        var input = Result.Success();
        Assert.ThrowsAsync<ArgumentNullException>(() => input.Then((Func<Task>)null!));
    }

    [Test]
    public async Task TestThenActionAsyncWithSuccessResult()
    {
        var actionMock = new Mock<Func<Task>>();
        actionMock.Setup(a => a.Invoke()).Returns(Task.CompletedTask);
        var input = Result.Success();
        var output = await input.Then(actionMock.Object);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output, Is.Not.SameAs(input));
        actionMock.Verify(a => a.Invoke(), Times.Once);
    }

    [Test]
    public async Task TestThenActionAsyncThrowingAction()
    {
        var input = Result.Success();
        var output = await input.Then((Func<Task>)(() => Task.FromException(ExpectedException)));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public async Task TestThenActionAsyncWithFailureResult()
    {
        var actionMock = new Mock<Func<Task>>();
        var input = Result.Failure(ExpectedException);
        var output = await input.Then(actionMock.Object);
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
        actionMock.Verify(a => a.Invoke(), Times.Never);
    }

    [Test]
    public void TestThenValueActionAsyncNullResult()
    {
        Result<string> result = default!;
        Assert.ThrowsAsync<ArgumentNullException>(() => result.Then(_ => Task.CompletedTask));
    }

    [Test]
    public void TestThenValueActionAsyncNullAction()
    {
        var input = Result.Success("abc");
        Assert.ThrowsAsync<ArgumentNullException>(() => input.Then((Func<string, Task>)null!));
    }

    [Test]
    public async Task TestThenValueActionAsyncWithSuccessResult()
    {
        var actionMock = new Mock<Func<string, Task>>();
        actionMock.Setup(a => a.Invoke("abc")).Returns(Task.CompletedTask);
        var input = Result.Success("abc");
        var output = await input.Then(actionMock.Object);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output, Is.Not.SameAs(input));
        actionMock.Verify(a => a.Invoke("abc"), Times.Once);
    }

    [Test]
    public async Task TestThenValueActionAsyncThrowingAction()
    {
        var input = Result.Success("abc");
        var output = await input.Then((Func<string, Task>)(
            _ => Task.FromException(ExpectedException)));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public async Task TestThenValueActionAsyncWithFailureResult()
    {
        var actionMock = new Mock<Func<string, Task>>();
        var input = Result.Failure<string>(ExpectedException);
        var output = await input.Then(actionMock.Object);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
        actionMock.Verify(a => a.Invoke(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void TestThenResultFuncGenericAsyncNullResult()
    {
        Result result = default!;
        Assert.ThrowsAsync<ArgumentNullException>(() => result.Then(() => Task.FromResult(Result.Success(1))));
    }

    [Test]
    public void TestThenResultFuncGenericAsyncNullAction()
    {
        var input = Result.Success();
        Assert.ThrowsAsync<ArgumentNullException>(() => input.Then((Func<Task<Result<int>>>)null!));
    }

    [Test]
    public async Task TestThenResultFuncGenericAsyncWithSuccessResult()
    {
        var actionMock = new Mock<Func<Task<Result<int>>>>();
        actionMock.Setup(a => a.Invoke()).ReturnsAsync(Result.Success(42));
        var input = Result.Success();
        var output = await input.Then(actionMock.Object);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.EqualTo(42));
        actionMock.Verify(a => a.Invoke(), Times.Once);
    }

    [Test]
    public async Task TestThenResultFuncGenericAsyncThrowingAction()
    {
        var input = Result.Success();
        var output = await input.Then((Func<Task<Result<int>>>)(
            () => Task.FromException<Result<int>>(ExpectedException)));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public async Task TestThenResultFuncGenericAsyncWithFailureResult()
    {
        var actionMock = new Mock<Func<Task<Result<int>>>>();
        var input = Result.Failure(ExpectedException);
        var output = await input.Then(actionMock.Object);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
        actionMock.Verify(a => a.Invoke(), Times.Never);
    }

    [Test]
    public void TestThenResultFuncAsyncNullResult()
    {
        Result result = default!;
        Assert.ThrowsAsync<ArgumentNullException>(() => result.Then(() => Task.FromResult(Result.Success())));
    }

    [Test]
    public void TestThenResultFuncAsyncNullAction()
    {
        var input = Result.Success();
        Assert.ThrowsAsync<ArgumentNullException>(() => input.Then((Func<Task<Result>>)null!));
    }

    [Test]
    public async Task TestThenResultFuncAsyncWithSuccessResult()
    {
        var outputResult = Result.Success();
        var actionMock = new Mock<Func<Task<Result>>>();
        actionMock.Setup(a => a.Invoke()).ReturnsAsync(outputResult);
        var input = Result.Success();
        var output = await input.Then(actionMock.Object);
#if CLASSES
        Assert.That(output, Is.SameAs(outputResult));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(outputResult));
#endif
        actionMock.Verify(a => a.Invoke(), Times.Once);
    }

    [Test]
    public async Task TestThenResultFuncAsyncThrowingAction()
    {
        var input = Result.Success();
        var output = await input.Then((Func<Task<Result>>)(() => Task.FromException<Result>(ExpectedException)));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public async Task TestThenResultFuncAsyncWithFailureResult()
    {
        var actionMock = new Mock<Func<Task<Result>>>();
        var input = Result.Failure(ExpectedException);
        var output = await input.Then(actionMock.Object);
#if CLASSES
        Assert.That(output, Is.SameAs(input));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(input));
#endif
        actionMock.Verify(a => a.Invoke(), Times.Never);
    }

    [Test]
    public void TestThenValueResultFuncAsyncNullResult()
    {
        Result<string> result = default!;
        Assert.ThrowsAsync<ArgumentNullException>(() => result.Then(_ => Task.FromResult(Result.Success())));
    }

    [Test]
    public void TestThenValueResultFuncAsyncNullMapper()
    {
        var input = Result.Success("abc");
        Assert.ThrowsAsync<ArgumentNullException>(() => input.Then((Func<string?, Task<Result>>)null!));
    }

    [Test]
    public async Task TestThenValueResultFuncAsyncWithSuccessResult()
    {
        var outputResult = Result.Success();
        var mapperMock = new Mock<Func<string?, Task<Result>>>();
        mapperMock.Setup(m => m.Invoke("abc")).ReturnsAsync(outputResult);
        var input = Result.Success("abc");
        var output = await input.Then(mapperMock.Object);
#if CLASSES
        Assert.That(output, Is.SameAs(outputResult));
#elif STRUCTS
        Assert.That(output, Is.EqualTo(outputResult));
#endif
        mapperMock.Verify(m => m.Invoke("abc"), Times.Once);
    }

    [Test]
    public async Task TestThenValueResultFuncAsyncThrowingMapper()
    {
        var input = Result.Success("abc");
        var output = await input.Then((Func<string?, Task<Result>>)(
            _ => Task.FromException<Result>(ExpectedException)));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public async Task TestThenValueResultFuncAsyncWithFailureResult()
    {
        var mapperMock = new Mock<Func<string?, Task<Result>>>();
        var input = Result.Failure<string>(ExpectedException);
        var output = await input.Then(mapperMock.Object);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
        mapperMock.Verify(m => m.Invoke(It.IsAny<string?>()), Times.Never);
    }

    [Test]
    public void TestThenValueMapperAsyncNullResult()
    {
        Result<string> result = default!;
        Assert.ThrowsAsync<ArgumentNullException>(() => result.Then(_ => Task.FromResult(1)));
    }

    [Test]
    public void TestThenValueMapperAsyncNullMapper()
    {
        var input = Result.Success("abc");
        Assert.ThrowsAsync<ArgumentNullException>(() => input.Then((Func<string?, Task<int>>)null!));
    }

    [Test]
    public async Task TestThenValueMapperAsyncWithSuccessResult()
    {
        var mapperMock = new Mock<Func<string?, Task<int>>>();
        mapperMock.Setup(m => m.Invoke("abc")).ReturnsAsync(42);
        var input = Result.Success("abc");
        var output = await input.Then(mapperMock.Object);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.EqualTo(42));
        mapperMock.Verify(m => m.Invoke("abc"), Times.Once);
    }

    [Test]
    public async Task TestThenValueMapperAsyncThrowingMapper()
    {
        var input = Result.Success("abc");
        var output = await input.Then((Func<string?, Task<int>>)(
            _ => Task.FromException<int>(ExpectedException)));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public async Task TestThenValueMapperAsyncWithFailureResult()
    {
        var mapperMock = new Mock<Func<string?, Task<int>>>();
        var input = Result.Failure<string>(ExpectedException);
        var output = await input.Then(mapperMock.Object);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
        mapperMock.Verify(m => m.Invoke(It.IsAny<string?>()), Times.Never);
    }

    [Test]
    public void TestThenValueResultMapperAsyncNullResult()
    {
        Result<string> result = default!;
        Assert.ThrowsAsync<ArgumentNullException>(() => result.Then(_ => Task.FromResult(Result.Success(1))));
    }

    [Test]
    public void TestThenValueResultMapperAsyncNullMapper()
    {
        var input = Result.Success("abc");
        Assert.ThrowsAsync<ArgumentNullException>(() => input.Then((Func<string?, Task<Result<int>>>)null!));
    }

    [Test]
    public async Task TestThenValueResultMapperAsyncWithSuccessResult()
    {
        var mapperMock = new Mock<Func<string?, Task<Result<int>>>>();
        mapperMock.Setup(m => m.Invoke("abc")).ReturnsAsync(Result.Success(42));
        var input = Result.Success("abc");
        var output = await input.Then(mapperMock.Object);
        Assert.That(output.IsSuccess, Is.True);
        Assert.That(output.Value, Is.EqualTo(42));
        mapperMock.Verify(m => m.Invoke("abc"), Times.Once);
    }

    [Test]
    public async Task TestThenValueResultMapperAsyncThrowingMapper()
    {
        var input = Result.Success("abc");
        var output = await input.Then((Func<string?, Task<Result<int>>>)(
            _ => Task.FromException<Result<int>>(ExpectedException)));
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
    }

    [Test]
    public async Task TestThenValueResultMapperAsyncWithFailureResult()
    {
        var mapperMock = new Mock<Func<string?, Task<Result<int>>>>();
        var input = Result.Failure<string>(ExpectedException);
        var output = await input.Then(mapperMock.Object);
        Assert.That(output.IsSuccess, Is.False);
        Assert.That(output.Exception, Is.SameAs(ExpectedException));
        mapperMock.Verify(m => m.Invoke(It.IsAny<string?>()), Times.Never);
    }
}
