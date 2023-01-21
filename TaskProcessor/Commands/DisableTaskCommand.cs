using MediatR;
using TaskProcessing.Core.Models.ValueObjects;

namespace TaskProcesser.Commands
{
    public record DisableTaskCommand(Guid TaskId) : IRequest<TaskResponse>;
}
