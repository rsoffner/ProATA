using JasperFx.Core;
using MediatR;
using ProATA.SharedKernel;
using TaskProcessing.Core.Repositories;
using TaskProcessing.Data.Entities;
using TaskProcessing.Data.Queries;

namespace TaskProcessing.Data.Handlers
{
    public class GetSchedulesByTaskHandler : IRequestHandler<GetSchedulesByTaskQuery, DatabaseResponse<ScheduleDto>>
    {
        private readonly IScheduleRepository _scheduleRepository;

        public GetSchedulesByTaskHandler(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public Task<DatabaseResponse<ScheduleDto>> Handle(GetSchedulesByTaskQuery request, CancellationToken cancellationToken)
        {
            IList<ScheduleDto> schedules = new List<ScheduleDto>();
            if (request.Options.Filters[0].Value != Guid.Empty.ToString())
            {
                var response = _scheduleRepository.GetSchedulesByTask(new Guid(request.Options.Filters[0].Value), request.Options.Paginate.Page, request.Options.Paginate.Limit);

                foreach (var item in response.Data)
                {
                    schedules.Add(new ScheduleDto
                    {
                        Id = item.Id,
                        Enabled = item.Enabled,
                        CronExpression = item.CronExpression,
                        Description = CronExpressionDescriptor.ExpressionDescriptor.GetDescription(item.CronExpression),
                        StartBoundery = item.StartBoundery,
                    });
                }

                return Task.FromResult(new DatabaseResponse<ScheduleDto>
                {
                    Data = schedules,
                    RecordsTotal = response.RecordsTotal,
                    RecordsFiltered = response.RecordsFiltered,
                });
            }
            else
            {
                return Task.FromResult(new DatabaseResponse<ScheduleDto>
                {
                    Data = schedules,
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                });
            }
        }
    }
}
