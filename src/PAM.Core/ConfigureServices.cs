using System.Reflection;
using MediatR;
using PAM.Core.Interfaces;
using PAM.SharedKernel.Interfaces;
using PAM.SharedKernel;
using PAM.Core.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {

    services.AddMediatR(Assembly.GetExecutingAssembly());
    services.AddScoped(typeof(IMediator), typeof(Mediator));
    services.AddScoped(typeof(IDomainEventDispatcher), typeof(DomainEventDispatcher));
    services.AddScoped(typeof(IAgreementsSearchService), typeof(AgreementsSearchService));

    return services;
  }
}

