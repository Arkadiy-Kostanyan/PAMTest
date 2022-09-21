using System.Reflection;
using MediatR;
using PAM.Core.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {

    services.AddMediatR(Assembly.GetExecutingAssembly());
    //services.AddScoped(typeof(IToDoItemSearchService), typeof(ToDoItemSearchService));
    return services;
  }
}

