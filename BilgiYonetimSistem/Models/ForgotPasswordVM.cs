using System.ComponentModel.DataAnnotations;
namespace BilgiYonetimSistem.Models
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
} 
