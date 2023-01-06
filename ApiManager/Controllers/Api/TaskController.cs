using ApiManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;

namespace ApiManager.Controllers.Api
{
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ISchedulerRepository _schedulerRepository;

        public TaskController(ITaskRepository taskRepository, ISchedulerRepository schedulerRepository)
        {
            _taskRepository = taskRepository;
            _schedulerRepository = schedulerRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/task/create")]
        public IActionResult Create([FromForm] TaskViewModel data)
        {
            Scheduler scheduler = _schedulerRepository.GetById(data.SchedulerId);
            APITask task = new APITask(Guid.NewGuid(), data.Title, data.Enabled, scheduler);
            _taskRepository.AddTask(task);

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [Route("api/task/delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            APITask task = _taskRepository.GetTask(id);

            if (task != null) 
            {
                _taskRepository.DeleteTask(task);

                return Ok(id);
            }
            else
            {
                return NotFound(id);
            }
        }

    }
}
