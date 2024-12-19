using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BilgiYonetimSistem.Models
{
    public class Transcript
    {
        [Key]
        public int TranscriptID { get; set; }

        [Required]
        [ForeignKey("Student")]
        public int StudentID { get; set; }
        public virtual Student Student { get; set; }

        [Required] 
        [ForeignKey("Course")]
        public int CourseID { get; set; }

        public virtual Course Course { get; set; }

        [StringLength(2)]
        public string? Grade { get; set; }

        [Required, StringLength(20)]
        public string Semester { get; set; }

    
    }
}
