using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BilgiYonetimSistem.Models
{
    public class StudentCourseSelection
    {
        [Key]
        public int SelectionID { get; set; }
        [Required]
        public int StudentID { get; set; }
        [Required]
        public int CourseID { get; set; }
        [Required]
        public DateTime SelectionDate { get; set; }
        public bool IsApproved { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
