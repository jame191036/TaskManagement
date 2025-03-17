using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using TaskManagemen.Data;
using TaskManagementApiV2.ViewModels;
using static TaskManagementApiV2.ViewModels.TaskCommentsModel;
using static TaskManagementApiV2.ViewModels.TaskImageModel;
using static TaskManagementApiV2.ViewModels.TaskMasterStatusModel;
using static TaskManagementApiV2.ViewModels.TaskModel;

namespace TaskManagementApiV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")] // Use specific policy here
    public class TaskMasterController : ControllerBase
    {
        private readonly ILogger<TaskMasterController> _logger;
        private readonly ApplicationDbContext _context;

        public TaskMasterController(ILogger<TaskMasterController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetMasterStatus")]
        public IActionResult GetMasterStatus()
        {
            try
            {
                var task1s = (from a in _context.TaskMasterStatus
                              where a.IsActive == true
                              && a.IsDeleted == false
                              orderby a.ModifyDate ascending
                              select new MasterStatus
                              {
                                  Label = a.StatusName ?? "",
                                  Value = a.StatusCode ?? ""
                              }).ToList();

                return Ok(task1s);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
