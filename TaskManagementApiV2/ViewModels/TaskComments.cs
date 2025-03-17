using System.ComponentModel.DataAnnotations;


namespace TaskManagementApiV2.ViewModels
{
    public class TaskCommentsModel
    {
        public class TaskComments
        {
            [Required]
            public Guid CommentId { get; set; }
            public Guid TaskId { get; set; }
            public string? Description { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime ModifyDate { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
        }

        public class InsertTaskComment
        {
            public Guid? CommentId { get; set; }
            public Guid TaskId { get; set; }
            public string? Description { get; set; }
        }
    }
}
