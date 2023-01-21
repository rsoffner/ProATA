using MediatR;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Data.Queries
{
    public record GetSchedulerByIdQuery(Guid Id) : IRequest<SchedulerDto>;
}
