using MediatR;
using TaskProcessing.Data.Entities;

namespace TaskProcessing.Data.Queries
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<APITaskDto>;
}
