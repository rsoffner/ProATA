using Microsoft.AspNetCore.Mvc;
using TaskProcessing.Core.Repositories;

namespace ApiManager.Controllers.Api
{
    
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        
    }
}
