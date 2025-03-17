using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementApiV2.ViewModels;
using TaskManagemen.Data;
using static TaskManagementApiV2.ViewModels.TaskCommentsModel;
using static TaskManagementApiV2.ViewModels.TaskModel;

namespace TaskManagementApiV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")] // Use specific policy here
    public class TaskCommentsController : ControllerBase
    {
        private readonly ILogger<TaskCommentsController> _logger;
        private readonly ApplicationDbContext _context;

        public TaskCommentsController(ILogger<TaskCommentsController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetTaskCommentById/{taskId}")]
        public IActionResult GetTaskCommentById(Guid taskId)
        {
            try
            {
                var result = (from a in _context.TaskComments
                              where a.TaskId == taskId
                              && a.IsActive == true
                              && a.IsDeleted == false
                              orderby a.ModifyDate ascending
                              select a).ToList();

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

        [HttpPost("InsertTaskComments")]
        public IActionResult InsertTaskComments([FromBody] InsertTaskComment model)
        {
            try
            {
                if ((model == null && model.TaskId == null) || string.IsNullOrEmpty(model.Description))
                {
                    return BadRequest("Invalid data.");
                }

                var newComment = new TaskCommentsModel.TaskComments
                {
                    CommentId = Guid.NewGuid(),
                    TaskId = model.TaskId, // Generate a new GUID for TaskId
                    Description = model.Description,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false // Default to false for a new task
                };

                _context.TaskComments.Add(newComment);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
