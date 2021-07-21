using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CalmR.Filters
{
    public class ApiException : Exception
    {
        public ApiException(string errorCode) :
            this("An error occurred while processing this request.", errorCode)
        {
        }

        public ApiException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public ApiException(string message, Exception? innerException, string errorCode) : base(message,
            innerException)
        {
            ErrorCode = errorCode;
        }

        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.BadRequest;
    
        public string ErrorCode { get; }
    }

    public class HandleExceptionsFilter : IActionFilter, IOrderedFilter
    {
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public HandleExceptionsFilter(ProblemDetailsFactory problemDetailsFactory)
        {
            _problemDetailsFactory = problemDetailsFactory;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var exception = context.Exception;
            if (exception is not ApiException apiException)
            {
                // The DeveloperExceptionPage will handle the exception for us, 
                // or, in production, ASP.NET Core's default error handling will
                // do the job
                return;
            }

            var statusCode = (int) apiException.StatusCode;
            var errorCode = apiException.ErrorCode; // A developer-friendly code for better localization
        
            context.Result = new ObjectResult(_problemDetailsFactory.CreateProblemDetails(
                context.HttpContext,
                statusCode,
                exception.Message,
                errorCode
            )) { StatusCode = statusCode };
            context.ExceptionHandled = true;
        }

        public int Order => int.MaxValue - 25;
    }
}