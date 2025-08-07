using System.ComponentModel.DataAnnotations;

namespace WebSIMS.Models
{
    public class CourseListViewModel
    {
        public int CourseID { get; set; }

        [Display(Name = "Course Code")]
        public string CourseCode { get; set; } = string.Empty;

        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Credits")]
        public int Credits { get; set; }

        [Display(Name = "Department")]
        public string? Department { get; set; }

        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; }
    }
} 