using System.ComponentModel.DataAnnotations;

namespace WebSIMS.Models
{
    public class TimetableViewModel
    {
        public int ScheduleID { get; set; }
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string FacultyName { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Room { get; set; }
        public string Semester { get; set; }
        public int Year { get; set; }
    }

    public class CreateTimetableViewModel
    {
        [Required(ErrorMessage = "Please select a class")]
        [Display(Name = "Class")]
        public int ClassID { get; set; }

        [Required(ErrorMessage = "Please select a day of the week")]
        [Display(Name = "Day of Week")]
        public string DayOfWeek { get; set; }

        [Required(ErrorMessage = "Please enter start time")]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Please enter end time")]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Please enter room number")]
        [Display(Name = "Room")]
        [StringLength(50, ErrorMessage = "Room cannot exceed 50 characters")]
        public string Room { get; set; }

        // For dropdown lists
        public List<ClassListViewModel> AvailableClasses { get; set; } = new List<ClassListViewModel>();
        public List<string> DaysOfWeek { get; set; } = new List<string>
        {
            "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"
        };

        // Predefined time slots
        public List<TimeSlotOption> TimeSlotOptions { get; set; } = new List<TimeSlotOption>
        {
            new TimeSlotOption { DisplayName = "Time slot 1 (7:15-9:15)", StartTime = new TimeSpan(7, 15, 0), EndTime = new TimeSpan(9, 15, 0) },
            new TimeSlotOption { DisplayName = "Time slot 2 (9:25-11:25)", StartTime = new TimeSpan(9, 25, 0), EndTime = new TimeSpan(11, 25, 0) },
            new TimeSlotOption { DisplayName = "Time slot 3 (12:00-14:00)", StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(14, 0, 0) },
            new TimeSlotOption { DisplayName = "Time slot 4 (14:10-16:10)", StartTime = new TimeSpan(14, 10, 0), EndTime = new TimeSpan(16, 10, 0) },
            new TimeSlotOption { DisplayName = "Time slot 5 (16:20-18:20)", StartTime = new TimeSpan(16, 20, 0), EndTime = new TimeSpan(18, 20, 0) }
        };
    }

    public class TimeSlotOption
    {
        public string DisplayName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public class EditTimetableViewModel
    {
        public int ScheduleID { get; set; }

        [Required(ErrorMessage = "Please select a class")]
        [Display(Name = "Class")]
        public int ClassID { get; set; }

        [Required(ErrorMessage = "Please select a day of the week")]
        [Display(Name = "Day of Week")]
        public string DayOfWeek { get; set; }

        [Required(ErrorMessage = "Please enter start time")]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Please enter end time")]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Please enter room number")]
        [Display(Name = "Room")]
        [StringLength(50, ErrorMessage = "Room cannot exceed 50 characters")]
        public string Room { get; set; }

        // For dropdown lists
        public List<ClassListViewModel> AvailableClasses { get; set; } = new List<ClassListViewModel>();
        public List<string> DaysOfWeek { get; set; } = new List<string>
        {
            "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"
        };

        // Predefined time slots
        public List<TimeSlotOption> TimeSlotOptions { get; set; } = new List<TimeSlotOption>
        {
            new TimeSlotOption { DisplayName = "Time slot 1 (7:15-9:15)", StartTime = new TimeSpan(7, 15, 0), EndTime = new TimeSpan(9, 15, 0) },
            new TimeSlotOption { DisplayName = "Time slot 2 (9:25-11:25)", StartTime = new TimeSpan(9, 25, 0), EndTime = new TimeSpan(11, 25, 0) },
            new TimeSlotOption { DisplayName = "Time slot 3 (12:00-14:00)", StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(14, 0, 0) },
            new TimeSlotOption { DisplayName = "Time slot 4 (14:10-16:10)", StartTime = new TimeSpan(14, 10, 0), EndTime = new TimeSpan(16, 10, 0) },
            new TimeSlotOption { DisplayName = "Time slot 5 (16:20-18:20)", StartTime = new TimeSpan(16, 20, 0), EndTime = new TimeSpan(18, 20, 0) }
        };
    }

    public class TimetableDisplayViewModel
    {
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public List<TimetableViewModel> TimetableData { get; set; } = new List<TimetableViewModel>();
        public Dictionary<string, Dictionary<string, TimetableViewModel>> TimetableGrid { get; set; } = new Dictionary<string, Dictionary<string, TimetableViewModel>>();
        
        // Time slots for the vertical axis - 5 specific slots as requested
        public List<string> TimeSlots { get; set; } = new List<string>
        {
            "Time slot 1 (7:15-9:15)",
            "Time slot 2 (9:25-11:25)", 
            "Time slot 3 (12:00-14:00)",
            "Time slot 4 (14:10-16:10)",
            "Time slot 5 (16:20-18:20)"
        };
    }
} 