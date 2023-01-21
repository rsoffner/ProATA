using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskProcessing.Core.Models.ValueObjects;
using TaskProcessing.Data.Entities;
using TaskProcessing.Data.Queries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProATA.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SchedulerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<SchedulerController>
        [HttpGet]
        public async Task<IList<SchedulerDto>> Get(DatatableOptions options)
        {
            return await _mediator.Send(new GetSchedulersQuery(options));
        }

        // GET api/<SchedulerController>/7f14d34d-a8db-4237-95f0-ac14008dda2d
        [HttpGet("{id}")]
        public async Task<SchedulerDto> Get(Guid id)
        {
            return await _mediator.Send(new GetSchedulerByIdQuery(id));
        }

        // POST api/<SchedulerController>
        [HttpPost]
        public async Task<IList<SchedulerDto>> Post([FromBody] DatatableOptions options)
        {
            return await _mediator.Send(new GetSchedulersQuery(options));
        }

        // PUT api/<SchedulerController>/7f14d34d-a8db-4237-95f0-ac14008dda2d
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SchedulerController>/7f14d34d-a8db-4237-95f0-ac14008dda2d
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
