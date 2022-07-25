using Microsoft.AspNetCore.Mvc;
using ProATA.SharedKernel.Interfaces;
using static TaskProcessing.Core.Contracts.Tasks;

namespace TaskManager.Api.Controllers
{
    [Route("api/task")]
    [ApiController]
    public class TaskCommandsApi : ControllerBase
    {
        private readonly IHandleCommand<V1.Run> _runTaskCommandHandler;

        public TaskCommandsApi(IHandleCommand<V1.Run> runTaskCommandHandler)
        {
            _runTaskCommandHandler = runTaskCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Post(V1.Run request)
        {
            await _runTaskCommandHandler.Handle(request);

            return Ok();
        }
    }
}
