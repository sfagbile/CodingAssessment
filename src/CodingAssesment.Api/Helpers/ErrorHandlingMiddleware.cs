using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace CodingAssessment.Api.Helpers
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var errorResponse = new ErrorResponse();
            try
            {
                await next(context);
            }
            catch (ConflictException conflict)
            {
                errorResponse = conflict.ErrorResponse;
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await PopulateResponse(context, errorResponse);
            }
            catch (NotFoundException notFound)
            {
                errorResponse = notFound.ErrorResponse;
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await PopulateResponse(context, errorResponse);
            }
            catch (BadRequestException bRequest)
            {
                errorResponse = bRequest.ErrorResponse;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await PopulateResponse(context, errorResponse);
            }
            catch (Exception ex)
            {

                errorResponse = new ErrorResponse
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await PopulateResponse(context, errorResponse);
            }
        }

        private async Task PopulateResponse(HttpContext context, ErrorResponse errorResponse)
        {
            context.Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(json);
            logger.LogError(json);
        }
    }

}
