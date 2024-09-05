using AspNetCoreIdentityApp.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Web.ViewModels
{
  public class UserEditViewModel
  {
    [Required(ErrorMessage = "Kullanıcı Ad alanı boş bırakılamaz.")]
    [Display(Name = "Kullanıcı Adı :")]
    public string UserName { get; set; }


    [EmailAddress(ErrorMessage = "Email Formatı Yanlıştır.")]
    [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
    [Display(Name = "Email :")]
    public string Email { get; set; }


    [Required(ErrorMessage = "Telefon alanı boş bırakılamaz.")]
    [Display(Name = "Telefon :")]
    public string Phone { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Doğum Tarihi :")]
    public DateOnly BirthDate { get; set; }

    [Display(Name = "Şehir :")]
    public string City { get; set; }

    [Display(Name = "Profil Resmi :")]
    public IFormFile? Picture { get; set; }

    [Display(Name = "Cinsiyet :")]
    public Gender Gender { get; set; }

  }
}
