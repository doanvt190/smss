namespace WebSIMS.Models
{
    public class StudentListViewModel
    {
        public int StudentID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string? Program { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
} 