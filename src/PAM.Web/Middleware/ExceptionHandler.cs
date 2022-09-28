using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PAM.Core.Interfaces;
using Serilog;

namespace PAM.Web.Middleware;

public enum ErrorType
{
  NotFound = 404,
  Application = 500,
  Conflict = 409
}

public class ErrorViewModel
{
  public ErrorType ErrorType { get; set; }
  public string Message { get; set; }
  public string ErrorId { get; set; }
  public string? UserId { get; set; }
}
public class ExceptionHandler
{
  private readonly RequestDelegate _next;

  private readonly ILogger<PerformanceCheck> _logger;
  private readonly ICurrentUserService _currentUserService;

  public ExceptionHandler(RequestDelegate next, ILogger<PerformanceCheck> logger,
        ICurrentUserService currentUserService)
  {
    _next = next ?? throw new ArgumentNullException(nameof(next));
    _logger = logger;
    _currentUserService = currentUserService;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {

      var requestName = context.Request.Path + context.Request.QueryString;
      var userId = _currentUserService.UserId ?? string.Empty;
      var userName = _currentUserService.UserName ?? string.Empty;

      _logger.LogError("Error executing request. Request: {Name} ErrorId: {ErrorId} Error: {Error} UserId: {UserId} UserName: {UserName}",
          requestName, System.Diagnostics.Activity.Current?.Id ?? context.TraceIdentifier, ex.Message, userId, userName);

      var errorType = ErrorType.Application;

      var jsonResponse = JsonConvert.SerializeObject(
          new ErrorViewModel
          {
            ErrorId = System.Diagnostics.Activity.Current?.Id ?? context.TraceIdentifier,
            Message = ex.Message,
            ErrorType = errorType,
            UserId = _currentUserService?.UserId
          });

      context.Response.StatusCode = (int)errorType;
      context.Response.ContentType = "application/json";

      await context.Response.WriteAsync(jsonResponse);
    }
  }
}

public static class ExceptionMiddlewareExtensions
{
  public static IApplicationBuilder UseGlobalExceptionHandler(
      this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<ExceptionHandler>();
  }
}
