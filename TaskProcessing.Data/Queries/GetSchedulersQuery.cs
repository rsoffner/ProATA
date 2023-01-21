using MediatR;
using TaskProcessing.Core.Models.ValueObjects;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Data.Queries
{
    public record GetSchedulersQuery(DatatableOptions Options) : IRequest<IList<SchedulerDto>>;
}
