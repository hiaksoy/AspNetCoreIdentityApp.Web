using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentityApp.Web.Requirements
{
  public class ViolanceRequirement : IAuthorizationRequirement
  {
    public int TresholdAge { get; set; }

  }

  public class ViolanceRequirementHandler : AuthorizationHandler<ViolanceRequirement>
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolanceRequirement requirement)
    {
    
      if (!context.User.HasClaim(x => x.Type == "birthdate"))
      {
        context.Fail();
        return Task.CompletedTask;
      }

      var birthDateClaim = context.User.FindFirst("birthdate");

      var today = DateTime.Now.Date;
      var birthDate = Convert.ToDateTime(birthDateClaim.Value);
      var age = today.Year - birthDate.Year;

      if (birthDate > today.AddYears(-age)) age--;

      if (requirement.TresholdAge > age)
      {
        context.Fail();
        return Task.CompletedTask;
      }

      context.Succeed(requirement);
      return Task.CompletedTask;


    }
  }

}
