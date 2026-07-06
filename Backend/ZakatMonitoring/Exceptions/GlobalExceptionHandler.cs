using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SharedKernel;

namespace API.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		int statusCode;
		Result<string> message;
		if (!(exception is ValidationException ex))
		{
			if (exception is NotFoundException)
			{
				statusCode = 404;
				message = new Result<string>
				{
					Errors = new List<Error> { Error.NotFound(statusCode.ToString(), exception.Message) }
				};
			}
			else
			{
				statusCode = 500;
				message = new Result<string>
				{
					Errors = new List<Error>
					{
						new Error(statusCode.ToString(), exception.Message, (ErrorType)0)
					}
				};
			}
		}
		else
		{
			statusCode = 400;
			message = new Result<string>
			{
				Errors = ex.Errors.Select((ValidationFailure e) => Error.Validation(statusCode.ToString(), e.ErrorMessage)).ToList()
			};
		}
		httpContext.Response.StatusCode = statusCode;
		httpContext.Response.ContentType = "application/json";
		await httpContext.Response.WriteAsJsonAsync(message, cancellationToken);
		return true;
	}
}
