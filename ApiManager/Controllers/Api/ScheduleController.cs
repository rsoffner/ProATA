using ApiManager.Models;
using Microsoft.AspNetCore.Mvc;
using ProATA.Logic.Model.ValueObjects;
using ProATA.SharedKernel.Enums;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;

namespace ApiManager.Controllers.Api
{
    
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ITaskRepository _taskRepository;

        public ScheduleController(IScheduleRepository scheduleRepository, ITaskRepository taskRepository)
        {
            _scheduleRepository = scheduleRepository;
            _taskRepository = taskRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("api/schedule/create")]
        public IActionResult Create([FromForm] TaskViewModel data)
        {
            try
            {
                var task = _taskRepository.GetTask(data.Schedule.TaskId);

                Schedule schedule = new Schedule
                {
                    CronExpression = data.Schedule.CronExpression,
                    Enabled = data.Schedule.Enabled
                };

                schedule.Task = task;

                _scheduleRepository.AddSchedule(schedule);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private DaysOfTheWeek ToDaysOfTheWeek(IList<int> days)
        {
            DaysOfTheWeek daysOfTheWeek = 0;
            foreach (var day in days)
            {
                daysOfTheWeek |= (DaysOfTheWeek)day;
            }

            return daysOfTheWeek;
        }
    }
}
