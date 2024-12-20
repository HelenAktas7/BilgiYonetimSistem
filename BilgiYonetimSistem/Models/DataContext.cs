using Microsoft.EntityFrameworkCore;
namespace BilgiYonetimSistem.Models
{
    public class DataContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=HELEN\\SQLEXPRESS04;initial Catalog=BysDB;integrated security=True;");
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Advisor> Advisors { get; set; }
        public  DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourseSelection> StudentCourseSelections { get; set; }
        public DbSet<Transcript> Transcripts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PendingSelection> PendingSelections { get; set; }
       

    }
}
