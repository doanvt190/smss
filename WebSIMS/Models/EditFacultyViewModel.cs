using System.ComponentModel.DataAnnotations;

namespace WebSIMS.Models
{
    public class EditFacultyViewModel
    {
        [Required]
        public int FacultyID { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [StrongPassword]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repeat New Password")]
        [Compare("NewPassword", ErrorMessage = "Repeat password must match the password")]
        public string? ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(20)]
        [Phone]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        [StringLength(100)]
        [Display(Name = "Department")]
        public string? Department { get; set; }

        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }
    }
}
