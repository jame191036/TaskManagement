using Microsoft.EntityFrameworkCore;
using TaskManagementApiV2.ViewModels;
using static TaskManagementApiV2.ViewModels.TaskCommentsModel;
using static TaskManagementApiV2.ViewModels.TaskImageModel;
using static TaskManagementApiV2.ViewModels.TaskMasterStatusModel;
using static TaskManagementApiV2.ViewModels.TaskModel;
using static TaskManagementApiV2.ViewModels.NotificationEmailModel;

namespace TaskManagemen.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        // Define DbSet properties for your models
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TaskComments> TaskComments { get; set; }
        public DbSet<TaskImage> TaskImage { get; set; }
        public DbSet<TaskMasterStatus> TaskMasterStatus  { get; set; }
        public DbSet<NotificationEmail> NotificationEmail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tasks>().HasKey(t => t.TaskId); // Explicitly define the primary key
            modelBuilder.Entity<TaskComments>().HasKey(t => t.CommentId); // Explicitly define the primary key
            modelBuilder.Entity<TaskImage>().HasKey(t => t.ImageId); // Explicitly define the primary key
            modelBuilder.Entity<TaskMasterStatus>().HasKey(t => t.StatusId); // Explicitly define the primary key
            modelBuilder.Entity<NotificationEmail>().HasKey(t => t.EmailId); // Explicitly define the primary key
            base.OnModelCreating(modelBuilder);
        }
    }
}
