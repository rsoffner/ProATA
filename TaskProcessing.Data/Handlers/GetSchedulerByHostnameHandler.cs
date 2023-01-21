using MediatR;
using TaskProcessing.Core.Models;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Queries;

namespace TaskProcessing.Data.Handlers
{
    public class GetSchedulerByHostnameHandler : IRequestHandler<GetSchedulerByHostnameQuery, Scheduler>
    {
        private readonly ISchedulerRepository _schedulerRepository;

        public GetSchedulerByHostnameHandler(ISchedulerRepository schedulerRepository)
        {
            _schedulerRepository = schedulerRepository;
        }

        public Task<Scheduler> Handle(GetSchedulerByHostnameQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_schedulerRepository.GetByHostName(request.Hostname));
        }
    }
}
