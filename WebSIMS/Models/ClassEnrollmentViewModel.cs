using System.ComponentModel.DataAnnotations;

namespace WebSIMS.Models
{
    public class ClassEnrollmentViewModel
    {
        public int EnrollmentID { get; set; }

        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Display(Name = "Student Email")]
        public string StudentEmail { get; set; }

        [Display(Name = "Class Name")]
        public string ClassName { get; set; }

        [Display(Name = "Course")]
        public string CourseName { get; set; }

        [Display(Name = "Faculty")]
        public string FacultyName { get; set; }

        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
    }
} 