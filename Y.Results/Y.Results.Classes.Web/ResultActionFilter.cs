using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Y.Results.Web;

public class ResultActionFilter(IExceptionHandler exceptionHandler) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is not ObjectResult { Value: IResult result } httpResult)
        {
            await next();
            return;
        }

        if (!result.IsSuccess)
        {
            await HandleFailureAsync(context.HttpContext, result.Exception!);
            return;
        }

        context.Result = CreateSuccessResult(httpResult, context, result.Value);

        await next();
    }

    private async Task HandleFailureAsync(HttpContext httpContext, Exception exception)
    {
        await exceptionHandler.TryHandleAsync(
            httpContext,
            exception,
            cancellationToken: CancellationToken.None);
    }

    private static IActionResult CreateSuccessResult(
        ObjectResult httpResult, 
        ResultExecutingContext context,
        object? value)
    {
        if (value is FileContentResult fileResult)
        {
            return fileResult;
        }

        if (context.Result is OkObjectResult && value is null)
        {
            var hasNoContentOverride =
                context.HttpContext.GetEndpoint()?.Metadata
                    .GetMetadata<ReturnsNoContentOnSuccessAttribute>() is not null;

            return hasNoContentOverride ? new NoContentResult() : new OkResult();
        }

        httpResult.Value = value;
        return httpResult;
    }
}