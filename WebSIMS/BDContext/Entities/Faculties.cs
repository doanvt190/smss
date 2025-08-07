using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSIMS.BDContext.Entities
{
    public class Faculties
    {
        [Key]
        public int FacultyID { get; set; }

        [ForeignKey("Users")]
        public int UserID { get; set; }

        [Column("FirstName", TypeName = "nvarchar(50)"), Required]
        public string FirstName { get; set; }

        [Column("LastName", TypeName = "nvarchar(50)"), Required]
        public string LastName { get; set; }

        [Column("Email", TypeName = "nvarchar(100)"), Required]
        [EmailAddress]
        public string Email { get; set; }

        [Column("Phone", TypeName = "nvarchar(20)")]
        public string? Phone { get; set; }

        [Column("Department", TypeName = "nvarchar(100)")]
        public string? Department { get; set; }

        [Column("HireDate")]
        public DateTime? HireDate { get; set; }

        // Navigation property
        public virtual Users User { get; set; }
    }
} 