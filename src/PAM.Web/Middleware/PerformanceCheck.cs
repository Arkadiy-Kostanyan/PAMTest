using System.Diagnostics;
using MediatR;
using PAM.Core.Interfaces;

namespace PAM.Web.Middleware;

public class PerformanceCheck
{
  private readonly RequestDelegate _next;
  private readonly Stopwatch _timer;
  private readonly ILogger<PerformanceCheck> _logger;
  private readonly ICurrentUserService _currentUserService;


  public PerformanceCheck(RequestDelegate next, ILogger<PerformanceCheck> logger,
        ICurrentUserService currentUserService)
  {
    _next = next;
    _timer = new Stopwatch();
    _logger = logger;
    _currentUserService = currentUserService;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    _timer.Start();

    // Call the next delegate/middleware in the pipeline.
    await _next(context);

    _timer.Stop();

    var elapsedMilliseconds = _timer.ElapsedMilliseconds;

    if (elapsedMilliseconds > 1000)
    {
      var requestName = context.Request.Path + context.Request.QueryString;
      var userId = _currentUserService.UserId ?? string.Empty;
      var userName = _currentUserService.UserName ?? string.Empty;

      _logger.LogWarning("PAM Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {UserId} {UserName}",
          requestName, elapsedMilliseconds, userId, userName);
    }

  }
}

public static class PerformanceCheckerMiddlewareExtensions
{
  public static IApplicationBuilder UsePerformanceCheck(
      this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<PerformanceCheck>();
  }
}

