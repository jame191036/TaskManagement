using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using TaskManagemen.Data;
using TaskManagementApiV2.ViewModels;
using static TaskManagementApiV2.ViewModels.TaskCommentsModel;
using static TaskManagementApiV2.ViewModels.TaskImageModel;
using static TaskManagementApiV2.ViewModels.TaskMasterStatusModel;
using static TaskManagementApiV2.ViewModels.TaskModel;
using static TaskManagementApiV2.ViewModels.NotificationEmailModel;
using System.Text.RegularExpressions;

namespace TaskManagementApiV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")] // Use specific policy here
    public class NotiEmailController : ControllerBase
    {
        private readonly ILogger<NotiEmailController> _logger;
        private readonly ApplicationDbContext _context;

        public NotiEmailController(ILogger<NotiEmailController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetNotiEmail")]
        public IActionResult GetNotiEmail()
        {
            var emailEntitys = (from a in _context.NotificationEmail
                                where a.IsActive == true && a.IsDeleted == false
                                orderby a.ModifyDate descending
                                select a).FirstOrDefault();

            if (emailEntitys == null)
            {
                return Ok("");
            }

            return Ok(emailEntitys.Email);
        }

        [HttpGet("UpdateNotiEmail")]
        public IActionResult UpdateNotiEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Invalid email");
                }

                if (!IsValidEmail(email))
                {
                    return BadRequest("Invalid email format.");
                }

                var emailEntitys = (from a in _context.NotificationEmail
                                    where a.IsActive == true && a.IsDeleted == false
                                    select a).ToList();

                if (emailEntitys != null && emailEntitys.Any())
                {
                    foreach (var image in emailEntitys)
                    {
                        image.IsActive = false;
                        image.IsDeleted = true;
                    }
                }

                var newEmailNoti = new NotificationEmailModel.NotificationEmail
                {
                    EmailId = Guid.NewGuid(), // Generate a new GUID for TaskId
                    Email = email,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false // Default to false for a new task
                };

                _context.NotificationEmail.Add(newEmailNoti);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}
