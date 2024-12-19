using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BilgiYonetimSistem.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        public int? AdvisorID { get; set; }

        
        public virtual Advisor Advisor { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }
public ICollection<StudentCourseSelection> StudentCourseSelections { get; set; } = new List<StudentCourseSelection>();
    }
}

