using System.ComponentModel.DataAnnotations;

namespace RegisteredUser.Model
{
    public class tblTask
    {
        public int UserId { get; set; }
        [Key]
        public int TaskId { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;
        public string TaskStatus { get; set; } = string.Empty;
        public string TaskCategory { get; set; } = string.Empty;
        public string TaskPriority { get; set; } = string.Empty;
        public DateTime TaskCreatedDate { get; set; }
        public DateTime TaskTargetDate { get; set; }
        public DateTime TaskModifiedDate { get; set;}
        public DateTime TaskCompletedDate { get; set; }

    }
}
