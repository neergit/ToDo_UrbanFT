using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo_UrbanFt.dbContext;
using ToDo_UrbanFt.Entities;
using ToDo_UrbanFt.Models;

namespace ToDo_UrbanFt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController: ControllerBase
	{
        TodoListContext _context;
		public TaskController(TodoListContext context)
		{
            _context = context;
            _context.Database.EnsureCreated();
		}

		[HttpGet]
		public ActionResult GetTasks()
		{
            IEnumerable<Tasks> tasks = _context.Tasks;

            return Ok(tasks.ToArray());
		}

        [HttpGet("{id}")]
        public ActionResult GetTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task is null)
            {
                return NotFound();
            }
            return Ok(task);
        }

		[HttpPost]
		public ActionResult<Tasks> PostTask(TaskInput task)
		{
            Tasks tsk = new Tasks
            {
                Title = task.Title,
                Description = task.Description,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };
            if (!IsValidTask(tsk, out string message))
            {
                return BadRequest(message);
            }
            _context.Tasks.Add(tsk);
            _context.SaveChanges();

            return CreatedAtAction("GetTask", new { Id = tsk.Id }, tsk);
		}

        [HttpPut("{id}")]
        public ActionResult<Tasks> PutTask(int id, Tasks task)
        {
            //again validate the input task
            if (id != task.Id)
            {
                return BadRequest("Id and Task Id are different.");
            }
            if (!IsValidTask(task, out string message))
            {
                return BadRequest(message);
            }
            _context.Entry(task).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tasks.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTask", new { Id = task.Id }, task);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task is null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return Ok(task);
        }

        [HttpPost]
        [Route("Delete")]
        public ActionResult DeleteMultiple(int[] ids)
        {
            var tasks = new List<Tasks>();
            foreach (var id in ids)
            {
                var task = _context.Tasks.Find(id);

                if (task is null)
                {
                    return NotFound();
                }

                tasks.Add(task);
            }

            _context.Tasks.RemoveRange(tasks);
            _context.SaveChanges();

            return Ok(tasks);
        }

        private bool IsValidTask(Tasks task, out string message)
        {
            message = string.Empty;
            if (string.IsNullOrWhiteSpace(task.Title) ||
                string.IsNullOrWhiteSpace(task.Description) ||
                !(task.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase) ||
                 task.Status.Equals("In Progress", StringComparison.OrdinalIgnoreCase) ||
                 task.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase)))
            {
                message = "Title and Description can't be empty.Status can only be either 'Pending', 'In Progress' or 'Completed'";
                return false;
            }
            return true;
        } 
    }
}

