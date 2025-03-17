using System.ComponentModel.DataAnnotations;


namespace TaskManagementApiV2.ViewModels
{
    public class TaskImageModel
    {
        public class TaskImage
        {
            [Required]
            public Guid ImageId { get; set; }
            public Guid TaskId { get; set; }
            public string ImageName { get; set; }
            public byte[] Source { get; set; }
            public string ContentType { get; set; }
            public decimal SizeInKB { get; set; }
            public decimal SizeInMB { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime ModifyDate { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}
