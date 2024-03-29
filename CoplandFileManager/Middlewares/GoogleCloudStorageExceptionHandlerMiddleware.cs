﻿namespace CoplandFileManager.Middlewares;

using CoplandFileManager.Domain.File.Exceptions;
using CoplandFileManager.Domain.SharedKernel.Exceptions;
using CoplandFileManager.Domain.User.Exceptions;
using System.Net;
using System.Text.Json;

public class GoogleCloudStorageExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GoogleCloudStorageExceptionHandlerMiddleware> _logger;

    public GoogleCloudStorageExceptionHandlerMiddleware(RequestDelegate next, ILogger<GoogleCloudStorageExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex) when (ex is UserNotFoundException or FileNotFoundException)
        {
            await HandleNotFoundExceptionAsync(context, ex);
        }
        catch (BusinessException ex)
        {
            await HandleBusinessExceptionAsync(context, ex);
        }
        catch (Google.GoogleApiException ex) when (ex.Error != null)
        {
            await HandleGoogleApiExceptionAsync(context, ex);
        }
        catch (JsonException ex)
        {
            await HandleJsonExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleGenericExceptionAsync(context);
        }
    }

    private Task HandleBusinessExceptionAsync(HttpContext context, BusinessException ex)
    {
        context.Response.ContentType = "application/json";      
        var statusCode = (int)HttpStatusCode.InternalServerError;
        string errorMessage = ex.Message;
        context.Response.StatusCode = statusCode;
        var result = JsonSerializer.Serialize(new { message = errorMessage });
        return context.Response.WriteAsync(result);
    }

    private Task HandleNotFoundExceptionAsync(HttpContext context, BusinessException ex)
    {
        context.Response.ContentType = "application/json";
        var statusCode = (int)HttpStatusCode.NotFound;
        string errorMessage = ex.Message;
        context.Response.StatusCode = statusCode;
        var result = JsonSerializer.Serialize(new { message = errorMessage });
        return context.Response.WriteAsync(result);
    }


    private Task HandleGoogleApiExceptionAsync(HttpContext context, Google.GoogleApiException ex)
    {
        context.Response.ContentType = "application/json";

        var statusCode = (int)HttpStatusCode.InternalServerError;
        string errorMessage = "An unknown error occurred.";

        if (ex.Error != null)
        {
            statusCode = (int)ex.HttpStatusCode;
            errorMessage = ex.Error.Message ?? "Lain detect an error while processing the request.";
        }

        context.Response.StatusCode = statusCode;
        var result = JsonSerializer.Serialize(new { message = errorMessage });
        return context.Response.WriteAsync(result);
    }

    private Task HandleGenericExceptionAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new { message = "Lain detect an error while processing the request." });
        return context.Response.WriteAsync(result);
    }

    private Task HandleJsonExceptionAsync(HttpContext context, JsonException ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        var result = JsonSerializer.Serialize(new { message = "Invalid JSON in request body." });
        return context.Response.WriteAsync(result);
    }
}