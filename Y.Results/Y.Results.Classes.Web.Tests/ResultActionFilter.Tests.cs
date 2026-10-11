using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Y.Results.Web.Tests;

public class ResultActionFilterTests
{
    private Mock<IExceptionHandler> ExceptionHandler { get; } = new();
    private ResultActionFilter Filter { get; }

    public ResultActionFilterTests()
    {
        Filter = new ResultActionFilter(ExceptionHandler.Object);
    }

    [Test]
    public async Task TestOnResultExecutionAsyncForNonObjectResult()
    {
        var actionContext = CreateActionContext();
        var context = CreateResultExecutingContext(actionContext, new EmptyResult());
        var nextInvoked = false;

        await Filter.OnResultExecutionAsync(context, () =>
        {
            nextInvoked = true;
            return Task.FromResult(new ResultExecutedContext(actionContext, [], new EmptyResult(), null!));
        });

        Assert.That(nextInvoked, Is.True);
        Assert.That(context.Result, Is.TypeOf<EmptyResult>());
    }

    [Test]
    public async Task TestOnResultExecutionAsyncForObjectResultWithoutResultValue()
    {
        var actionContext = CreateActionContext();
        var originalResult = new ObjectResult("value");
        var context = CreateResultExecutingContext(actionContext, originalResult);
        var nextInvoked = false;

        await Filter.OnResultExecutionAsync(context, () =>
        {
            nextInvoked = true;
            return Task.FromResult(new ResultExecutedContext(actionContext, [], new EmptyResult(), null!));
        });

        Assert.That(nextInvoked, Is.True);
        Assert.That(context.Result, Is.SameAs(originalResult));
    }

    [Test]
    public async Task TestOnResultExecutionAsyncForSuccessWithValue()
    {
        var actionContext = CreateActionContext();
        var expectedValue = new object();
        var objectResult = new ObjectResult(Result.Success(expectedValue));
        var context = CreateResultExecutingContext(actionContext, objectResult);
        var nextInvoked = false;

        await Filter.OnResultExecutionAsync(context, () =>
        {
            nextInvoked = true;
            return Task.FromResult(new ResultExecutedContext(actionContext, [], new EmptyResult(), null!));
        });

        Assert.That(context.Result, Is.SameAs(objectResult));
        Assert.That(objectResult.Value, Is.SameAs(expectedValue));
        Assert.That(nextInvoked, Is.True);
    }

    [Test]
    public async Task TestOnResultExecutionAsyncForSuccessWithFileValue()
    {
        var actionContext = CreateActionContext();
        var expectedResult = new FileContentResult([1, 2, 3], "application/octet-stream");
        var context = CreateResultExecutingContext(
            actionContext,
            new ObjectResult(Result.Success(expectedResult)));

        await Filter.OnResultExecutionAsync(
            context,
            () => Task.FromResult(new ResultExecutedContext(actionContext, [], new EmptyResult(), null!)));

        Assert.That(context.Result, Is.SameAs(expectedResult));
    }

    [Test]
    public async Task TestOnResultExecutionAsyncForSuccessWithNullValueOnOkObjectResult()
    {
        var actionContext = CreateActionContext();
        var context = CreateResultExecutingContext(
            actionContext,
            new OkObjectResult(Result.Success<object?>(null)));

        await Filter.OnResultExecutionAsync(
            context,
            () => Task.FromResult(new ResultExecutedContext(actionContext, [], new EmptyResult(), null!)));

        Assert.That(context.Result, Is.TypeOf<OkResult>());
    }

    [Test]
    public async Task TestOnResultExecutionAsyncForSuccessWithNullValueAndNoContentMetadata()
    {
        var actionContext = CreateActionContext(new ReturnsNoContentOnSuccessAttribute());
        var context = CreateResultExecutingContext(
            actionContext,
            new OkObjectResult(Result.Success<object?>(null)));

        await Filter.OnResultExecutionAsync(
            context,
            () => Task.FromResult(new ResultExecutedContext(actionContext, [], new EmptyResult(), null!)));

        Assert.That(context.Result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public async Task TestOnResultExecutionAsyncForFailure()
    {
        var expectedException = new InvalidOperationException("Expected failure.");
        var actionContext = CreateActionContext();
        var context = CreateResultExecutingContext(
            actionContext,
            new ObjectResult(Result.Failure(expectedException)));
        var nextInvoked = false;
        ExceptionHandler
            .Setup(handler => handler.TryHandleAsync(
                actionContext.HttpContext,
                expectedException,
                CancellationToken.None))
            .ReturnsAsync(true);

        await Filter.OnResultExecutionAsync(context, () =>
        {
            nextInvoked = true;
            return Task.FromResult(new ResultExecutedContext(actionContext, [], new EmptyResult(), null!));
        });

        ExceptionHandler.Verify(handler => handler.TryHandleAsync(
            actionContext.HttpContext,
            expectedException,
            CancellationToken.None), Times.Once);
        Assert.That(nextInvoked, Is.False);
    }

    [Test]
    public async Task TestOnResultExecutionAsyncForTypedFailure()
    {
        var expectedException = new InvalidOperationException("Expected typed failure.");
        var actionContext = CreateActionContext();
        var context = CreateResultExecutingContext(
            actionContext,
            new ObjectResult(Result.Failure<object>(expectedException)));
        ExceptionHandler
            .Setup(handler => handler.TryHandleAsync(
                actionContext.HttpContext,
                expectedException,
                CancellationToken.None))
            .ReturnsAsync(true);

        await Filter.OnResultExecutionAsync(
            context,
            () => Task.FromResult(new ResultExecutedContext(actionContext, [], new EmptyResult(), null!)));

        ExceptionHandler.Verify(handler => handler.TryHandleAsync(
            actionContext.HttpContext,
            expectedException,
            CancellationToken.None), Times.Once);
    }

    private static ActionContext CreateActionContext(params object[] endpointMetadata)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.SetEndpoint(new Endpoint(
            _ => Task.CompletedTask,
            new EndpointMetadataCollection(endpointMetadata),
            "test"));

        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }

    private static ResultExecutingContext CreateResultExecutingContext(
        ActionContext actionContext,
        IActionResult result) =>
        new(actionContext, [], result, controller: null!);
}
