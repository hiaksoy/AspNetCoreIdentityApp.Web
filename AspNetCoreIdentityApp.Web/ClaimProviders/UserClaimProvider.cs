using AspNetCoreIdentityApp.Web.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.ClaimProviders
{
  public class UserClaimProvider : IClaimsTransformation
  {
    private readonly UserManager<AppUser> _userManager;

    public UserClaimProvider(UserManager<AppUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
      var identity = principal.Identity as ClaimsIdentity;

      var currentUser = await _userManager.FindByNameAsync(identity.Name);


      if (string.IsNullOrEmpty(currentUser.City))
      {
        return principal;
      }

      if (principal.HasClaim(x => x.Type != "city"))
      {
        Claim cityClaim = new Claim("city", currentUser.City);
        identity.AddClaim(cityClaim);
      }

      return principal;

    }
  }
}
