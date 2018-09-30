using System;
using System.Net;
using Alternatives.CustomExceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ProjectRenamer.Api.Responses;

namespace ProjectRenamer.Api.Filters
{
    public class GeneralExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public GeneralExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(this.GetType());
        }

        public void OnException(ExceptionContext context)
        {
            var errorResponse = new ErrorResponse();
            HttpStatusCode resultHttpStatusCode;
            if (context.Exception is CustomApiException customApiException)
            {
                errorResponse.AddErrorMessage(customApiException.FriendlyMessage);
                resultHttpStatusCode = customApiException.ReturnHttpStatusCode;
            }
            else
            {
                errorResponse.AddErrorMessage("Unexpected error occured", context.Exception);
                resultHttpStatusCode = HttpStatusCode.InternalServerError;
            }

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = (int)resultHttpStatusCode,
            };

            DoLogging(context.Exception, resultHttpStatusCode);
        }

        private void DoLogging(Exception contextException, HttpStatusCode resultHttpStatusCode)
        {
            if ((int)resultHttpStatusCode < 400)
            {
                _logger.LogInformation(contextException, contextException.Message);
            }
            else if ((int)resultHttpStatusCode < 500)
            {
                _logger.LogWarning(contextException, contextException.Message);
            }
            else
            {
                _logger.LogError(contextException, contextException.Message);
            }
        }
    }
}