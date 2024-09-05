using AspNetCoreIdentityApp.Web.Models;

namespace AspNetCoreIdentityApp.Web.CustomValidations
{
  public class PasswordValidator : IPasswordValidator<AppUser>
  {
    public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
    {
      var errors = new List<IdentityError>();
      if(password.ToLower().Contains(user.UserName.ToLower()))
      {
        errors.Add(new() { Code = "PasswordContainsUserName", Description = "Şifre Alanı Kullanıcı Adını içeremez." });
      }

      if(password.ToLower().Contains("1234"))
      {
        errors.Add(new() { Code = "PasswordContains1234", Description = "Şifre Alanı Ardaşık sayı içeremez." });
      }

      if(errors.Any())
      {
        return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
      }
      
      return Task.FromResult(IdentityResult.Success);
    }
  }
}
