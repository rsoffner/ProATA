using MediatR;
using TaskProcessing.Core.Models.ValueObjects;

namespace TaskProcesser.Commands
{
    public record RunTaskCommand(Guid TaskId) : IRequest<TaskResponse>;
}
