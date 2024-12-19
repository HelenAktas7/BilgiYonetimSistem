namespace BilgiYonetimSistem.DataTransfer
{
    public class CourseSelectionDTO
    {
        public int studentId { get; set; }
        public int courseId { get; set; }
        public DateTime selectionDate { get; set; }
        public bool isConfirmed { get; set; }
    }
}
