using Microsoft.EntityFrameworkCore;
using WebSIMS.BDContext.Entities;

namespace WebSIMS.BDContext
{
    public class SIMSDBContext : DbContext
    {
        public SIMSDBContext(DbContextOptions<SIMSDBContext> options) : base(options) { }

        public DbSet<Courses> CoursesDb { get; set; }
        public DbSet<Users> UsersDb { get; set; }
        public DbSet<Students> StudentsDb { get; set; }
        public DbSet<Classes> ClassesDb { get; set; }
        public DbSet<Faculties> FacultiesDb { get; set; }
        public DbSet<StudentClassEnrollments> StudentClassEnrollmentsDb { get; set; }
        public DbSet<Schedules> SchedulesDb { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // table Users
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Users>().HasKey("UserID");
            modelBuilder.Entity<Users>().HasIndex("Username").IsUnique();
            modelBuilder.Entity<Users>().Property("Role").HasDefaultValue("Student");

            // table Courses
            modelBuilder.Entity<Courses>().ToTable("Courses");
            modelBuilder.Entity<Courses>().HasKey("CourseID");
            modelBuilder.Entity<Courses>().HasIndex("CourseCode").IsUnique();

            // table Students
            modelBuilder.Entity<Students>().ToTable("Students");
            modelBuilder.Entity<Students>().HasKey("StudentID");
            modelBuilder.Entity<Students>().HasIndex("Email").IsUnique();
            
            // Configure relationship between Users and Students
            modelBuilder.Entity<Students>()
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Students>(s => s.UserID);

            // table Classes
            modelBuilder.Entity<Classes>().ToTable("Classes");
            modelBuilder.Entity<Classes>().HasKey("ClassID");

            // table Faculties
            modelBuilder.Entity<Faculties>().ToTable("Faculties");
            modelBuilder.Entity<Faculties>().HasKey("FacultyID");
            modelBuilder.Entity<Faculties>().HasIndex("Email").IsUnique();

            // Configure relationship between Users and Faculties
            modelBuilder.Entity<Faculties>()
                .HasOne(f => f.User)
                .WithOne()
                .HasForeignKey<Faculties>(f => f.UserID);

            // Configure relationships for Classes
            modelBuilder.Entity<Classes>()
                .HasOne(c => c.Course)
                .WithMany()
                .HasForeignKey(c => c.CourseID);

            modelBuilder.Entity<Classes>()
                .HasOne(c => c.Faculty)
                .WithMany()
                .HasForeignKey(c => c.FacultyID);

            // table StudentClassEnrollments
            modelBuilder.Entity<StudentClassEnrollments>().ToTable("StudentClassEnrollments");
            modelBuilder.Entity<StudentClassEnrollments>().HasKey("EnrollmentID");

            // Configure relationships for StudentClassEnrollments
            modelBuilder.Entity<StudentClassEnrollments>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentID);

            modelBuilder.Entity<StudentClassEnrollments>()
                .HasOne(e => e.Class)
                .WithMany()
                .HasForeignKey(e => e.ClassID);

            // table Schedules
            modelBuilder.Entity<Schedules>().ToTable("Schedules");
            modelBuilder.Entity<Schedules>().HasKey("ScheduleID");

            // Configure relationships for Schedules
            modelBuilder.Entity<Schedules>()
                .HasOne(s => s.Class)
                .WithMany()
                .HasForeignKey(s => s.ClassID);
        }
    }
}
