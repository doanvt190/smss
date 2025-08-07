using System.ComponentModel.DataAnnotations;

namespace WebSIMS.Models
{
    public class CreateCourseViewModel
    {
        [Required(ErrorMessage = "Course Code is required")]
        [StringLength(20, ErrorMessage = "Course Code cannot exceed 20 characters")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course Name is required")]
        [StringLength(100, ErrorMessage = "Course Name cannot exceed 100 characters")]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Credits is required")]
        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10")]
        [Display(Name = "Credits")]
        public int Credits { get; set; }

        [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters")]
        [Display(Name = "Department")]
        public string? Department { get; set; }
    }
} 