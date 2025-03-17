using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using TaskManagementApiV2.ViewModels;
using TaskManagemen.Data;
using static TaskManagementApiV2.ViewModels.TaskCommentsModel;
using static TaskManagementApiV2.ViewModels.TaskImageModel;
using static TaskManagementApiV2.ViewModels.TaskModel;

namespace TaskManagementApiV2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")] // Use specific policy here
    public class TaskImageController : ControllerBase
    {
        private readonly ILogger<TaskImageController> _logger;
        private readonly ApplicationDbContext _context;

        public TaskImageController(ILogger<TaskImageController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("GetImage")]
        public IActionResult GetImage(Guid taskId)
        {
            try
            {
                var imageEntity = (from a in _context.TaskImage
                                   where a.TaskId == taskId
                                   && a.IsActive == true
                                   && a.IsDeleted == false
                                   orderby a.ModifyDate descending
                                   select a).FirstOrDefault();

                if (imageEntity == null)
                {
                    return NotFound(new { Message = "Image not found for the provided taskId." });
                }

                var base64Image = Convert.ToBase64String(imageEntity.Source);

                // Build the image URL with base64 string
                var imageUrl = $"data:{imageEntity.ContentType};ImageId:{imageEntity.ImageId};base64,{base64Image}";

                // Return both ImageId and imageUrl in the response
                return Ok(new
                {
                    ImageId = imageEntity.ImageId,  // Include the ImageId in the response
                    ImageUrl = imageUrl             // Include the imageUrl with base64 data
                });
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] Guid taskId, [FromForm] IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded or file is empty.");
                }

                if (taskId == Guid.Empty)
                {
                    return BadRequest("Invalid TaskId provided.");
                }

                var imageEntitys = (from a in _context.TaskImage
                                    where a.TaskId == taskId
                                    && a.IsActive == true
                                    && a.IsDeleted == false
                                    orderby a.ModifyDate ascending
                                    select a).ToList();

                if (imageEntitys != null && imageEntitys.Any())
                {
                    foreach (var image in imageEntitys)
                    {
                        image.IsActive = false;
                        image.IsDeleted = true;
                    }
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();

                    var imageEntity = new TaskImage
                    {
                        ImageId = Guid.NewGuid(),
                        TaskId = taskId, // Use the provided TaskId
                        ImageName = file.FileName,
                        Source = imageBytes,
                        ContentType = file.ContentType,
                        SizeInKB = ((decimal)file.Length / 1024),
                        SizeInMB = ((decimal)file.Length / 1024) / 1024,
                        CreateDate = DateTime.Now,
                        ModifyDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false
                    };

                    _context.TaskImage.Add(imageEntity);
                    await _context.SaveChangesAsync();

                    return Ok(new { Message = "Image uploaded and saved to database successfully!", ImageId = imageEntity.ImageId });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
