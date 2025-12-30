using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationSystem.Web.ViewModels.Account;

public class VerifyEmailViewModel
{
    [Required]
    [Display(Name = "Verification Token")]
    public string Token { get; set; } = string.Empty;
}
