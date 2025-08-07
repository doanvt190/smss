namespace WebSIMS.Models
{
    public class DashboardViewModel
    {
        public string SchoolName { get; set; }
        public string SchoolDescription { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalStudents { get; set; }
        public int TotalFaculty { get; set; }
        public int TotalCourses { get; set; }
        public int TotalClasses { get; set; }
        public List<StudentSummary> RecentStudents { get; set; } = new List<StudentSummary>();
        public List<FacultySummary> RecentFaculty { get; set; } = new List<FacultySummary>();
    }

    public class StudentSummary
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Program { get; set; }
        public DateTime? EnrollmentDate { get; set; }
    }

    public class FacultySummary
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public DateTime? HireDate { get; set; }
    }
} 