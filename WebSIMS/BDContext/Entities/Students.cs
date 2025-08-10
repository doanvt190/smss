using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSIMS.BDContext.Entities
{
    public class Students
    {
        [Key]
        public int StudentID { get; set; }

        [ForeignKey("Users")]
        public int UserID { get; set; }

        [Column("FirstName", TypeName = "nvarchar(50)"), Required]
        public string FirstName { get; set; }

        [Column("LastName", TypeName = "nvarchar(50)"), Required]
        public string LastName { get; set; }

        [Column("DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("Gender", TypeName = "nvarchar(10)")]
        public string? Gender { get; set; }

        [Column("Email", TypeName = "nvarchar(100)"), Required]
        [EmailAddress]
        public string Email { get; set; }

        [Column("Phone", TypeName = "nvarchar(20)")]
        public string? Phone { get; set; }

        [Column("Address", TypeName = "nvarchar(255)")]
        public string? Address { get; set; }

        [Column("EnrollmentDate")]
        public DateTime? EnrollmentDate { get; set; }

        [Column("Program", TypeName = "nvarchar(100)")]
        public string? Program { get; set; }

        // Navigation property
        public virtual Users User { get; set; }
    }
} 