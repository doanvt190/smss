using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSIMS.BDContext.Entities
{
    public class Schedules
    {
        [Key]
        public int ScheduleID { get; set; }

        [ForeignKey("Classes")]
        public int ClassID { get; set; }

        [Column("DayOfWeek", TypeName = "nvarchar(20)"), Required]
        public string DayOfWeek { get; set; }

        [Column("StartTime", TypeName = "time"), Required]
        public TimeSpan StartTime { get; set; }

        [Column("EndTime", TypeName = "time"), Required]
        public TimeSpan EndTime { get; set; }

        [Column("Room", TypeName = "nvarchar(50)"), Required]
        public string Room { get; set; }

        // Navigation properties
        public virtual Classes Class { get; set; }
    }
} 