using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisteredUser.Database;
using RegisteredUser.Model;
using System.Drawing.Text;

namespace RegisteredUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskContext _context;
        public TaskController(TaskContext context) 
        {
            _context = context;
        }
        [HttpGet("GetAllTasks")]
        public async Task<ActionResult<IEnumerable<tblTask>>> Get()
        {
            if(_context.Tasks == null)
            {
                return NotFound();
            }
            return await _context.Tasks.ToListAsync();
  
        }
        [HttpPost("GetTasksByUser")]
        public async Task<ActionResult<IEnumerable<tblTask>>> GetTasks([FromBody]int userId)
        {
            /*var userIsValid = IsUserExists(Convert.ToInt32(userId));
            if(!userIsValid)
            {
                return BadRequest();
            }*/
           return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync(); 
        }

        [HttpGet("GetTaskById/{taskId}")]
        public IActionResult GetTaskById(int taskId)
        {
            var task = IsTaskExists(taskId);
            if(!task)
            {
                return NotFound();
            }
           var resultTask = _context.Tasks.FirstOrDefault(t=>t.TaskId == taskId);
            return Ok(resultTask);
        }
        

        [HttpPost("CreateTask")]
        public async Task<ActionResult<tblTask>> Create([FromBody] tblTask createdTask)
        {
            tblTask _task = GetCreatedTask(createdTask);
            
           _context.Tasks.Add(_task);
           await _context.SaveChangesAsync();

            var responseTask = _context.Tasks.ToList();
            return Ok(responseTask);
        }

        [HttpPut("UpdateTask")]
        public async Task<IActionResult> Update([FromBody] tblTask task)
        {
            var taskToUpdate = _context.Tasks.FirstOrDefault(t=>t.TaskId==task.TaskId);
            Console.WriteLine(taskToUpdate);
            if(taskToUpdate == null)
            {
                return NotFound();
            }
            
            var updatedTask = GetModifiedTask(taskToUpdate,task);
            
             _context.Entry(updatedTask).State = EntityState.Modified;

            try
            {
                 await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok();
        }

        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> Delete([FromBody] int TaskId)
        {
            var taskToDelete = await _context.Tasks.FindAsync(TaskId);
            if(taskToDelete == null)
            {
                return NotFound("User does not exist");
            }
            _context.Tasks.Remove(taskToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private tblTask GetCreatedTask(tblTask task) 
        {
            var _task = new tblTask();

            _task.UserId = Convert.ToInt32(task.UserId);
            _task.TaskName = task.TaskName;
            _task.TaskDescription = task.TaskDescription;
            _task.TaskCategory = task.TaskCategory;
            _task.TaskPriority = task.TaskPriority;
            _task.TaskStatus = task.TaskStatus;
            _task.TaskCreatedDate = Convert.ToDateTime(task.TaskCreatedDate);
            _task.TaskModifiedDate = Convert.ToDateTime(task.TaskModifiedDate);
            _task.TaskCompletedDate = Convert.ToDateTime(task.TaskModifiedDate);
            _task.TaskTargetDate = Convert.ToDateTime(task.TaskTargetDate);

            return _task;
        }

        private tblTask GetModifiedTask(tblTask dbtask, tblTask userTask)
        { 
            dbtask.UserId = userTask.UserId;
            dbtask.TaskName = userTask.TaskName;
            dbtask.TaskDescription = userTask.TaskDescription;
            dbtask.TaskCategory = userTask.TaskCategory;
            dbtask.TaskPriority = userTask.TaskPriority;
            dbtask.TaskStatus = userTask.TaskStatus;
            dbtask.TaskCreatedDate = Convert.ToDateTime(userTask.TaskCreatedDate);
            dbtask.TaskModifiedDate = Convert.ToDateTime(userTask.TaskModifiedDate);
            dbtask.TaskCompletedDate = Convert.ToDateTime(userTask.TaskModifiedDate);
            dbtask.TaskTargetDate = Convert.ToDateTime(userTask.TaskTargetDate);

            return dbtask;
        }
        private bool IsUserExists(int UserId)
        {
            return (bool)(_context.Tasks?.Any(t => t.UserId == UserId));
        }

        private bool IsTaskExists(int TaskId)
        {
            return (bool)_context.Tasks?.Any(t => t.TaskId == TaskId);
        }
    }
}
