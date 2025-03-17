using System.ComponentModel.DataAnnotations;


namespace TaskManagementApiV2.ViewModels
{
    public class TaskMasterStatusModel
    {
        public class TaskMasterStatus
        {
            [Required]
            public Guid StatusId { get; set; }
            public string? StatusCode { get; set; }
            public string? StatusName { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime ModifyDate { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
        }

        public class MasterStatus
        {
            public string Label { get; set; }
            public string Value { get; set; }
        }
    }
}
