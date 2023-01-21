using MediatR;
using TaskProcessing.Core.Models.ValueObjects;

namespace TaskProcesser.Commands
{
    public record EndTaskCommand(Guid TaskId) : IRequest<TaskResponse>;
}
