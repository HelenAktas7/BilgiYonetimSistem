using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BilgiYonetimSistem.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [StringLength(20)]
        [Required]
        public string CourseCode { get; set; }
        [StringLength(100)]
        [Required]
        public string CourseName { get; set; }
        [Required]
        public bool IsMandatory { get; set; }
        [Required]
        public int Credit { get; set; }
        [StringLength(100)]
        [Required]
        public string Department { get; set; }
        public int? quata { get; set; }
        public ICollection<StudentCourseSelection> StudentCourseSelections { get; set; } = new List<StudentCourseSelection>();
    }
}
