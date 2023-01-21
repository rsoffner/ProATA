using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProATA.SharedKernel;
using TaskProcessing.Core.Models.ValueObjects;
using TaskProcessing.Data.Entities;
using TaskProcessing.Data.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProATA.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<TaskController>
        [HttpGet]
        public async Task<DatabaseResponse<APITaskDto>> Get(DatatableOptions options)
        {
            return await _mediator.Send(new GetTasksQuery(options));
        }

        // GET api/<TaskController>/7f14d34d-a8db-4237-95f0-ac14008dda2d
        [HttpGet("{id}")]
        public async Task<APITaskDto> Get(Guid id)
        {
            return await _mediator.Send(new GetTaskByIdQuery(id));
        }

        // POST api/<TaskController>
        [HttpPost]
        public async Task<DatabaseResponse<APITaskDto>> Post([FromBody] DatatableOptions options)
        {
            return await _mediator.Send(new GetTasksQuery(options));
        }

        // PUT api/<TaskController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TaskController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
