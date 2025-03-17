using System.ComponentModel.DataAnnotations;


namespace TaskManagementApiV2.ViewModels
{
    public class NotificationEmailModel
    {
        public class NotificationEmail
        {
            [Required]
            public Guid EmailId { get; set; }
            public string? Email { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime ModifyDate { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
        }

        public class NotificationEmailRequired
        {
            public string? Email { get; set; }
        }
    }
}
