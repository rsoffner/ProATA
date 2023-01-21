using MediatR;
using TaskProcessing.Core.Models.ValueObjects;

namespace TaskProcesser.Commands
{
    public record EnableTaskCommand(Guid TaskId) : IRequest<TaskResponse>;
}
