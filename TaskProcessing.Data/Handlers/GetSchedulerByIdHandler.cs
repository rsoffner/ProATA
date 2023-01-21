using MediatR;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Entities;
using TaskProcessing.Data.Queries;

namespace TaskProcessing.Data.Handlers
{
    public class GetSchedulerByIdHandler : IRequestHandler<GetSchedulerByIdQuery, SchedulerDto>
    {
        private readonly ISchedulerRepository _schedulerRepository;

        public GetSchedulerByIdHandler(ISchedulerRepository schedulerRepository) 
        {
            _schedulerRepository = schedulerRepository;
        }  
        
        public Task<SchedulerDto> Handle(GetSchedulerByIdQuery request, CancellationToken cancellationToken)
        {
            var item = _schedulerRepository.GetById(request.Id);

            return Task.FromResult(new SchedulerDto
            {
                Id = item.Id,
                HostName = item.HostName,
                DefaultHost = item.DefaultHost,
            });
        }
    }
}
