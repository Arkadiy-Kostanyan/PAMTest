namespace PAM.Core.Interfaces;

public interface ICurrentUserService
{
  string? UserId { get; }
  string? UserName { get; }

  //string? UserEmail { get; }
  //Task<IdentityUser> GetCurrentUser();

}
