using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSIMS.BDContext.Entities
{
    public class StudentClassEnrollments
    {
        [Key]
        public int EnrollmentID { get; set; }

        [ForeignKey("Students")]
        public int StudentID { get; set; }

        [ForeignKey("Classes")]
        public int ClassID { get; set; }

        [Column("EnrollmentDate")]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        [Column("Status", TypeName = "nvarchar(20)")]
        public string Status { get; set; } = "Active"; // Active, Dropped, Completed

        // Navigation properties
        public virtual Students Student { get; set; }
        public virtual Classes Class { get; set; }
    }
} 