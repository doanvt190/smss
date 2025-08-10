using System.ComponentModel.DataAnnotations;

namespace WebSIMS.Models
{
    public class FacultyListViewModel
    {
        public int FacultyID { get; set; }
        
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
        
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Phone")]
        public string? Phone { get; set; }
        
        [Display(Name = "Department")]
        public string? Department { get; set; }
        
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; }
        
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
} 