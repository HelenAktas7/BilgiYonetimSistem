using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BilgiYonetimSistem.Models
{
    public class Advisor
    {
        [Key]
        public int AdvisorId { get; set; }
        [StringLength(100)]
        [Required]
        public string FullName { get; set; }
        [StringLength(50)]
        [Required]
        public string Title { get; set; }
        [StringLength(100)]
        [Required]
        public string Department { get; set; }
        [StringLength(100)]
        [Required]
        public string Email { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
