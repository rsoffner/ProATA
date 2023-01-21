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
    public class ScheduleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ScheduleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<ScheduleController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ScheduleController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("task")]
        public async Task<DatabaseResponse<ScheduleDto>> GetSchedulesByTask(DatatableOptions options)
        {
            return await _mediator.Send(new GetSchedulesByTaskQuery(options));
        }

        // POST api/<ScheduleController>
        [HttpPost("task")]
        public async Task<DatabaseResponse<ScheduleDto>> Post([FromBody] DatatableOptions options)
        {
            return await _mediator.Send(new GetSchedulesByTaskQuery(options));
        }

        // PUT api/<ScheduleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ScheduleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
