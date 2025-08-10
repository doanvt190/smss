using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSIMS.BDContext.Entities
{
    public class Classes
    {
        [Key]
        public int ClassID { get; set; }

        [Column("ClassName", TypeName = "nvarchar(100)"), Required]
        public string ClassName { get; set; }

        [ForeignKey("Courses")]
        public int CourseID { get; set; }

        [ForeignKey("Faculties")]
        public int FacultyID { get; set; }

        [Column("Semester", TypeName = "nvarchar(20)"), Required]
        public string Semester { get; set; }

        [Column("Year", TypeName = "integer"), Required]
        public int Year { get; set; }

        // Navigation properties
        public virtual Courses Course { get; set; }
        public virtual Faculties Faculty { get; set; }
    }
} 