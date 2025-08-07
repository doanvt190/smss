using System.ComponentModel.DataAnnotations;

namespace WebSIMS.Models
{
    public class ClassListViewModel
    {
        public int ClassID { get; set; }

        [Display(Name = "Class Name")]
        public string ClassName { get; set; }

        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

        [Display(Name = "Course")]
        public string CourseName { get; set; }

        [Display(Name = "Faculty")]
        public string FacultyName { get; set; }

        [Display(Name = "Semester")]
        public string Semester { get; set; }

        [Display(Name = "Year")]
        public int Year { get; set; }

        public string DisplayName => $"{ClassName} - {CourseCode} ({FacultyName})";
    }
} 