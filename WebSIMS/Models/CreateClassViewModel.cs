using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebSIMS.Models
{
    public class CreateClassViewModel
    {
        [Required(ErrorMessage = "Class name is required")]
        [Display(Name = "Class Name")]
        [StringLength(100, ErrorMessage = "Class name cannot exceed 100 characters")]
        public string ClassName { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public int CourseID { get; set; }

        [Required(ErrorMessage = "Faculty is required")]
        [Display(Name = "Faculty")]
        public int FacultyID { get; set; }

        [Required(ErrorMessage = "Semester is required")]
        [Display(Name = "Semester")]
        [StringLength(20, ErrorMessage = "Semester cannot exceed 20 characters")]
        public string Semester { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Display(Name = "Year")]
        [Range(2020, 2030, ErrorMessage = "Year must be between 2020 and 2030")]
        public int Year { get; set; }

        // For dropdown lists
        public List<SelectListItem> Courses { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Faculties { get; set; } = new List<SelectListItem>();
    }
} 