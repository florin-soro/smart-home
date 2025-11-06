using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Thermostat.DataAccessLayer.Exceptions;
using Thermostat.Domain.Common.Exceptions;
using Thermostat.Domain.Domain.HouseAgg.Exceptions;

public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var (statusCode, message) = context.Exception switch
        {
            DomainRuleValidationException exception => (HttpStatusCode.UnprocessableContent, exception.Message),
            MeasurementEarlierOrEqualToLastMeasurementException ex => (HttpStatusCode.Conflict, ex.Message),
            EntityNotFoundException ex => (HttpStatusCode.NotFound, ex.Message),
            DomainException ex => (HttpStatusCode.BadRequest, ex.Message),
            ApplicationException ex => (HttpStatusCode.BadRequest, ex.Message),
            InfrastructureException ex => (HttpStatusCode.InternalServerError, "An internal infrastructure error occurred."),

            _ => ((HttpStatusCode)500, "An unexpected error occurred.") // Corrected
        };

        if (statusCode >= HttpStatusCode.InternalServerError)
        {
            logger.LogError(context.Exception, "An unhandled exception occurred: {Message}", context.Exception.Message);
        }
        else
        {
            logger.LogWarning(context.Exception, "A client-side error occurred: {Message}", context.Exception.Message);
        }

        context.Result = new ObjectResult(new { error = message })
        {
            StatusCode = (int)statusCode
        };

        context.ExceptionHandled = true;
    }
}