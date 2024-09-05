using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace AspNetCoreIdentityApp.Web.Localization
{
  public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
  {
    public override IdentityError DuplicateUserName(string userName)
    {
      return new() { Code = "DuplicateUserName", Description = $"{userName} Daha önce alınmış." };

    }

    public override IdentityError DuplicateEmail(string email)
    {
      return new() { Code = "DuplicateEmail", Description = $"{email} Daha önce alınmış." };
    }

    public override IdentityError PasswordTooShort(int length)
    {
      return new() { Code = "PasswordTooShort", Description = $"Şifre en az 6 karakter olmalıdır." };
    }

  }
}
