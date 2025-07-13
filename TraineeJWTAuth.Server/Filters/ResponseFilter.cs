using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TraineeJWTAuth.Server.Responces;

namespace TraineeJWTAuth.Server.Filters;

public class ResponseFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToArray();
            context.Result = new BadRequestObjectResult(new ApiResponse
            {
                Errors = errors
            });
            return;
        }

        if (context.Result is ObjectResult objectResult)
        {
            int statusCode = objectResult.StatusCode ?? 200;
            if (statusCode == 200)
            {
                objectResult.Value = new ApiResponse
                {
                    Data = objectResult.Value
                };
            }
            else
            {
                var response = new ApiResponse();
                if (objectResult.Value is IEnumerable<string>)
                {
                    response.Errors = objectResult.Value as IEnumerable<string>;
                }
                else if (objectResult.Value is string)
                {
                    response.Errors = new[] { objectResult.Value as string };
                }
                objectResult.Value = response;
            }
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}