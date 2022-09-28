using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MediatR;
using MediatR.Pipeline;
using System.Reflection;
using PAM.Core.Interfaces;
using PAM.Core.AgreementAggregate;
using PAM.Infrastructure.Data;
using PAM.SharedKernel;
using PAM.SharedKernel.Interfaces;
using PAM.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;
public static class ConfigureServices
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
  {

    services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

    return services;
  }
}
