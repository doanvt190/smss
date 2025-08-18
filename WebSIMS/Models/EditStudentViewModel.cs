using System.ComponentModel.DataAnnotations;

namespace WebSIMS.Models
{
    public class EditStudentViewModel
    {
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(30, ErrorMessage = "Username cannot exceed 30 characters")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "New Password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? Phone { get; set; }

        [Display(Name = "Address")]
        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
        public string? Address { get; set; }

        [Display(Name = "Program")]
        [StringLength(100, ErrorMessage = "Program cannot exceed 100 characters")]
        public string? Program { get; set; }
    }
}
