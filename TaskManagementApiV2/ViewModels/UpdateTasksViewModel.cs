using System.ComponentModel.DataAnnotations;


namespace TaskManagementApiV2.ViewModels
{
    public class UpdateTasksViewModel
    {
        public Guid? TaskId { get; set; }
        public string? TaskNo { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
