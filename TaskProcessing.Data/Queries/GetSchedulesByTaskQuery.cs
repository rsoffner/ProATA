using MediatR;
using ProATA.SharedKernel;
using TaskProcessing.Core.Models.ValueObjects;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Data.Queries
{
    public record GetSchedulesByTaskQuery(DatatableOptions Options) : IRequest<DatabaseResponse<ScheduleDto>>;
}