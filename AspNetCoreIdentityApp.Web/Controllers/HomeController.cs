using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Services;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IEmailService _emailService;

    public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
    {
      _logger = logger;
      _userManager = userManager;
      _signInManager = signInManager;
      _emailService = emailService;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    public IActionResult Signup()
    {
      return View();
    }

    public IActionResult SignIn()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = null)
    {

      if (!ModelState.IsValid)
      {
        return View();
      }

      returnUrl = returnUrl ?? Url.Action("Index", "Home");

      var isUser = await _userManager.FindByEmailAsync(model.Email);

      if (isUser == null)
      {
        ModelState.AddModelError(string.Empty, "Email veya Þifre yanlýþ");
        return View();
      }


      var signInResult = await _signInManager.PasswordSignInAsync(isUser, model.Password, model.RememberMe, true);


      if (signInResult.IsLockedOut)
      {
        ModelState.AddModelErrorList(new List<string>() { "3 dakika boyunca giriþ yapamazsýnýz" });
        return View();
      }

      if (!signInResult.Succeeded)
      {
        ModelState.AddModelErrorList(new List<string>() { $"Email veya Þifre yanlýþ", $"Baþarýþýz giriþ sayýsý : {await _userManager.GetAccessFailedCountAsync(isUser)}" });
        return View();
      }

      if(!string.IsNullOrEmpty(isUser.BirthDate.ToString()))
      {
          await _signInManager.SignInWithClaimsAsync(isUser, model.RememberMe, new[] { new Claim("birthdate", isUser.BirthDate.ToString()) });
      }

        return Redirect(returnUrl);


    }

    [HttpPost]
    public async Task<IActionResult> Signup(SignUpViewModel request)
    {

      if (!ModelState.IsValid)
      {
        return View();
      }

      var identityResult = await _userManager.CreateAsync(new() { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email }, request.PasswordConfirm);

      if (!identityResult.Succeeded)
      {
        ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
        return View();
      }

      var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());

      var user = await _userManager.FindByNameAsync(request.UserName);

      var claimResult = await _userManager.AddClaimAsync(user, exchangeExpireClaim);

      if (!claimResult.Succeeded)
      {
        ModelState.AddModelErrorList(claimResult.Errors.Select(x => x.Description).ToList());
        return View();
      }


      TempData["SuccessMessage"] = "Üyelik kayýt iþlemi baþarýyla gerçekleþmiþtir.";


      return RedirectToAction(nameof(HomeController.Signup));


    }

    public IActionResult ForgetPassword()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
    {
      var hasUser = await _userManager.FindByEmailAsync(request.Email);

      if (hasUser == null)
      {
        ModelState.AddModelError(string.Empty, "Bu Email Adresine sahip kullanýcý bulunamamýþtýr.");
        return View();
      }

      string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

      var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

      //app password uqus bmjl esgg tkxv

      await _emailService.SendResetPasswordEmail(passwordResetLink, hasUser.Email);


      TempData["success"] = "Þifre sýfýrlama linki e-posta adresinize gönderilmiþtir.";
      return RedirectToAction(nameof(ForgetPassword));

    }

    public async Task<IActionResult> ResetPasswordAsync(string userId, string token)
    {
      TempData["userId"] = userId;
      TempData["token"] = token;

      await Task.CompletedTask;
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
    {
      var userId = TempData["userId"];
      var token = TempData["token"];

      if (userId == null || token == null)
      {
        throw new Exception("Bir hata meydana geldi");
      }


      var hasUser = await _userManager.FindByIdAsync(userId.ToString());

      if (hasUser == null)
      {
        ModelState.AddModelError(string.Empty, "Kullanýcý bulunamadý.");
        return View();
      }

      var result = await _userManager.ResetPasswordAsync(hasUser, token.ToString(), request.Password);

      if (result.Succeeded)
      {
        TempData["SuccessMessage"] = "Þifreniz baþarýyla yenilenmiþtir.";
      }
      else
      {
        ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
      }

      return View();

    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
