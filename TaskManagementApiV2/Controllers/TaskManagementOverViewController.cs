using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using TaskManagementApiV2.ViewModels;
using TaskManagemen.Data;
using static TaskManagementApiV2.ViewModels.TaskModel;
using System.Configuration;
using Azure.Core;

namespace TaskManagementApiV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")] // Use specific policy here
    public class TaskManagementOverViewController : ControllerBase
    {
        private readonly ILogger<TaskManagementOverViewController> _logger;
        private readonly ApplicationDbContext _context;

        public TaskManagementOverViewController(ILogger<TaskManagementOverViewController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("GetOverView")]
        public IActionResult GetOverView([FromBody] FilterTask filter)
        {
            try
            {
                //List<TaskManagementApiV2.ViewModels.TaskModel.Tasks> tasks = _context.Tasks.ToList();
                var task1s = (from a in _context.Tasks

                              join b in _context.TaskMasterStatus on a.Status equals b.StatusCode into b_join
                              from b in b_join.DefaultIfEmpty()

                              orderby a.ModifyDate descending
                              where a.IsActive == true
                              && a.IsDeleted == false
                              && ((!string.IsNullOrEmpty(filter.TaskNo) && a.TaskNo.ToLower().Contains(filter.TaskNo.ToLower())) || string.IsNullOrEmpty(filter.TaskNo))
                              && ((!string.IsNullOrEmpty(filter.Title) && a.Title.ToLower().Contains(filter.Title.ToLower())) || string.IsNullOrEmpty(filter.Title))
                              select new TaskResponse
                              {
                                  TaskId = a.TaskId,
                                  Title = a.Title,
                                  Description = a.Description,
                                  Status = a.Status,
                                  StatusName = b.StatusName,
                                  TaskNo = a.TaskNo,
                                  CreateDate = a.CreateDate,
                                  ModifyDate = a.ModifyDate,
                              }).ToList();

                return Ok(task1s);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTaskById/{taskId}")]
        public IActionResult GetTaskById(Guid taskId)
        {
            try
            {
                var result = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId && t.IsActive == true && t.IsDeleted == false);
                if (result == null)
                {
                    return NotFound("Task not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("CreateTask")]
        //public async Task<IActionResult> CreateTask()
        //{

        //}

        [HttpPost("UpdateTask")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTasksViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid data.");
                }

                string subjectEmail = "";
                string messageEmail = "";

                if (model.TaskId == null)
                {
                    string taskNo = this.CreateTaskNo();

                    var newTask = new TaskModel.Tasks
                    {
                        TaskId = Guid.NewGuid(), // Generate a new GUID for TaskId
                        TaskNo = taskNo,
                        Title = model.Title,
                        Description = model.Description,
                        Status = model.Status ?? "Pending", // Default status if none provided
                        CreateDate = DateTime.Now,
                        ModifyDate = DateTime.Now,
                        IsActive = model.IsActive ?? true,
                        IsDeleted = false // Default to false for a new task
                    };
                    _context.Tasks.Add(newTask);

                    model.TaskId = newTask.TaskId;

                    subjectEmail = "มีการเพิ่ม Task ใหม่";
                    messageEmail = $"<p>มีการเพิ่ม Task ใหม่เลขที่ {newTask.TaskNo} เรื่อง {newTask.Title} <a target=\"_blank\" rel=\"noopener noreferrer\" href=\"http://localhost:5173/task/{newTask.TaskId}\">คลิก</a></p>\r\n";
                }
                else
                {
                    var task = _context.Tasks.FirstOrDefault(t => t.TaskId == model.TaskId);

                    if (task != null)
                    {
                        task.Title = model.Title;
                        task.Description = model.Description;
                        task.Status = model.Status;
                        task.ModifyDate = DateTime.Now;
                    }
                    subjectEmail = "มีการเปลี่ยนแปลง Task";
                    messageEmail = $"<p>Task ที่ {task.TaskNo} มีการการเปลี่ยนแปลง <a target=\"_blank\" rel=\"noopener noreferrer\" href=\"http://localhost:5173/task/{task.TaskId}\">คลิก</a></p>\r\n";
                }

                _context.SaveChanges();

                var emailEntity = (from a in _context.NotificationEmail
                                   where a.IsActive == true && a.IsDeleted == false
                                   orderby a.ModifyDate descending
                                   select a).FirstOrDefault();

                if (emailEntity != null)
                {
                    var sendEmail = await SendEmailAsync(emailEntity.Email ?? "", subjectEmail, messageEmail);
                }

                return Ok(model.TaskId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("DeleteTaskById")]
        public IActionResult DeleteTaskById(Guid taskId)
        {
            try
            {
                var result = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId && t.IsActive == true && t.IsDeleted == false);

                if (result == null)
                {
                    return NotFound("Task not found");
                }

                result.IsActive = false;
                result.IsDeleted = true;

                _context.SaveChanges();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string CreateTaskNo()
        {
            var task1s = (from a in _context.Tasks select a).Count();
            var currentYear = DateTime.Now.Year;
            string result = $"{currentYear}-{(++task1s):D5}";
            return result;
        }

        [HttpGet("SendEmail")]
        public async Task<IActionResult> SendEmail()
        {
            var send = await SendEmailAsync("piyawat2163@gmail.com", "test", "testeeeeeee");
            return Ok(send);
        }

        private async Task<bool> SendEmailAsync(string email, string subjectEmail, string messageEmail)
        {
            // Configure your SMTP settings
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587; // Common ports: 25, 465, 587
            string smtpUsername = "jamepiyawat66@gmail.com";
            string smtpPassword = "evgx znrc xkxx gqgu";
            bool enableSsl = true;

            try
            {
                // Create a new MailMessage
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername), // Set a valid email address
                    Subject = subjectEmail,
                    Body = messageEmail,
                    IsBodyHtml = true
                };

                // Add recipient
                mailMessage.To.Add(new MailAddress(email));

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = enableSsl;

                    // Send the email asynchronously
                    await client.SendMailAsync(mailMessage);
                }

                return true; // Email sent successfully
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false; // Email sending failed
            }
        }

    }
}
