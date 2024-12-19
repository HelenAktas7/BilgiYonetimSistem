using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BilgiYonetimSistem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(255)]
        public string PasswordHash { get; set; }

        [Required, StringLength(50)]
        public string Role { get; set; }

        public int? RelatedID { get; set; } //baska bir tabloyla olan baglantısını tutar

        [Required, StringLength(100)]
        public string Email { get; set; }
    }
}
