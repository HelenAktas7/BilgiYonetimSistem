using BilgiYonetimSistem.Models;

public class PendingCourseSelection
{
    public int Id { get; set; } // Kayıt ID'si
    public int StudentId { get; set; } // Öğrenci ID'si
    public int CourseId { get; set; } // Ders ID'si
    public DateTime SelectionDate { get; set; } // Seçim tarihi
    public bool IsConfirmed { get; set; } // Onaylanmış mı?

    // İlişkili veriler
    public Student? Student { get; set; }
    public Course? Course { get; set; }
}
