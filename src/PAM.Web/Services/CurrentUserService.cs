using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PAM.Core.Interfaces;

namespace PAM.Web.Services;
public class CurrentUserService : ICurrentUserService
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  //private readonly UserManager<IdentityUser> _manager;

  public CurrentUserService(IHttpContextAccessor httpContextAccessor/*, UserManager<IdentityUser> manager*/)
  {
    _httpContextAccessor = httpContextAccessor;
    //_manager = manager;
  }

  /*
  public async Task<IdentityUser> GetCurrentUser()
  {
    return await _manager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
  }
  */

  public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

  public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;

  //public string? UserEmail => GetCurrentUser().GetAwaiter().GetResult().Email;
}
