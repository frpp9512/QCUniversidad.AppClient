using Microsoft.AspNetCore.Mvc;

namespace QCUniversidad.WebClient.Services.Platform;

public static class LoggingExtensions
{
    public static void LogRequest<T>(this ILogger<T> logger, HttpContext context)
        where T : Controller
    {
        logger.LogInformation($"Requested {context.Request.Path} using {context.Request.Method} from {context.Connection.RemoteIpAddress}.");
    }

    public static void LogModelNotExist<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogWarning($"The model of type {typeof(M).Name} was not found. Args: {string.Join(", ", args)}");
    }

    public static void LogCheckModelExistence<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogWarning($"Checking if the model of type {typeof(M).Name}. Args: {string.Join(", ", args)}");
    }

    public static void LogModelLoading<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogInformation($"Loading model of type {typeof(M).Name}. Args: {string.Join(",", args)}");
    }

    public static void LogModelSetLoading<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogInformation($"Loading list of model {typeof(M).Name}. Args: {string.Join(",", args)}");
    }

    public static void LogCreateModelRequest<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogInformation($"Requesting create new model of type {typeof(M).Name}. Args: {string.Join(",", args)}");
    }

    public static void LogModelCreated<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogInformation($"The model of type {typeof(M).Name} was created. Args: {string.Join(",", args)}");
    }

    public static void LogErrorCreatingModel<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogError($"Error creating model of type {typeof(M).Name}. Args: {string.Join(",", args)}");
    }

    public static void LogEditModelRequest<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogInformation($"Requesting edit model of type {typeof(M).Name}. Args: {string.Join(",", args)}");
    }

    public static void LogModelEdited<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogInformation($"The model of type {typeof(M).Name} was edited. Args: {string.Join(",", args)}");
    }

    public static void LogErrorEditingModel<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogError($"Error editing model of type {typeof(M).Name}. Args: {string.Join(",", args)}");
    }

    public static void LogDeleteModelRequest<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogInformation($"Requesting delete model of type {typeof(M).Name}. Args: {string.Join(",", args)}");
    }

    public static void LogModelDeleted<T, M>(this ILogger<T> logger, HttpContext context, params object?[] args)
        where T : Controller
    {
        logger.LogInformation($"The model of type {typeof(M).Name} was deleted. Args: {string.Join(",", args)}");
    }
}