namespace BilgiYonetimSistem.DataTransfer
{
    public class StudentDto
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? AdvisorID { get; set; }

        public AdvisorDto Advisor { get; set; }
        public List<CourseDto> Courses { get; set; }
    }

    public class AdvisorDto
    {
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
    }

    public class CourseDto
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public DateTime SelectionDate { get; set; }
    }
}
