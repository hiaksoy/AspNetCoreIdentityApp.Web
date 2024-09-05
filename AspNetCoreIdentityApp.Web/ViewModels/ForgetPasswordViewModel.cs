using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
  public class ForgetPasswordViewModel
  {
    [EmailAddress(ErrorMessage = "Email Formatı Yanlıştır.")]
    [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
    [Display(Name = "Email :")]
    public string Email { get; set; }
  }
}
