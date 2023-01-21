using JasperFx.Core;
using MediatR;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Entities;
using TaskProcessing.Data.Queries;

namespace TaskProcessing.Data.Handlers
{
    public class GetSchedulersHandler : IRequestHandler<GetSchedulersQuery, IList<SchedulerDto>>
    {
        private readonly ISchedulerRepository _schedulerRepository;

        public GetSchedulersHandler(ISchedulerRepository schedulerRepository)
        {
            _schedulerRepository = schedulerRepository;
        }

        public Task<IList<SchedulerDto>> Handle(GetSchedulersQuery request, CancellationToken cancellationToken)
        {
            var items = _schedulerRepository.GetSchedulers();
            IList<SchedulerDto> schedulers = new List<SchedulerDto>();
            foreach (var scheduler in items)
            {
                schedulers.Add(new SchedulerDto
                {
                    Id = scheduler.Id,
                    HostName = scheduler.HostName,
                    DefaultHost = scheduler.DefaultHost,
                });
            }

            return Task.FromResult(schedulers);
        }
    }
}
