﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Credit.Core.Application.Exceptions;
using Credit.Core.Domain.Exceptions;

namespace Credit.Presentation.BackEnd.Filters
{
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionFilterAttribute(IWebHostEnvironment env) => _env = env;

        public override void OnException(ExceptionContext context)
        {
            var result = context.Exception switch
            {
                NotFoundException => CoreExceptionHandler(context, StatusCodes.Status404NotFound),
                CoreDomainException => CoreExceptionHandler(context, StatusCodes.Status400BadRequest),
                CoreApplicationException => CoreExceptionHandler(context, StatusCodes.Status400BadRequest),
                _ => InternalServerErrorExceptionHandler(context, StatusCodes.Status500InternalServerError)
            };

            if (result != null)
            {
                context.Result = result;
                context.ExceptionHandled = true;
            }
        }

        private IActionResult CoreExceptionHandler(ExceptionContext context, int statusCode)
        {
            var problemDetails = CreateProblemDetails(context, statusCode);

            if (context.Exception is CoreApplicationException domainEx)
                problemDetails.Extensions["errors"] = domainEx.Errors;

            if (_env.IsDevelopment())
                problemDetails.Extensions["exception"] = context.Exception.ToString();

            return new ObjectResult(problemDetails);
        }

        private IActionResult InternalServerErrorExceptionHandler(ExceptionContext context, int statusCode)
        {
            var ex = context.Exception as CoreApplicationException;
            var problemDetails = CreateProblemDetails(context, statusCode, ex?.Key);

            problemDetails.Detail = $"Error processing request, please contact the support.";

            if (_env.IsDevelopment())
                problemDetails.Extensions["exception"] = context.Exception.ToString();

            return new ObjectResult(problemDetails);
        }

        private static ProblemDetails CreateProblemDetails(ExceptionContext context, int statusCode, string? title = null) => new()
        {
            Title = title ?? context.Exception.GetType().Name,
            Instance = context.HttpContext.Request.Path.Value,
            Detail = context.Exception.Message,
            Status = statusCode,
        };
    }
}
